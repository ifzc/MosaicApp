using System.Threading.Tasks;

namespace MosaicApp.ImageProcessing
{
    public interface IImageProcessingService
    {
        /// <summary>
        /// 马赛克处理
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<string> MosaicProcessAsync(MosaicProcessParam param);

        /// <summary>
        /// 水印处理
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<string> MarkProcessAsync(MarkProcessParam param);
    }
}