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
        private readonly ILogger<AuthorizationController> _logger;

        public AuthorizationController(ILogger<AuthorizationController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public string getAuthorizationUrl()
        {
            try
            {
                _logger.LogInformation("call getAuthorizationUrl");
                return string.Format("{0}/{1}/oauth2/v2.0/authorize?client_id={2}&prompt=select_account&scope={3}&response_type=token&redirect_uri={4}",
                                        _configuration.GetValue<string>("AzureAd:Instance"),
                                        _configuration.GetValue<string>("AzureAd:TenantId"),
                                        _configuration.GetValue<string>("AzureAd:ClientId"),
                                        UrlEncoder.Default.Encode(_configuration.GetValue<string>("DownstreamApi:Scopes")),
                                        UrlEncoder.Default.Encode(_configuration.GetValue<string>("DownstreamApi:RedirectUri"))
                                     );
            }
            catch(Exception ex)
            {
                _logger.LogError(string.Format("ERROR getAuthorizationUrl: {0}", ex.Message));
                throw;
            }
        }
    }
}