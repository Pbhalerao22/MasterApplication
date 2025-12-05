using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MasterApplication.Services
{
    public class PortalMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PortalMiddleware> _logger;

        public PortalMiddleware(RequestDelegate next, ILogger<PortalMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            string strMaliciousURL = "/v2/Denied/CustomError/InvalidInput";
            string strInvalidMethodURL = "/v2/Denied/CustomError/InvalidMethod";

            bool isAjaxRequest = context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            bool isInvalidMethod = IsRequestMethodNotAllowed(context);
            //bool isMaliciousInput = isMaliciousInputNotAllowed(context, isAjaxRequest);
            // Otherwise, pass the request to the next middleware in the pipeline

            if (isInvalidMethod == true)
            {
                await PortalMiddleware.HandleRedirect(context, strInvalidMethodURL, isAjaxRequest);

            }
            /*else if (isMaliciousInput == true)
            {
                await PortalMiddleware.HandleRedirect(context, strMaliciousURL, isAjaxRequest);
            }*/


            await _next(context);
        }
        public bool IsRequestMethodNotAllowed(HttpContext context)
        {
            //string strPageURL = "/v2/Denied/CustomError/Index";
            // Check if the method is GET or POST
            if (context.Request.Method != "GET" && context.Request.Method != "POST")
            {
                // Return a 405 Method Not Allowed if the request method is neither GET nor POST
                context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                //context.Response.Redirect(strPageURL);
                //await context.Response.WriteAsync("Method Not Allowed");

                return true;
            }
            return false;
        }
        public bool isMaliciousInputNotAllowed(HttpContext context, bool isAjaxRequest)
        {
            //string strPageURL = "/v2/Denied/CustomError/Index";
            bool isMalicious = false;
            if (context.Request.HasFormContentType)
            {
                foreach (var key in context.Request.Form.Keys)
                {
                    string value = context.Request.Form[key];

                    if (IsMalicious(value))
                    {
                        _logger.LogWarning($"Blocked malicious form input: {value}");
                        //context.Response.Redirect(strPageURL);
                        isMalicious = true;
                    }
                    /*else if (IsMalicious(value) && isAjaxRequest == true)
                    {
                        _logger.LogWarning($"Blocked malicious form input: {value}");
                        context.Response.StatusCode = 200; // OK status code
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            redirectTo = strPageURL  // URL to redirect the user to
                        }));
                        return;
                    }*/
                }
            }
            foreach (var key in context.Request.Query.Keys)
            {
                string value = context.Request.Query[key];

                if (IsMalicious(value))
                {
                    _logger.LogWarning($"Blocked malicious query input: {value}");
                    isMalicious = true;
                }
            }
            return isMalicious;
        }
        private bool IsMalicious(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            string pattern = @"(\b(select|insert|update|delete|drop|script|iframe|alert|onmouseover)\b|<|>|'|""|--|;|\|)";
            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        public static async Task HandleRedirect(HttpContext context, string targetUrl, bool isAjaxRequest)
        {
            if (isAjaxRequest)
            {
                context.Response.StatusCode = 200;
                var payload = new { redirectTo = targetUrl };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(payload));
            }
            else
            {
                context.Response.Redirect(targetUrl);
            }
        }


    }
}
