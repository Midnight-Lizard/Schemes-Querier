using System;

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

        public ScreenshotUrls()
        {
        }

        public ScreenshotUrls(bool randomScreenshots)
        {
            var i = new Random().Next(0, screenshots.Length - 1);
            this.xs = this.sm = this.md = this.lg = this.xl = screenshots[i];
        }

        private static readonly string[] screenshots = {
            "https://lh3.googleusercontent.com/xGFHyTjX8IWoU8W4GypW-rKZ-O83MDhP3dr5sBWvj0cpG7ITyD-GkhVk1jE-bLtyDO5FtDReCLdPUA-J51-vbD13ibL54_CIMagf=w1920-h1200",
            "https://lh3.googleusercontent.com/QiZPT1yLxr0-Ob9QuDtrshGO6Fsw701tL1yKCWq5aqHLjBrsRbmSXKKTi3mK9b4pXhGyU8vi1X1McamyzYGkT-YyjGmXLmXkXNtC=w1920-h1200",
            "https://lh3.googleusercontent.com/dzvDXQGLmzdBwaxU5qyCe08ewaFskfISVCDgtH14V57GdNRXe6WlbO8pGX20KDybqecFYWNjtLDQUuMwLuceUp0iNam9cRo_0hlE=w1920-h1200",
            "https://lh3.googleusercontent.com/2elQVTWkqkLpWtqOtm6Qirt_Wl0thuhyEhQqCxzlR1MbmdSkhN4uaFKitqHtxCH2H_gpz0vuMkPOhmuOI05tOAgFdfM8cXbKLAxp=w1920-h1200",
            "https://lh3.googleusercontent.com/szBFviBG1jz3r6jopNGLkHRL-raFjLuYeCmJ5as9G1vVme9AyEFSwa8qEowxhat46bpZwK-iAMuPY9vRhhrA_5ykuQRXklpfhHhs=w1920-h1200",
            "https://lh3.googleusercontent.com/iv67RC1_Dncgw4Ylhq-DjTZzYaivx21vaTM6_uQ-x5TLkwo4lTu2MOYsINVIyZ5CxvXgzR5gZ4UZDHa-6CeQyA_xgbPG5p-nZp8=w1920-h1200"
        };
    }
}
