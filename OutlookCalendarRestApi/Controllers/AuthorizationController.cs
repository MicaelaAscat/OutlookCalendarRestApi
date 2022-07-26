using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System.Text.Encodings.Web;

namespace OutlookCalendarRestApi.Controllers
{
    [ApiController]
    [AuthorizeForScopes(Scopes = new[] { "access_as_user" })]
    [Route("Authorization")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthorizationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public string getAuthorizationUrl()
        {
            return string.Format("{0}/{1}/oauth2/v2.0/authorize?client_id={2}&prompt=select_account&scope={3}&response_type=token&redirect_uri={4}",
                _configuration.GetValue<string>("AzureAd:Instance"),
                _configuration.GetValue<string>("AzureAd:TenantId"),
                _configuration.GetValue<string>("AzureAd:ClientId"),
                UrlEncoder.Default.Encode(_configuration.GetValue<string>("DownstreamApi:Scopes")),
                UrlEncoder.Default.Encode(_configuration.GetValue<string>("DownstreamApi:RedirectUri"))
                );
        }
    }
}