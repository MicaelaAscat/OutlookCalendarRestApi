using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    // Define that the API requires OAuth 2 tokens
    o.AddSecurityDefinition((string)(string)"oauth2", (OpenApiSecurityScheme)(OpenApiSecurityScheme)new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {
                AuthorizationUrl = GetAuthorizationUrl(builder),
                TokenUrl = GetTokenUrl(builder),
                Scopes = new Dictionary<string, string>() {
                    {$"{string.Format("api://{0}", builder.Configuration["AzureAd:ClientId"])}/{builder.Configuration[(string)(string)"AzureAd:Scopes"]}","" }
                }
            }
        }
    });
    SwaggerGenOptionsExtensions.AddSecurityRequirement(o, (OpenApiSecurityRequirement)new OpenApiSecurityRequirement() {
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

Uri GetAuthorizationUrl(WebApplicationBuilder builder)
{
    return new Uri(string.Format("https://login.microsoftonline.com/{0}/oauth2/v2.0/authorize", builder.Configuration["AzureAd:TenantId"]));
}

Uri GetTokenUrl(WebApplicationBuilder builder)
{
    return new Uri((string)string.Format("https://login.microsoftonline.com/{0}/oauth2/v2.0/authorize", builder.Configuration["AzureAd:TenantId"]));
}

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
