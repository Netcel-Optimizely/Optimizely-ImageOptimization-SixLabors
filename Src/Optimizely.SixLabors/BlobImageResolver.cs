using System.IO;
using System.Threading.Tasks;
using EPiServer.Framework.Blobs;
using SixLabors.ImageSharp.Web;
using SixLabors.ImageSharp.Web.Resolvers;

namespace Optimizely.SixLabors
{
    /// <summary>
    /// Blob image resolver invoke by SixLabors image processor middleware
    /// </summary>
    public class BlobImageResolver : IImageResolver
    {
        private readonly Blob _blob;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobImageResolver"/> class.
        /// Blob Image Resolver
        /// </summary>
        /// <param name="blob"></param>
        public BlobImageResolver(Blob blob)
        {
            _blob = blob;
        }

        /// <summary>
        /// Gets image meta data
        /// </summary>
        /// <returns></returns>
        public async Task<ImageMetadata> GetMetaDataAsync()
        {
            var fileInfo = await _blob.AsFileInfoAsync();
            return new ImageMetadata(fileInfo.LastModified.UtcDateTime, fileInfo.Length);
        }

        /// <summary>
        /// Read open blob
        /// </summary>
        /// <returns></returns>
        public Task<Stream> OpenReadAsync()
        {
            return Task.FromResult(_blob.OpenRead());
        }
    }
}
