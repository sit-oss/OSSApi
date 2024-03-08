namespace OSSApi.Models
{
    /// <summary>
    /// Video meta
    /// </summary>
    public class VideoMeta
    {
        /// <summary>
        /// video id
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public int id { get; set; }
        /// <summary>
        /// video name
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public string name { get; set; } = "";
        /// <summary>
        /// video link
        /// </summary>
        public string url { get; set; } = "";
        /// <summary>
        /// video title
        /// </summary>
        public string? title { get; set; }
        /// <summary>
        /// video cover
        /// </summary>
        public string? cover { get; set; }

        /// <summary>
        /// danmaku pool id
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("id")]
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string pid { get; set; } = "";

        /// <summary>
        /// danmaku pool id
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("usernum")]
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public int ucount { get; set; } = 0;
    }
}
