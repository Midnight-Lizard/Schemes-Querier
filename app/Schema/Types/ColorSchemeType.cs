using GraphQL.Types;
using MidnightLizard.Schemes.Querier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidnightLizard.Schemes.Querier.Schema.Types
{
    public class ColorSchemeType: ObjectGraphType<ColorScheme>
    {
        public ColorSchemeType()
        {
            Field(x => x.colorSchemeId);
            Field(x => x.colorSchemeName);
            Field(x => x.runOnThisSite);
            Field(x => x.blueFilter);
            Field(x => x.useDefaultSchedule);
            Field(x => x.scheduleStartHour);
            Field(x => x.scheduleFinishHour);
            Field(x => x.backgroundSaturationLimit);
            Field(x => x.backgroundContrast);
            Field(x => x.backgroundLightnessLimit);
            Field(x => x.backgroundGraySaturation);
            Field(x => x.backgroundGrayHue);
            Field(x => x.textSaturationLimit);
            Field(x => x.textContrast);
            Field(x => x.textLightnessLimit);
            Field(x => x.textGraySaturation);
            Field(x => x.textGrayHue);
            Field(x => x.textSelectionHue);
            Field(x => x.linkSaturationLimit);
            Field(x => x.linkContrast);
            Field(x => x.linkLightnessLimit);
            Field(x => x.linkDefaultSaturation);
            Field(x => x.linkDefaultHue);
            Field(x => x.linkVisitedHue);
            Field(x => x.borderSaturationLimit);
            Field(x => x.borderContrast);
            Field(x => x.borderLightnessLimit);
            Field(x => x.borderGraySaturation);
            Field(x => x.borderGrayHue);
            Field(x => x.imageLightnessLimit);
            Field(x => x.imageSaturationLimit);
            Field(x => x.backgroundImageLightnessLimit);
            Field(x => x.backgroundImageSaturationLimit);
            Field(x => x.scrollbarSaturationLimit);
            Field(x => x.scrollbarContrast);
            Field(x => x.scrollbarLightnessLimit);
            Field(x => x.scrollbarGrayHue);
            Field(x => x.buttonSaturationLimit);
            Field(x => x.buttonContrast);
            Field(x => x.buttonLightnessLimit);
            Field(x => x.buttonGraySaturation);
            Field(x => x.buttonGrayHue);
            Field(x => x.backgroundReplaceAllHues);
            Field(x => x.borderReplaceAllHues);
            Field(x => x.buttonReplaceAllHues);
            Field(x => x.linkReplaceAllHues);
            Field(x => x.textReplaceAllHues);
            Field(x => x.useImageHoverAnimation);
            Field(x => x.scrollbarSize);
            Field(x => x.doNotInvertContent);
            Field(x => x.mode);
            Field(x => x.modeAutoSwitchLimit);
            Field(x => x.includeMatches);
            Field(x => x.excludeMatches);
            Field(x => x.backgroundHueGravity);
            Field(x => x.buttonHueGravity);
            Field(x => x.textHueGravity);
            Field(x => x.linkHueGravity);
            Field(x => x.borderHueGravity);
            Field(x => x.scrollbarStyle);
        }
    }
}
