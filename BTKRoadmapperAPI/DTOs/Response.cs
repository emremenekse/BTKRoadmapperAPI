using System.Text.Json.Serialization;

namespace BTKRoadmapperAPI.DTOs
{
    public class Response<T>
    {
        public T Data { get; private set; }
        [JsonIgnore]
        public int StatusCode { get; private set; }
        [JsonIgnore]
        public bool IsSuccessful { get; private set; }
        public string ErrorMessage { get; private set; }
        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { Data = default, StatusCode = statusCode, IsSuccessful = true };
        }
        public static Response<T> Fail(string errorMessage, int statusCode)
        {
            return new Response<T> { ErrorMessage = errorMessage, StatusCode = statusCode, IsSuccessful = false };
        }
    }
}
