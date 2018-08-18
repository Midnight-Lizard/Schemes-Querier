using SemVer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidnightLizard.Schemes.Querier.Serialization
{
    public interface IModelMetadata
    {
        string Type { get; set; }
        Range VersionRange { get; set; }
    }

    public class ModelAttribute : Attribute, IModelMetadata
    {
        public Range VersionRange { get; set; }

        /// <summary>
        /// SemVer range syntax
        /// </summary>
        public string Version
        {
            get
            {
                return VersionRange.ToString();
            }
            set
            {
                VersionRange = new SemVer.Range(value);
            }
        }

        /// <summary>
        /// Custom message type.
        /// No need to provide if it exactly matches with 
        /// <see cref="IVersionedModelDeserializer"/> generic type parameter TModel type name
        /// </summary>
        public string Type { get; set; }
    }
}
