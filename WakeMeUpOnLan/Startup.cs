using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using WakeMeUpOnLan.Services;

namespace WakeMeUpOnLan {
    public class Startup {
        public Startup( IConfiguration configuration ) {
            Configuration = configuration;
        }

        public IConfiguration Configuration {
            get;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services ) {
            services.AddDbContext<WolContext>();
            services.AddControllers();

            services.AddControllers().AddNewtonsoftJson( options => {
                options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Error += JsonSerializerError;
#if DEBUG
                options.SerializerSettings.Formatting = Formatting.Indented;
#endif
            } );
            services.AddResponseCompression();
        }

        private void JsonSerializerError( object? sender, ErrorEventArgs e ) {
            Log.Error( e.ErrorContext.Error.Message );
            Log.Error( e.ErrorContext.Error.StackTrace );
            Log.Error( e.ErrorContext.Path );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env ) {
            if( env.IsDevelopment() ) {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();

            app.UseMiddleware<ApiKeyMiddleware>();

            if( !env.IsDevelopment() ) {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints( endpoints => {
                endpoints.MapControllers();
            } );


        }
    }
}
