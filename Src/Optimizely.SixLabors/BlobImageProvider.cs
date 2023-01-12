using System;
using System.Threading.Tasks;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using SixLabors.ImageSharp.Web;
using SixLabors.ImageSharp.Web.Providers;
using SixLabors.ImageSharp.Web.Resolvers;

namespace Optimizely.SixLabors
{
    /// <summary>
    /// Blob image provider invoke by SixLabors image processor
    /// </summary>
    public class BlobImageProvider : IImageProvider
    {
        /// <summary>
        /// Contains various format helper methods based on the current configuration.
        /// </summary>
        private readonly FormatUtilities _formatUtilities;

        private readonly IUrlResolver _urlResolver;

        /// <summary>
        /// A match function used by the resolver to identify itself as the correct resolver to use.
        /// </summary>
        private Func<HttpContext, bool> _match;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobImageProvider"/> class.
        /// </summary>
        /// <param name="formatUtilities">Contains various format helper methods based on the current configuration.</param>
        /// <param name="urlResolver"></param>
        public BlobImageProvider(FormatUtilities formatUtilities, IUrlResolver urlResolver)
        {
            _formatUtilities = formatUtilities;
            _urlResolver = urlResolver;
        }

        /// <inheritdoc/>
        public ProcessingBehavior ProcessingBehavior => ProcessingBehavior.CommandOnly;

        /// <inheritdoc/>
        public Func<HttpContext, bool> Match
        {
            get => _match ?? IsMatch;
            set => _match = value;
        }

        /// <inheritdoc/>
        public bool IsValidRequest(HttpContext context)
        {
            return _formatUtilities.TryGetExtensionFromUri(context.Request.GetDisplayUrl(), out _);
        }

        /// <inheritdoc/>
        public Task<IImageResolver> GetAsync(HttpContext context)
        {
            var url = context.Request.Path.Value;

            if (_urlResolver.Route(new UrlBuilder(url)) is MediaData media && media.BinaryData != null)
            {
                return Task.FromResult<IImageResolver>(new BlobImageResolver(media.BinaryData));
            }

            return Task.FromResult<IImageResolver>(null);
        }

        private bool IsMatch(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/contentassets", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (context.Request.Path.StartsWithSegments("/globalassets", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (context.Request.Path.StartsWithSegments("/siteassets", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}

