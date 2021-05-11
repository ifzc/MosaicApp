namespace MosaicApp.ImageProcessing
{
    /// <summary>
    /// 水印处理 参数
    /// </summary>
    public class MarkProcessParam
    {
        /// <summary>
        /// Base64图片字符串
        /// </summary>
        public string Base64 { get; set; }

        /// <summary>
        /// 水印内容
        /// </summary>
        public string Mark { get; set; }
    }
}
