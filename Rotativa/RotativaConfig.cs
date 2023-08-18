using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Rotativa
{
    public static class RotativaConfiguration
    {
        private static string _RotativaPath;
        internal static string RotativaPath => _RotativaPath;

        /// <summary>
        /// Setup Rotativa library
        /// </summary>
        /// <param name="rootPath">The path to the web-servable application files.</param>
        /// <param name="wkhtmltopdfRelativePath">Optional. Relative path to the directory containing wkhtmltopdf.exe. Default is "Rotativa". Download at https://wkhtmltopdf.org/downloads.html</param>
        public static void Setup(string rootPath, string wkhtmltopdfRelativePath = "Rotativa")
        {
            var rotativaPath = Path.Combine(rootPath, wkhtmltopdfRelativePath);

            if (!Directory.Exists(rotativaPath))
            {
                throw new ApplicationException("Folder containing wkhtmltopdf.exe not found, searched for " + rotativaPath);
            }

            _RotativaPath = rotativaPath;
        }

    }
}
