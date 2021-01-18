using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace WakeMeUpOnLan.Services {
    public class ApiKeyMiddleware {

        private readonly RequestDelegate _next;
        private const string ApiKeyName = "ApiKey";

        public ApiKeyMiddleware( RequestDelegate next ) {
            _next = next;
        }

        public async Task InvokeAsync( HttpContext context, WolContext dataContext ) {
            if( !context.Request.Headers.TryGetValue( ApiKeyName, out var extractedApiKey ) ) {
                context.Response.StatusCode = 401;
                Log.Warning( "Api Key was not provided." );
                return;
            }


            if( !await dataContext.ApiUsers.AnyAsync( key => key.ApiKey == extractedApiKey ) ) {
                context.Response.StatusCode = 401;
                Log.Warning( $"Unauthorized Key. ({extractedApiKey})" );
                return;
            }

            await _next( context );
        }

    }
}
