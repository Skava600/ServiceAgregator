using Microsoft.OpenApi.Models;

namespace ServiceAggregator.Services.JWT
{
    public static class JwtConfiguration
    {
        public static IServiceCollection AddJwt(this IServiceCollection services)
        {
            var openApiSecurityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter a bearer token (JWT):",
            };

            var openApiSecurityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                    },
                    Array.Empty<string>()
                },
            };

            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("https://localhost:44492")
                                      .WithMethods("PUT", "DELETE", "GET", "POST", "UPDATE")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();

                                  });
            });

            services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", openApiSecurityScheme);
                options.AddSecurityRequirement(openApiSecurityRequirement);
                options.EnableAnnotations();
            });

            return services;
        }
    }
}
