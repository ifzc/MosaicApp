using System.Threading.Tasks;
using Autofac;
using MosaicApp.ImageProcessing;

namespace MosaicApp
{
    internal class Program
    {
        private static async Task Main()
        {
            await CompositionRoot().Resolve<Application>().RunAsync();
        }

        private static IContainer CompositionRoot()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Application>();
            builder.RegisterType<ImageProcessingService>().As<IImageProcessingService>();
            return builder.Build();
        }
    }
}