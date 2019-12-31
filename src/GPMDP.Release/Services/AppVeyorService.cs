using Flurl;
using System;
using System.Linq;
using GPMDP.Release.Data;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;

namespace GPMDP.Release.Services
{
    public class AppVeyorService
    {
        public Task<(string fileName, Uri artifactUri)> GetArtifactAsync(int buildNumber, BuildRequest request)
        {
            throw new NotImplementedException("AppVeyor does not support anonymously fetching artifacts!");
        }

        public async Task<(string buildNumber, int buildId, Uri buildUrl)> GetLatestValidBuild()
        {

            return (await GetValidBuilds()).FirstOrDefault();
        }

        public async Task<IEnumerable<(string buildNumber, int buildId, Uri buildUrl)>> GetValidBuilds()
        {
             var url = "https://ci.appveyor.com/api/projects/MarshallOfSound/google-play-music-desktop-player-unofficial/history"
                .SetQueryParam("recordsNumber", 100)
                .SetQueryParam("branch", "master")
                .ToString();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var json = await client.GetStringAsync(url);
                var buildRecords = JsonSerializer.Deserialize<AppveyorResponse>(json);
                var successful = buildRecords.Builds.Where(b =>
                {
                    return string.IsNullOrWhiteSpace(b.PullRequestId) && b.Status == "success";
                });
                if (successful == null || !successful.Any()) throw new Exception("Valid build not found!");
                return successful.Select(s => (s.BuildNumber.ToString(), s.BuildId, new Uri($"https://ci.appveyor.com/project/MarshallOfSound/google-play-music-desktop-player-unofficial/builds/{s.BuildId}")));
            }
        }
    }
}