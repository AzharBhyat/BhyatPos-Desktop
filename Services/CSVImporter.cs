using System;
using System.Globalization;
using Avalonia.Controls.Documents;
using CsvHelper;
using CsvHelper.Configuration;

namespace BhyatPos.Services
{
    public class CSVImporter
    {
        // product importer
        // inventory importer
        // other

        private string _csvPath;
        CsvConfiguration _csvConfig;

        public CSVImporter(string csvPath) 
        {
            _csvPath = csvPath;

            _csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
             {
                 HasHeaderRecord = false
             };

        }

        public bool importProducts()
        {

            return _csvPath != null;
        }
    }
}
