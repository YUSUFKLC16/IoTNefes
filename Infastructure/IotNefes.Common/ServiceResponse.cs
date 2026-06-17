namespace IotNefes.Common
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool IsSuccessful { get; set; }
        public ServiceError? Error { get; set; }

        public static ServiceResponse<T> Success(T data) => new()
        {
            Data = data,
            IsSuccessful = true
        };

        public static ServiceResponse<T> Fail(string errorMessage, string? errorCode = null) => new()
        {
            IsSuccessful = false,
            Error = new ServiceError(errorMessage, errorCode)
        };
    }

    public class ServiceResponse
    {
        public bool IsSuccessful { get; set; }
        public ServiceError? Error { get; set; }

        public static ServiceResponse Success() => new() { IsSuccessful = true };

        public static ServiceResponse Fail(string errorMessage, string? errorCode = null) => new()
        {
            IsSuccessful = false,
            Error = new ServiceError(errorMessage, errorCode)
        };
    }

    public class ServiceError
    {
        public string Message { get; set; }
        public string? Code { get; set; }

        public ServiceError(string message, string? code = null)
        {
            Message = message;
            Code = code;
        }
    }
}
