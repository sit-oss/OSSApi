namespace OSSApi.Models
{
    /// <summary>
    /// 播放器设置
    /// </summary>
    public class PlayerSetting
    {
        /// <summary>
        /// 空对象
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public static PlayerSetting Empty = new()
        {
            danmuon = "",
            color = "",
            logo = "",
            trytime = "",
            waittime = "",
            sendtime = "",
            dmrule = "",
            pbgjz = "",
            ads = new object()
        };

        /// <summary>
        /// 是否开启弹幕
        /// </summary>
        public required string danmuon { get; set; }
        /// <summary>
        /// 弹幕颜色
        /// </summary>
        public required string color { get; set; }
        /// <summary>
        /// logo
        /// </summary>
        public required string logo { get; set; }
        /// <summary>
        /// 重试间隔
        /// </summary>
        public required string trytime { get; set; }
        /// <summary>
        /// 等待间隔
        /// </summary>
        public required string waittime { get; set; }
        /// <summary>
        /// 发送间隔
        /// </summary>
        public required string sendtime { get; set; }
        /// <summary>
        /// 规则html路径
        /// </summary>
        public required string dmrule { get; set; }
        /// <summary>
        /// 屏蔽关键词
        /// </summary>
        public required string pbgjz { get; set; }
        /// <summary>
        /// 广告配置
        /// </summary>
        public required object ads { get; set; }
    }
}
