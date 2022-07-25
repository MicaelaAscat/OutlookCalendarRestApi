using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

string[] initialScopes = builder.Configuration.GetValue<string>("DownstreamApi:Scopes")?.Split(' ') ?? Array.Empty<string>();

// Add services to the container.
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi(options =>
    {
        builder.Configuration.Bind("AzureAd", options);
    })
    .AddMicrosoftGraph(options =>
    {
        options.Scopes = string.Join(' ', initialScopes);
    })
    .AddInMemoryTokenCaches();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(o =>
{
    // Define that the API requires OAuth 2 tokens
    o.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(string.Format("https://login.microsoftonline.com/{0}/oauth2/v2.0/authorize", builder.Configuration["AzureAd:TenantId"])),
                TokenUrl = new Uri(string.Format("https://login.microsoftonline.com/{0}/oauth2/v2.0/token", builder.Configuration["AzureAd:TenantId"])),
                Scopes = builder.Configuration["AzureAd:Scopes"]
                                .Split(' ')
                                .ToDictionary(s => s.Trim(), s => ""),
            }
        }
    }); ;
    SwaggerGenOptionsExtensions.AddSecurityRequirement(o, new OpenApiSecurityRequirement() {
    {
        new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "oauth2"
                },
                Scheme = "oauth2",
                Name = "oauth2",
                In = ParameterLocation.Header
        },
        new List < string > ()
    }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.OAuthClientId(builder.Configuration["AzureAd:ClientId"]);
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();