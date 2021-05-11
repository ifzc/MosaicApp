namespace MosaicApp.ImageProcessing
{
    /// <summary>
    /// 马赛克处理 参数
    /// </summary>
    public class MosaicProcessParam
    {
        /// <summary>
        /// Base64图片字符串
        /// </summary>
        public string Base64 { get; set; }

        /// <summary>
        /// 开始点（x,y）
        /// </summary>
        public string StartPoint { get; set; }

        /// <summary>
        /// 结束点（x,y）
        /// </summary>
        public string EndPoint { get; set; }
    }
}
