using SrcdsFirewallManager.Converters;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
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
        /// Field for <see cref="JsonOptions"/>.
        /// </summary>
        private static JsonSerializerOptions? _jsonOptions = null;

        /// <summary>
        /// Gets the <see cref="JsonSerializerOptions"/> for (de)serialization of entities.
        /// </summary>
        private static JsonSerializerOptions JsonOptions
        {
            get
            {
                if (_jsonOptions is null)
                {
                    _jsonOptions = new()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    _jsonOptions.Converters.Add(new IpAddressJsonConverter());
                    _jsonOptions.Converters.Add(new PortRangeJsonConverter());
                }
                return _jsonOptions;
            }
        }

        /// <summary>
        /// Gets an entity.
        /// </summary>
        /// <typeparam name="TDeserialized">The entity to deserialize.</typeparam>
        /// <param name="httpClient">The <see cref="HttpClient"/> to use for the request.</param>
        /// <param name="url">The URL to get the data from.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> to be passed to the <see cref="HttpClient"/>.</param>
        /// <returns>An instance of <typeparamref name="TDeserialized"/> or <see langword="null"/> if failed.</returns>
        public static Task<TDeserialized?> GetObjectAsync<TDeserialized>(this HttpClient httpClient, string url, CancellationToken cancellationToken = default)
        {
            return httpClient.GetFromJsonAsync<TDeserialized>(url, JsonOptions, cancellationToken);
        }

    }
}
