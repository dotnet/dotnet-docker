using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Docker.UpdateDependencies;

internal static class ChecksumHelper
{
    public static async Task<string?> ComputeChecksumShaAsync(
        HttpClient httpClient,
        string downloadUrl,
        CancellationToken cancellationToken = default)
    {
        string? sha = null;

        using (HttpResponseMessage response = await httpClient.GetAsync(downloadUrl, cancellationToken))
        {
            if (response.IsSuccessStatusCode)
            {
                using (Stream httpStream = await response.Content.ReadAsStreamAsync(cancellationToken))
                using (SHA512 hash = SHA512.Create())
                {
                    byte[] hashedInputBytes = hash.ComputeHash(httpStream);

                    StringBuilder stringBuilder = new(128);
                    foreach (byte b in hashedInputBytes)
                    {
                        stringBuilder.Append(b.ToString("X2"));
                    }
                    sha = stringBuilder.ToString();
                }
            }
            else
            {
                Trace.TraceInformation($"Failed to download {downloadUrl}.");
            }
        }

        return sha?.ToLowerInvariant();
    }
}
