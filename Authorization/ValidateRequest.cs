﻿using System.Net;
using System.Security.Principal;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace API.Response.Filter.Authorization
{
    public class ValidateRequest : IAuthenticationModule
    {
        private const string Realm = "My Realm";
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //If the Authorization header is empty or null
            //then return Unauthorized
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);
                // If the request was unauthorized, add the WWW-Authenticate header 
                // to the response which indicates that it require basic authentication
                if (actionContext.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    actionContext.Response.Headers.Add("WWW-Authenticate",
                        string.Format("Basic realm=\"{0}\"", Realm));
                }
            }
            else
            {
                //Get the authentication token from the request header
                string? authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                //Decode the string
                string? decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                //Convert the string into an string array
                string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
                //First element of the array is the username
                string username = usernamePasswordArray[0];
                //Second element of the array is the password
                string password = usernamePasswordArray[1];

                //call the login method to check the username and password
                if (1 == 1)
                {
                    var identity = new GenericIdentity(username);
                    IPrincipal principal = new GenericPrincipal(identity, null);
                    Thread.CurrentPrincipal = principal;

                    actionContext.Response = actionContext.Request
                      .CreateResponse(HttpStatusCode.OK);

                }
                else
                {
                    actionContext.Response = actionContext.Request
                        .CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}
