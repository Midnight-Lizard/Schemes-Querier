using MidnightLizard.Schemes.Querier.Models;
using System;

namespace MidnightLizard.Schemes.Querier.Serialization
{
    [Model(Version = "1.0")]
    public class PublicSchemeModelDeserializer_v1_0 : PublicSchemeModelDeserializer_v1_1
    {
        public override void StartAdvancingToTheLatestVersion(PublicScheme model)
        {
            base.AdvanceToTheLatestVersion(model);
        }
    }

    [Model(Version = "1.1")]
    public class PublicSchemeModelDeserializer_v1_1 : PublicSchemeModelDeserializer_v1_2
    {
        public override void StartAdvancingToTheLatestVersion(PublicScheme model)
        {
            base.AdvanceToTheLatestVersion(model);
        }

        protected override void AdvanceToTheLatestVersion(PublicScheme model)
        {
            // in version 1.1 scrollbar size and image hover options are added
            var cs = model.ColorScheme;
            cs.scrollbarSize = 10;//px
            cs.useImageHoverAnimation = cs.imageLightnessLimit > 80;

            base.AdvanceToTheLatestVersion(model);
        }
    }

    [Model(Version = "1.2")]
    public class PublicSchemeModelDeserializer_v1_2 : PublicSchemeModelDeserializer_v1_3
    {
        public override void StartAdvancingToTheLatestVersion(PublicScheme model)
        {
            base.AdvanceToTheLatestVersion(model);
        }

        protected override void AdvanceToTheLatestVersion(PublicScheme model)
        {
            // in version 1.2 button component is added
            var cs = model.ColorScheme;
            cs.buttonSaturationLimit = (int)Math.Round(Math.Min(cs.backgroundSaturationLimit * 1.1, 100));
            cs.buttonContrast = cs.backgroundContrast;
            cs.buttonLightnessLimit = (int)Math.Round(cs.backgroundLightnessLimit * 0.8);
            cs.buttonGraySaturation = (int)Math.Round(Math.Min(cs.backgroundGraySaturation * 1.1, 100));
            cs.buttonGrayHue = cs.borderGrayHue;

            base.AdvanceToTheLatestVersion(model);
        }
    }

    [Model(Version = ">=1.3 <9.3")]
    public class PublicSchemeModelDeserializer_v1_3 : PublicSchemeModelDeserializer_v9_3
    {
        public override void StartAdvancingToTheLatestVersion(PublicScheme model)
        {
            base.AdvanceToTheLatestVersion(model);
        }

        protected override void AdvanceToTheLatestVersion(PublicScheme model)
        {
            // in version 1.3 option to ignore color hues is added
            var cs = model.ColorScheme;
            cs.linkReplaceAllHues = true;

            base.AdvanceToTheLatestVersion(model);
        }
    }

    [Model(Version = ">=9.3 <10.1")]
    public class PublicSchemeModelDeserializer_v9_3 : PublicSchemeModelDeserializer_v10_1
    {
        public override void StartAdvancingToTheLatestVersion(PublicScheme model)
        {
            base.AdvanceToTheLatestVersion(model);
        }

        protected override void AdvanceToTheLatestVersion(PublicScheme model)
        {
            var cs = model.ColorScheme;

            cs.doNotInvertContent = false;
            cs.mode = "auto";
            cs.modeAutoSwitchLimit = 5000;
            cs.includeMatches = null;
            cs.excludeMatches = null;
            cs.backgroundHueGravity = 0;
            cs.buttonHueGravity = 0;
            cs.textHueGravity = 0;
            cs.linkHueGravity = 80;
            cs.borderHueGravity = 0;
            cs.scrollbarStyle = "true";

            base.AdvanceToTheLatestVersion(model);
        }
    }

    [Model(Version = ">=10.1")]
    public class PublicSchemeModelDeserializer_v10_1 : AbstractVersionedModelDeserializer<PublicScheme>
    {
        public override void StartAdvancingToTheLatestVersion(PublicScheme model)
        {
            base.AdvanceToTheLatestVersion(model);
        }

        protected override void AdvanceToTheLatestVersion(PublicScheme model)
        {
            var cs = model.ColorScheme;

            cs.hideBigBackgroundImages = true;
            cs.maxBackgroundImageSize = 500;

            base.AdvanceToTheLatestVersion(model);
        }
    }
}
