using System;
using System.Text.Json.Serialization;

namespace GPMDP.Release.Data
{
    public class BuildRequest
    {
        public string Platform { get; set; }

        public string Architecture { get; set; }

        public string PackageType {get;set;}
    }

    public class CircleCIBuild {
        [JsonPropertyName("build_num")]
        public int Number {get;set;}

        [JsonPropertyName("failed")]
        public bool Failed {get;set;}

        [JsonPropertyName("has_artifacts")]
        public bool HasArtifacts {get;set;}
    }
}
