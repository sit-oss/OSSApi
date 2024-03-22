namespace OSSApi.Models
{
    /// <summary>
    /// 弹幕列表
    /// </summary>
    public class DanmakuList
    {
        /// <summary>
        /// 弹幕数量
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("danum")]
        public int count { get; set; }

        /// <summary>
        /// I don't know why need this, just gives 233
        /// </summary>
        public int code { get; set; } = 233;

        /// <summary>
        /// 弹幕列表
        /// </summary>
        public required System.Text.Json.Nodes.JsonArray danmaku { get; set; }
        //public required IEnumerable<DanmakuItem> danmaku { get; set; }
    }

    /// <summary>
    /// danmaku item
    /// </summary>
    public class DanmakuItem
    {
        /// <summary>
        /// 发送者名
        /// </summary>
        public string? author { get; set; }

        /// <summary>
        /// 弹幕池id
        /// </summary>
        public required string id { get; set; }
        /// <summary>
        /// 弹幕id
        /// </summary>
        public int cid { get; set; }
        /// <summary>
        /// 弹幕类型
        /// </summary>
        public required string type { get; set; }
        /// <summary>
        /// 弹幕内容
        /// </summary>
        public required string text { get; set; }
        /// <summary>
        /// 弹幕颜色
        /// </summary>
        public required string color { get; set; }
        /// <summary>
        /// 弹幕大小
        /// </summary>
        public required string size { get; set; }
        /// <summary>
        /// 时间点
        /// </summary>
        public float videotime { get; set; }
        /// <summary>
        /// 发送者id
        /// </summary>
        public int uid { get; set; }
        /// <summary>
        /// 发送者ip
        /// </summary>
        public required string ip { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public long time { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            /*
               // 请不要随意调换下列数组赋值顺序
               $arr[$k][0] = (float) $v['videotime'];  //弹幕出现时间(s)
               $arr[$k][1] = (string) $v['type'];   //弹幕样式  
               $arr[$k][2] = (string) $v['color']; //字体的颜色
               $arr[$k][3] = (string) $v['cid'];  //现在是弹幕id，以后可能是发送者id了
               $arr[$k][4] = (string) $v['text'];  //弹幕文本
               $arr[$k][5] = (string) $v['ip'];  //弹幕ip
               $arr[$k][6] = $date = date('m-d H:i', $v['time']);  //弹幕系统时间
               $arr[$k][7] = (string) $v['size'];  //弹幕系统大小
             */
            return $"[{videotime},\"{type}\",\"{color}\",\"{cid}\",\"{text}\",\"{ip}\",{time},\"{size}\",{uid}]";
        }
    }


    internal class DanmakuRequest
    {
        public required string id { get; set; }
        public required string author { get; set; }
        public required float time { get; set; }
        public required string text { get; set; }
        public required string color { get; set; }
        public required string type { get; set; }
        public required string size { get; set; }
    }

}
