using Flurl;
using System;
using System.Linq;
using GPMDP.Release.Data;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;

namespace GPMDP.Release.Services
{
    public class CircleCIService
    {
        public async Task<(int buildNumber, Uri buildUrl)> GetLatestValidBuild()
        {
            var url = "https://circleci.com/api/v1.1/project/github/MarshallOfSound/Google-Play-Music-Desktop-Player-UNOFFICIAL-/"
                .SetQueryParam("limit", 100)
                .ToString();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var json = await client.GetStringAsync(url);
                var buildRecords = JsonSerializer.Deserialize<List<CircleCIBuild>>(json.Trim('(', ')'));
                var firstSuccess = buildRecords.FirstOrDefault(b =>
                {
                    return !b.Failed && b.HasArtifacts && string.Equals(b.Branch, "master", StringComparison.CurrentCultureIgnoreCase);
                });
                if (firstSuccess == null) throw new Exception("Valid build not found!");
                return (firstSuccess.Number, new Uri($"https://circleci.com/gh/MarshallOfSound/Google-Play-Music-Desktop-Player-UNOFFICIAL-/{firstSuccess.Number}"));
            }
        }

        public async Task<(string fileName, Uri artifactUri)> GetArtifactAsync(int buildNumber, BuildRequest request)
        {
            var url = $"https://circleci.com/api/v1.1/project/github/MarshallOfSound/Google-Play-Music-Desktop-Player-UNOFFICIAL-/{buildNumber}/artifacts";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var json = await client.GetStringAsync(url);
                var artifacts = JsonSerializer.Deserialize<List<CircleCIArtifact>>(json.Trim('(', ')'));
                var firstMatch = artifacts.FirstOrDefault(a => a.Path.Contains(request.ToMatchString()));
                if (firstMatch == null) throw new Exception("Valid artifact not found!");
                return (System.IO.Path.GetFileName(firstMatch.Path), new Uri(firstMatch.Url));
            }
        }
    }
}