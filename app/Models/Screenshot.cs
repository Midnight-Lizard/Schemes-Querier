using System;
using MidnightLizard.Schemes.Querier.Configuration;

namespace MidnightLizard.Schemes.Querier.Models
{
    public class Screenshot
    {
        public string Title { get; set; }
        public ScreenshotUrls Urls { get; set; }
    }

    public class ScreenshotUrls
    {
        public string xs { get; set; }
        public string sm { get; set; }
        public string md { get; set; }
        public string lg { get; set; }
        public string xl { get; set; }
    }
}
