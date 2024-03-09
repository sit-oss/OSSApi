using System.Diagnostics.Metrics;
using System.Text.Json;
using System.Text.Json.Nodes;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MySqlConnector;
using OSSApi.Models;

namespace OSSApi.Controllers
{
    /// <summary>
    /// Danmaku controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DanmakuController : ControllerBase
    {
        /// <summary>
        /// Get the player setting
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("settings")]
        public async Task<PlayerSetting?> GetPlayerSetting()
        {
            await using var _db = new MySqlConnection(Global.ConnectionString);
            var query = await _db.QueryFirstOrDefaultAsync<PlayerSetting>(
                "SELECT `content` FROM `configs` WHERE `name` = 'PlayerSetting';");
            return query;
        }

        /// <summary>
        /// Get video info
        /// </summary>
        /// <param name="vid"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("meta/{vid:required}")]
        public async Task<VideoMeta?> GetPlayer([FromRoute] string vid)
        {
            await using var _db = new MySqlConnection(Global.ConnectionString);
            var item = await _db.QueryFirstOrDefaultAsync<VideoMeta>(
                "SELECT * FROM `videos` WHERE `name` = @vid;",new{vid});
            if (item == null)
                return new VideoMeta{pid = ""};
            item.pid = item.id.ToString().md5();

            var ip = Request.GetIpAddress();
            var exp = 5;
            var key = $"video:{item.id}";
            var count = Global.MemoryCache.GetOrCreate(key, _ => 0);
            var keywithip = $"{key}:{ip}";
            if (!Global.MemoryCache.Get<bool>(keywithip))
            {
                count++;
                Global.MemoryCache.Set(keywithip, true, TimeSpan.FromMinutes(exp));
                Global.MemoryCache.Set(key, count, TimeSpan.FromMinutes(exp));
            }
            item.ucount = count;
            return item;
        }

        /// <summary>
        /// Get danmaku list
        /// </summary>
        /// <param name="id">video hash id</param>
        /// <param name="max">max len</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id:length(32)}")]
        public async Task<DanmakuList?> GetDanmaku([FromRoute] string id,[FromQuery] int? max = null)
        {
            await using var _db = new MySqlConnection(Global.ConnectionString);
            var sql = "SELECT * FROM `danmaku` WHERE `deleted`=0 AND `id` = @id;";
            if (max is > 0 and <= 100)
                sql = $"SELECT * FROM `danmaku` WHERE `id` = @id ORDER BY `videotime` DESC LIMIT {max};";
            var query = (await _db.QueryAsync<DanmakuItem>(sql,new{id})).AsList();
            // var count = _db.QueryFirstOrDefault<int>("SELECT COUNT(*) FROM `danmaku` WHERE `id` = @id;",new{id});
            var count = query.Count;
            var jsonArray = JsonSerializer.Deserialize<JsonArray>("[]")!;
            foreach (var danmakuItem in query)
            {
                jsonArray.Add(JsonSerializer.Deserialize<JsonArray>(danmakuItem.ToString())!);
            }
            return new DanmakuList { count = count, danmaku = jsonArray };
        }

        /// <summary>
        /// Insert a danmaku
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            var item = JsonSerializer.Deserialize<DanmakuRequest>(body);
            if (item == null)
                return BadRequest();
            var uid = User.Identity.GetId();
            if (uid == 0)
                return Unauthorized();
            var sqlItem = new DanmakuItem
            {
                id = item.id,
                type = item.type,
                text = item.text,
                color = item.color,
                size = item.size,
                videotime = item.time,
                uid = uid,
                ip = Request.GetIpAddress(),
                time = DateTimeOffset.Now.ToUnixTimeSeconds()
            };
            await using var _db = new MySqlConnection(Global.ConnectionString);
            var sql = "INSERT IGNORE INTO `danmaku` (`id`, `type`, `text`, `color`, `size`, `videotime`, `uid`, `ip`, `time`) VALUES (@id, @type, @text, @color, @size, @videotime, @uid, @ip, @time);";
            var result = await _db.ExecuteAsync(sql, sqlItem);
            return result > 0 ? new JsonResult(new BaseResponse()) : BadRequest();
        }
    }
}
