using Microsoft.Extensions.DependencyInjection;
using SixLabors.ImageSharp.Web.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Web.DependencyInjection;

namespace Optimizely.SixLabors
{
    /// <summary>
    /// Adds service collection extensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure SixLabors image processor
        /// </summary>
        /// <param name="services"></param>
        public static void AddImageOptimizer(this IServiceCollection services)
        {
            services.AddImageSharp()
                .ClearProviders()
                .AddProvider<BlobImageProvider>()
                .AddProvider<PhysicalFileSystemProvider>();
        }
    }
}
