using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ORM;
using ServiceAggregator.Data;
using ServiceAggregator.Options;
using ServiceAggregator.Services;
using System.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization();

builder.Services.AddRepositories();
builder.Services.AddDataServices();

builder.Services.AddScoped<ApplicationDbContext>(conn => new ApplicationDbContext(builder.Configuration.GetConnectionString("DataAccessPostgreSqlProviderNeon")));

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
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:44492");
                      });
});

builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        options.AddSecurityDefinition("Bearer", openApiSecurityScheme);
        options.AddSecurityRequirement(openApiSecurityRequirement);
        options.EnableAnnotations();
    });
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddOptions();

builder.Services.Configure<MyOptions>(myOptions =>
{
    myOptions.ConnectionString = builder.Configuration.GetConnectionString("DataAccessPostgreSqlProviderNeon");
    myOptions.Issuer = builder.Configuration["Jwt:Issuer"];
    myOptions.Audience = builder.Configuration["Jwt:Audience"];
    myOptions.Subject = builder.Configuration["Jwt:Subject"];
    myOptions.Key = builder.Configuration["Jwt:Key"];
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();    // аутентификация
app.UseAuthorization();     // авторизация


app.UseCors(MyAllowSpecificOrigins);
using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    var myDependency = services.GetRequiredService<IDbInitializer>();
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.MigrateAsync().Wait();
    myDependency.Seed().Wait();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}");

app.MapFallbackToFile("index.html"); ;

app.Run();
