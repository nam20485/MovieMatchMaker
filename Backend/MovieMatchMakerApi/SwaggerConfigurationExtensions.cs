using System.Reflection;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerUI;

namespace MovieMatchMakerApi
{
    public static class SwaggerConfigurationExtensions
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {         
            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "MovieMatchMaker API",
                    Description = "Web API for MovieMatchMaker",
                    //TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Nathan Miller",
                        Url = new Uri("https://www.github.com/nam20485")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://github.com/nam20485/MovieMatchMaker/blob/4d3698ca4ae75e0696ccc9db1325d84f9061069f/LICENSE")
                    }
                });

                // add generated Xml comments
               var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
               options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        private const bool SyntaxHighlightingEnabled = false;

        public static IApplicationBuilder ConfigureSwaggerUI(this IApplicationBuilder app)
        {
            return app.UseSwaggerUI(options =>
            {
                //options.SupportedSubmitMethods(new SubmitMethod[] { });   
                options.EnableTryItOutByDefault();
                options.EnableDeepLinking();
                options.DisplayRequestDuration();                                             
                options.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>()
                {
                    ["activated"] = SyntaxHighlightingEnabled
                };
                options.ConfigObject.AdditionalItems["requestSnippetsEnabled"] = true;
                options.ConfigObject.AdditionalItems["requestSnippets"] = new Dictionary<string, object>()
                {
                    ["defaultExpanded"] = false
                };
            });
        }
    }
}
