using System.Net.Http.Headers;
using System.Text;

namespace API.Response.Filter.Authorization
{
    public class RequestValidator
    {
        private readonly IConfiguration _configuration;

        public RequestValidator(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public bool IsValidToken(HttpContext context)
        {
            bool isValid = false;

            try
            {
                var _secrets_API_key = _configuration.GetValue<string>("Secrets:API_key");

                var _secrets_API_key_secret = _configuration.GetValue<string>("Secrets:API_key_secret");


                var authenticationSchema = context.Request.Headers["Authorization"];
                if (!String.IsNullOrWhiteSpace(authenticationSchema))
                {
                    var authHeader = AuthenticationHeaderValue.Parse(authenticationSchema);
                    if (authHeader != null)
                    {
                        var keyBytes = Convert.FromBase64String(authHeader.Parameter);
                        var credentials = Encoding.UTF8.GetString(keyBytes).Split(':', 2);
                        var API_key = credentials[0];
                        var API_key_secret = credentials[1];

                        if (API_key == _secrets_API_key && API_key_secret == _secrets_API_key_secret)
                            isValid = true;
                    }

                }
                
                
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return isValid;

        }
    }
}
