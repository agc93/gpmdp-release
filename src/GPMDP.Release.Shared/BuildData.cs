using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GPMDP.Release
{
    public static class CoreExtensions
    {
        internal static bool IsEmpty(this string s) {
            return string.IsNullOrWhiteSpace(s) && s != "_";
        }
    }

    public class BuildRequest
    {
        public BuildRequest() {}

        public BuildRequest(string platform, string package, string arch)
        {
            Platform = platform;
            PackageType = package;
            Architecture = arch;
        }
        public string Platform { get; set; }

        public string Architecture { get; set; }

        public string PackageType {get;set;}

        public string ToMatchString() {
            switch (Platform)
            {
                case "windows":
                    return ".exe";
                case "macos":
                    return ".zip";
                case "linux":
                    if (PackageType == "deb") {
                        return $"_{(Architecture == "x64" ? "amd64" : "i386")}.deb";
                    } else {
                        return $".{(Architecture == "x64" ? "x86_64" : "i386")}.rpm";
                    }
                default:
                    return null;
            }
        }
    }

    public class CircleCIBuild {
        [JsonPropertyName("build_num")]
        public int Number {get;set;}

        [JsonPropertyName("failed")]
        public bool Failed {get;set;}

        [JsonPropertyName("has_artifacts")]
        public bool HasArtifacts {get;set;}

        [JsonPropertyName("branch")]
        public string Branch {get;set;}

        [JsonPropertyName("workflows")]
        public CircleCIWorkflow Workflows {get;set;}

        public string JobName => Workflows?.JobName;
    }

    public class CircleCIWorkflow {
        [JsonPropertyName("job_name")]
        public string JobName {get;set;}
    }

    public class CircleCIArtifact {
        [JsonPropertyName("pretty_path")]
        public string Path {get;set;}

        [JsonPropertyName("url")]
        public string Url {get;set;}
    }

    public class AppveyorResponse {
        [JsonPropertyName("builds")]
        public List<AppveyorBuild> Builds {get;set;}
    }

    public class AppveyorBuild {
        [JsonPropertyName("status")]
        public string Status {get;set;}

        [JsonPropertyName("buildNumber")]
        public int BuildNumber {get;set;}

        [JsonPropertyName("pullRequestId")]
        public string PullRequestId {get;set;}

        [JsonPropertyName("buildId")]
        public int BuildId {get;set;}
    }
}
