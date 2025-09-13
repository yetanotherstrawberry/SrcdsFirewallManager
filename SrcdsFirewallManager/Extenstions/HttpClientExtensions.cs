using SrcdsFirewallManager.Generators;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SrcdsFirewallManager.Extenstions
{
    internal static class HttpClientExtensions
    {

        public static Task<TDeserialized?> GetObjectAsync<TDeserialized>(this HttpClient httpClient, string url, CancellationToken cancellationToken = default)
        {
            return httpClient.GetFromJsonAsync(url, typeof(TDeserialized), SourceGenerationContext.Default, cancellationToken).ContinueWith(result => (TDeserialized?)result.Result);
        }

    }
}
