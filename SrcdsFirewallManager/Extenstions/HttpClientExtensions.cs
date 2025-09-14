using SrcdsFirewallManager.Generators;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SrcdsFirewallManager.Extenstions
{
    /// <summary>
    /// Provides extensions for the <see cref="HttpClient"/>.
    /// </summary>
    internal static class HttpClientExtensions
    {

        /// <summary>
        /// Gets an entity.
        /// </summary>
        /// <typeparam name="TDeserialized">The entity to deserialize.</typeparam>
        /// <param name="httpClient">The <see cref="HttpClient"/> to use the <see cref="HttpClientJsonExtensions.GetFromJsonAsync(HttpClient, System.Uri?, System.Type, System.Text.Json.Serialization.JsonSerializerContext, CancellationToken)"/> from.</param>
        /// <param name="url">The URL to get the data from.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> to be passed to the <see cref="HttpClient"/>.</param>
        /// <returns>An instance of <typeparamref name="TDeserialized"/> or <see langword="null"/> if failed.</returns>
        public static Task<TDeserialized?> GetObjectAsync<TDeserialized>(this HttpClient httpClient, string url, CancellationToken cancellationToken = default)
        {
            return httpClient.GetFromJsonAsync(url, typeof(TDeserialized), SourceGenerationContext.Default, cancellationToken).ContinueWith(result => (TDeserialized?)result.Result);
        }

    }
}
