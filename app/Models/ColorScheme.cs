﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidnightLizard.Schemes.Querier.Models
{
    public class ColorScheme
    {
        public string colorSchemeId { get; set; }
        public string colorSchemeName { get; set; }

        public bool runOnThisSite { get; set; }
        public int blueFilter { get; set; }
        public bool doNotInvertContent { get; set; }
        public string mode { get; set; }
        public int modeAutoSwitchLimit { get; set; }

        public bool useDefaultSchedule { get; set; }
        public int scheduleStartHour { get; set; }
        public int scheduleFinishHour { get; set; }

        public string includeMatches { get; set; }
        public string excludeMatches { get; set; }

        public int backgroundSaturationLimit { get; set; }
        public int backgroundContrast { get; set; }
        public int backgroundLightnessLimit { get; set; }
        public int backgroundGraySaturation { get; set; }
        public int backgroundGrayHue { get; set; }
        public bool backgroundReplaceAllHues { get; set; }
        public int backgroundHueGravity { get; set; }

        public int buttonSaturationLimit { get; set; }
        public int buttonContrast { get; set; }
        public int buttonLightnessLimit { get; set; }
        public int buttonGraySaturation { get; set; }
        public int buttonGrayHue { get; set; }
        public bool buttonReplaceAllHues { get; set; }
        public int buttonHueGravity { get; set; }

        public int textSaturationLimit { get; set; }
        public int textContrast { get; set; }
        public int textLightnessLimit { get; set; }
        public int textGraySaturation { get; set; }
        public int textGrayHue { get; set; }
        public int textSelectionHue { get; set; }
        public bool textReplaceAllHues { get; set; }
        public int textHueGravity { get; set; }

        public int linkSaturationLimit { get; set; }
        public int linkContrast { get; set; }
        public int linkLightnessLimit { get; set; }
        public int linkDefaultSaturation { get; set; }
        public int linkDefaultHue { get; set; }
        public int linkVisitedHue { get; set; }
        public bool linkReplaceAllHues { get; set; }
        public int linkHueGravity { get; set; }

        public int borderSaturationLimit { get; set; }
        public int borderContrast { get; set; }
        public int borderLightnessLimit { get; set; }
        public int borderGraySaturation { get; set; }
        public int borderGrayHue { get; set; }
        public bool borderReplaceAllHues { get; set; }
        public int borderHueGravity { get; set; }

        public int imageLightnessLimit { get; set; }
        public int imageSaturationLimit { get; set; }
        public bool useImageHoverAnimation { get; set; }

        public int backgroundImageLightnessLimit { get; set; }
        public int backgroundImageSaturationLimit { get; set; }

        public int scrollbarSaturationLimit { get; set; }
        public int scrollbarContrast { get; set; }
        public int scrollbarLightnessLimit { get; set; }
        public int scrollbarGrayHue { get; set; }
        public int scrollbarSize { get; set; }
        public string scrollbarStyle { get; set; }
    }
}
