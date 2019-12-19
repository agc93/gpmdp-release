using Flurl;
using System;
using System.Linq;
using GPMDP.Release.Data;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace GPMDP.Release.Services
{
    public class AppVeyorService
    {
        public async Task<(string buildNumber, int buildId, Uri buildUrl)> GetLatestValidBuild()
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
                var firstSuccess = buildRecords.Builds.FirstOrDefault(b =>
                {
                    return string.IsNullOrWhiteSpace(b.PullRequestId) && b.Status == "success";
                });
                if (firstSuccess == null) throw new Exception("Valid build not found!");
                return (firstSuccess.BuildNumber.ToString(), firstSuccess.BuildId, new Uri($"https://ci.appveyor.com/project/MarshallOfSound/google-play-music-desktop-player-unofficial/builds/{firstSuccess.BuildId}"));
            }
        }
    }
}