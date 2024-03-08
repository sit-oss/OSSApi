namespace OSSApi.Models;

/// <summary>
/// Response base class
/// </summary>
/// <typeparam name="T">data</typeparam>
public class BaseResponse<T> where T : class
{
    /// <summary>
    /// response code
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("code")]
    public int Code { get; set; }
    /// <summary>
    /// data
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("data")]
    public required T Data { get; set; }
}

/// <summary>
/// Just ok
/// </summary>
public class BaseResponse
{
    /// <summary>
    /// I don't know why need this, just gives 233
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("code")]
    public int Code { get; set; } = 233;
    /// <summary>
    /// I don't know why need this too
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("danmuku")]
    public bool Danmuku { get; set; } = true;
}