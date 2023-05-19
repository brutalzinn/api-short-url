using System;

namespace ApiShortUrl.Models.Exceptions
{
    [Serializable]
    public class CustomException : Exception
    {
        public int StatusCode { get; }
        public TypeException Type { get; }

        public CustomException(TypeException typeException, string message)
       : base(message)
        {
            Type = typeException;
            StatusCode = (int)typeException;
        }

        public CustomException(TypeException typeException)
        {
            Type = typeException;
            StatusCode = (int)typeException;
        }
        public CustomExceptionResponse GetResponse()
        {
            var response = new CustomExceptionResponse()
            {
                Type = GetType(),
                Message = base.Message
            };

            return response;
        }

        private string GetType()
        {
            switch (Type)
            {
                case TypeException.BUSINESS_LOGIC:
                    return "business_logic";
                case TypeException.AUTHORIZATION:
                    return "not_authorize";
                case TypeException.VALIDATION:
                    return "validation";

            }
            return "not_recognized";
        }


    }
    public class CustomExceptionResponse
    {
        public string Type { get; set; }
        public string Message { get; set; }
    }
}
