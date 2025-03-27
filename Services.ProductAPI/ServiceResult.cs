using System.Net;
using System.Text.Json.Serialization;

namespace Services.ProductAPI;

public class ServiceResult<T>
{
    public T? Data { get; set; }
    public List<string>? ErrorMessage { get; set; }
    public bool IsSuccess => ErrorMessage == null || ErrorMessage.Count == 0; // get metodu olan property
    [JsonIgnore] public bool IsFail => !IsSuccess;
    [JsonIgnore] public HttpStatusCode Status { get; set; }
    [JsonIgnore] public string? UrlAsCreated { get; set; }
    //static factory method
    public static ServiceResult<T> Success(T data ,HttpStatusCode status = HttpStatusCode.OK )
    {
        return new ServiceResult<T>
        { 
            Data = data,
            Status = status,
        };
    }
    public static ServiceResult<T> SuccessCreated(T data,string urlCreated )
    {
        return new ServiceResult<T>
        {
            UrlAsCreated = urlCreated,
            Data = data,
            Status = HttpStatusCode.Created,
        };
    }
    public static ServiceResult<T> Fail(List<string>  errorMessage , HttpStatusCode status = HttpStatusCode.BadRequest)
    {
        return new ServiceResult<T>
        {
            ErrorMessage = errorMessage,
            Status = status,
        };
    }
    public static ServiceResult<T> Fail(string errorMessage, HttpStatusCode status = HttpStatusCode.BadRequest)
    {
        return new ServiceResult<T>
        {
            ErrorMessage = new List<string>() {errorMessage}, //  c# 8 [errorMessage]
            Status = status,
        };
    } 
}

    //no content 
    public class ServiceResult
    {
       

        public List<string>? ErrorMessage { get; set; }

        [JsonIgnore]
        public bool IsSuccess => ErrorMessage == null || ErrorMessage.Count == 0; // get metodu olan property
        [JsonIgnore]

        public bool IsFail => !IsSuccess;
        [JsonIgnore]

        public HttpStatusCode Status { get; set; }

        public static ServiceResult Success(HttpStatusCode status = HttpStatusCode.OK)
        {
            return new ServiceResult
            {
             
                Status = status,
            };
        }

        public static ServiceResult Fail(List<string> errorMessage, HttpStatusCode status = HttpStatusCode.BadRequest)
        {
            return new ServiceResult
            {
                ErrorMessage = errorMessage,
                Status = status,
            };
        }

        public static ServiceResult Fail(string errorMessage, HttpStatusCode status = HttpStatusCode.BadRequest)
        {
            return new ServiceResult
            {
                ErrorMessage = new List<string>() { errorMessage }, //  c# 8 [errorMessage]
                Status = status,
            };
        }





    }