using Flurl;
using Flurl.Http;
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
        public async Task<(Uri buildUrl, Uri artifactUrl)> GetLatestArtifact(BuildRequest request) {
            var url = "https://circleci.com/api/v1.1/project/github/MarshallOfSound/Google-Play-Music-Desktop-Player-UNOFFICIAL-/"
                .SetQueryParam("limit", 100)
                .ToString();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var json = await client.GetStringAsync(url);
                // json = $"[{json.Trim('(', ')')}]";
                json = json.Trim('(', ')');
            var buildRecords = JsonSerializer.Deserialize<List<CircleCIBuild>>(json);
            var firstSuccess = buildRecords.FirstOrDefault(b => {
                return !b.Failed && b.HasArtifacts;
            });
            return (new Uri($"https://circleci.com/gh/MarshallOfSound/Google-Play-Music-Desktop-Player-UNOFFICIAL-/{firstSuccess.Number}#artifacts"), null);
            }
        }
    }

    public class AppveyorService {
        public (Uri buildUrl, Uri artifactUrl) GetLatestArtifact(BuildRequest request) {

            return (null, null);
        }
    }
}