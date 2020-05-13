using Microsoft.AspNetCore.Http;

namespace CoreAppSample.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string Message)
        {
            response.Headers.Add("Application-Error", Message);
            response.Headers.Add("Access-Control-Expose-Headers-Error","Application-Error" );
            response.Headers.Add("Access-Control-Allow-Origin","*" );
        }
    }
}