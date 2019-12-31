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
    public class CircleCIService : CIService
    {
        public async Task<(int buildNumber, Uri buildUrl)> GetLatestValidBuild()
        {
            return (await GetValidBuilds()).FirstOrDefault();
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

        public async Task<IEnumerable<(int buildNumber, Uri buildUrl)>> GetValidBuilds()
        {
            var url = "https://circleci.com/api/v1.1/project/github/MarshallOfSound/Google-Play-Music-Desktop-Player-UNOFFICIAL-/"
                .SetQueryParam("limit", 100)
                .ToString();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var json = await client.GetStringAsync(url);
                var buildRecords = JsonSerializer.Deserialize<List<CircleCIBuild>>(json.Trim('(', ')'));
                var successful = buildRecords.Where(b => {
                    return b != null 
                    && !b.Failed 
                    && b.HasArtifacts 
                    && b.JobName.ToLower() == "artifact_gather" // this is specific to GPMDP's CI builds
                    && string.Equals(b.Branch, "master", StringComparison.CurrentCultureIgnoreCase);
                });
                if (successful == null || !successful.Any()) throw new Exception("Valid build not found!");
                return successful.Select(s => (s.Number, new Uri($"https://circleci.com/gh/MarshallOfSound/Google-Play-Music-Desktop-Player-UNOFFICIAL-/{s.Number}")));
            }
        }
    }
}