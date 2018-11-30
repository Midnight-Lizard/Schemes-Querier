using GraphQL.Types;
using MidnightLizard.Schemes.Querier.Models;

namespace MidnightLizard.Schemes.Querier.Schema.Types
{
    public class ColorSchemeType : ObjectGraphType<ColorScheme>
    {
        public ColorSchemeType()
        {
            this.Field(x => x.colorSchemeId);
            this.Field(x => x.colorSchemeName);
            this.Field(x => x.runOnThisSite);
            this.Field(x => x.blueFilter);
            this.Field(x => x.useDefaultSchedule);
            this.Field(x => x.scheduleStartHour);
            this.Field(x => x.scheduleFinishHour);
            this.Field(x => x.backgroundSaturationLimit);
            this.Field(x => x.backgroundContrast);
            this.Field(x => x.backgroundLightnessLimit);
            this.Field(x => x.backgroundGraySaturation);
            this.Field(x => x.backgroundGrayHue);
            this.Field(x => x.textSaturationLimit);
            this.Field(x => x.textContrast);
            this.Field(x => x.textLightnessLimit);
            this.Field(x => x.textGraySaturation);
            this.Field(x => x.textGrayHue);
            this.Field(x => x.textSelectionHue);
            this.Field(x => x.linkSaturationLimit);
            this.Field(x => x.linkContrast);
            this.Field(x => x.linkLightnessLimit);
            this.Field(x => x.linkDefaultSaturation);
            this.Field(x => x.linkDefaultHue);
            this.Field(x => x.linkVisitedHue);
            this.Field(x => x.borderSaturationLimit);
            this.Field(x => x.borderContrast);
            this.Field(x => x.borderLightnessLimit);
            this.Field(x => x.borderGraySaturation);
            this.Field(x => x.borderGrayHue);
            this.Field(x => x.imageLightnessLimit);
            this.Field(x => x.imageSaturationLimit);
            this.Field(x => x.backgroundImageLightnessLimit);
            this.Field(x => x.backgroundImageSaturationLimit);
            this.Field(x => x.scrollbarSaturationLimit);
            this.Field(x => x.scrollbarContrast);
            this.Field(x => x.scrollbarLightnessLimit);
            this.Field(x => x.scrollbarGrayHue);
            this.Field(x => x.buttonSaturationLimit);
            this.Field(x => x.buttonContrast);
            this.Field(x => x.buttonLightnessLimit);
            this.Field(x => x.buttonGraySaturation);
            this.Field(x => x.buttonGrayHue);
            this.Field(x => x.backgroundReplaceAllHues);
            this.Field(x => x.borderReplaceAllHues);
            this.Field(x => x.buttonReplaceAllHues);
            this.Field(x => x.linkReplaceAllHues);
            this.Field(x => x.textReplaceAllHues);
            this.Field(x => x.useImageHoverAnimation);
            this.Field(x => x.scrollbarSize);
            this.Field(x => x.doNotInvertContent);
            this.Field(x => x.mode);
            this.Field(x => x.modeAutoSwitchLimit);
            this.Field(x => x.includeMatches, nullable: true);
            this.Field(x => x.excludeMatches, nullable: true);
            this.Field(x => x.backgroundHueGravity);
            this.Field(x => x.buttonHueGravity);
            this.Field(x => x.textHueGravity);
            this.Field(x => x.linkHueGravity);
            this.Field(x => x.borderHueGravity);
            this.Field(x => x.scrollbarStyle, nullable: true);
            this.Field(x => x.hideBigBackgroundImages);
            this.Field(x => x.maxBackgroundImageSize);
        }
    }
}
