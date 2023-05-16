using System.ComponentModel;

namespace ApiShortUrl.Models
{

    public enum TypeException
    {
        VALIDATION = 400,
        BUSINESS_LOGIC = 406,
        NOTFOUND = 404,
        AUTHORIZATION = 401,
        INTERN = 500
    }
}
