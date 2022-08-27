using Colorify;
using Colorify.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using FileImporter.Data.Context;
using System.Numerics;
using System.Net;
using System.Diagnostics.Metrics;
using ShellProgressBar;


namespace FileImporter.Workers;

public class Importer
{
    private readonly IConfiguration _configuration;
    private readonly ContosoContext _context;
    private static Format _colorify { get; set; }

    public Importer(IConfiguration configuration, ContosoContext context) 
    {
        _configuration = configuration;
        _context = context;
        _colorify = new Format(Theme.Dark);
    }

    public Task Import() 
    {
        _colorify.Write("Having fun importing files \r\n", Colors.bgPrimary);
        _colorify.Wrap("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer sed turpis in ligula aliquet ornare tristique sed ante. Nam pretium ullamcorper condimentum. Aliquam quis sodales ex, vitae gravida metus. Suspendisse potenti. Maecenas nunc sapien, semper vel tincidunt sed, scelerisque ut est. Nunc eu venenatis libero. Nulla consectetur pretium leo. Nullam suscipit scelerisque neque fringilla volutpat. Aliquam condimentum, neque quis malesuada ultrices, mauris velit tincidunt arcu, vel sodales tortor felis quis velit. Aliquam tempus ullamcorper orci, vitae pretium leo maximus ut. Aliquam iaculis leo sed tempor mattis.\r\n", Colors.bgWarning);

        Console.Write("\r\n");

        var csvDirectory = _configuration["FileImportSettings:CsvDirectory"].ToString();
        var directory = new DirectoryInfo(csvDirectory);
        var files = directory.GetFiles("*.CSV", System.IO.SearchOption.AllDirectories).ToList();

        using (var progressBar = new ProgressBar(files.Count, "A cool progress Bar to track the import process"))
        {
            Execute(files, progressBar.AsProgress<FileInfo>(x => $"File Processed: {x.Name}"));
        }

        return Task.FromResult(1);
    }

    private void Execute(IEnumerable<FileInfo> files, IProgress<FileInfo> progress) 
    {
        foreach (var file in files) 
        {
            ProcessFile(file);
            RegisterFileName(file);

            progress?.Report(file);
        }
    }

    private void ProcessFile(FileInfo file) 
    {
        using (TextFieldParser parser = new TextFieldParser(file.FullName)) 
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            while (!parser.EndOfData) 
            {
                var fields = parser.ReadFields();

                if (fields != null)
                {
                    _context.LeadsData.Add(
                        new Data.Models.LeadsData() 
                        {
                            Name = Convert.ToString(fields[0]),
                            Phone = Convert.ToString(fields[1]),
                            Email = Convert.ToString(fields[2]),
                            Address = Convert.ToString(fields[3]),
                            PostalZip = Convert.ToString(fields[4]),
                            Country = Convert.ToString(fields[5])
                        });
                }
            }

            _context.SaveChanges();
        }
    }

    private void RegisterFileName(FileInfo file) 
    {
        _context.Files.Add(
            new Data.Models.File()
            {
                FileName = file.FullName
            });

        _context.SaveChanges(true);
    }












}
