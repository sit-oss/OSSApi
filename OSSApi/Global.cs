using System.Data.Common;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace OSSApi
{
    internal static class Global
    {
        public static MemoryCache MemoryCache { get; } = new(new MemoryCacheOptions());
        public static string ConnectionString { get; set; } = "";

        public static byte[] ToBytes(this string str, Encoding? encoding = null)
        {
            return (encoding ?? Encoding.UTF8).GetBytes(str);
        }
        public static byte[] MD5(this byte[] bt)
        {
            using var sha = System.Security.Cryptography.MD5.Create();
            return sha.ComputeHash(bt);
        }
        public static string MD5(this string str, Encoding? encoding = null)
        {
            return Convert.ToHexString(str.ToBytes(encoding).MD5());
        }
        public static string md5(this string str, Encoding? encoding = null)
        {
            return str.MD5(encoding).ToLower();
        }
        public static string GetIpAddress(this HttpRequest request)
        {
            if (request.Headers.TryGetValue("CF-CONNECTING-IP", out var address))
                return address!;
            if (request.Headers.TryGetValue("HTTP_X_FORWARDED_FOR", out var ipAddress) && !string.IsNullOrEmpty(ipAddress))
            {
                var addresses = ipAddress.ToString().Split(',');
                if (addresses.Length != 0)
                    return addresses.Last();
            }
            if (request.Headers.TryGetValue("X-Real-IP", out var xrealip))
                return xrealip!;
            return request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "8.8.8.8";
        }
        public static int GetId(this IIdentity? identity)
        {
            if (identity == null) return 0;
            var id = identity as ClaimsIdentity;
            if (id == null) return 0;
            var claim = id.FindFirst("sub") ?? id.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if(claim == null) return 0;
            return int.TryParse(claim.Value, out var i) ? i : 0;
        }
    }
}
