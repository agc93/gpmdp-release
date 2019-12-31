using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GPMDP.Release.Data;

namespace GPMDP.Release.Services
{
    public interface CIService
    {
         Task<IEnumerable<(int buildNumber, Uri buildUrl)>> GetValidBuilds();
         Task<(int buildNumber, Uri buildUrl)> GetLatestValidBuild();
         Task<(string fileName, Uri artifactUri)> GetArtifactAsync(int buildNumber, BuildRequest request);
    }
}