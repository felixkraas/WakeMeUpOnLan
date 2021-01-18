using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using WakeMeUpOnLan.Services;

namespace WakeMeUpOnLan {
    public class Program {

        private static readonly string LogTemplate = "[{Timestamp:dd.MM.yyyy-HH:mm:ss}] [{Level:u}] - {Message:lj}{NewLine}{Exception}";

        public static void Main( string[] args ) {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override( "Microsoft", LogEventLevel.Information )
                .MinimumLevel.Override( "Microsoft.AspNetCore", LogEventLevel.Warning )
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Async( a => a.File( @"logs\WakeMeUpOnLan-.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 14, encoding: Encoding.UTF8, outputTemplate: LogTemplate, formatProvider: new CultureInfo( "de-de" ) ) )
                .CreateLogger();

            try {
                Log.Information( "Starting web host" );
                var host = CreateHostBuilder( args ).Build();
                using( var scope = host.Services.CreateScope() ) {
                    var db = scope.ServiceProvider.GetRequiredService<WolContext>();
                    db.Database.Migrate();
                }
                host.Run();
                return;
            } catch( Exception ex ) {
                Log.Fatal( ex, "Host terminated unexpectedly" );
                return;
            } finally {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder( string[] args ) =>
            Host.CreateDefaultBuilder( args )
                .UseSerilog()
                .ConfigureWebHostDefaults( webBuilder => {
                    webBuilder.UseStartup<Startup>();
                } );
    }
}
