using Rotativa.Options;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rotativa
{
    public abstract class AsPdf : AsResultBase
    {
        protected AsPdf()
        {
            this.PageMargins = new Margins();
        }

        public Uri BaseUrl { get; set; }
        public string Html { get; set; }

        /// <summary>
        /// Sets the page size.
        /// </summary>
        [OptionFlag("-s")]
        public Size? PageSize { get; set; }

        /// <summary>
        /// Sets the page width in mm.
        /// </summary>
        /// <remarks>Has priority over <see cref="PageSize"/> but <see cref="PageHeight"/> has to be also specified.</remarks>
        [OptionFlag("--page-width")]
        public double? PageWidth { get; set; }

        /// <summary>
        /// Sets the page height in mm.
        /// </summary>
        /// <remarks>Has priority over <see cref="PageSize"/> but <see cref="PageWidth"/> has to be also specified.</remarks>
        [OptionFlag("--page-height")]
        public double? PageHeight { get; set; }

        /// <summary>
        /// Sets the page orientation.
        /// </summary>
        [OptionFlag("-O")]
        public Orientation? PageOrientation { get; set; }

        /// <summary>
        /// Sets the page margins.
        /// </summary>
        public Margins PageMargins { get; set; }

        protected override byte[] WkhtmlConvert(string switches)
        {
            return WkhtmltopdfDriver.Convert(this.WkhtmlPath, switches);
        }

        /// <summary>
        /// Indicates whether the PDF should be generated in lower quality.
        /// </summary>
        [OptionFlag("-l")]
        public bool IsLowQuality { get; set; }

        /// <summary>
        /// Number of copies to print into the PDF file.
        /// </summary>
        [OptionFlag("--copies")]
        public int? Copies { get; set; }

        /// <summary>
        /// Indicates whether the PDF should be generated in grayscale.
        /// </summary>
        [OptionFlag("-g")]
        public bool IsGrayScale { get; set; }

        protected override string GetConvertOptions()
        {
            var result = new StringBuilder();

            if (this.PageMargins != null)
                result.Append(this.PageMargins.ToString());

            result.Append(" ");
            result.Append(base.GetConvertOptions());

            return result.ToString().Trim();
        }

        protected override Task<byte[]> CallTheDriver()
        {
            string baseUrl = string.Format("{0}://{1}", BaseUrl.Scheme, BaseUrl.Host);
            var htmlForWkhtml = Regex.Replace(Html, "<head>", string.Format("<head><base href=\"{0}\" />", baseUrl), RegexOptions.IgnoreCase);

            byte[] fileContent = WkhtmltopdfDriver.ConvertHtml(this.WkhtmlPath, this.GetConvertOptions(), htmlForWkhtml);
            return Task.FromResult(fileContent);
        }
    }
}