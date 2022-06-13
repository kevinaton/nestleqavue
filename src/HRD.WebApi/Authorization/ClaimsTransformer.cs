using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using HRD.WebApi.Data;
using HRD.WebApi.Logging;
using HRD.WebApi.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace HRD.WebApi.Authorization
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        private readonly IUserService _service;
        private readonly ILogger<ClaimsTransformer> _logger;
        public ClaimsTransformer(IConfiguration configuration, IUserService service, ILogger<ClaimsTransformer> logger)
        {
            _service = service;
            _logger = logger;
        }
        

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = (ClaimsIdentity)principal.Identity;
            _logger.LogInformation($"Identity of person logged in is {principal.Identity}");

            var username = identity.Name?.Split('\\').LastOrDefault();
            _logger.LogInformation($"Username is {username}");

            var userId = await _service.GetOrCreateUserIdByUsername(username);
            _logger.LogInformation($"Found {username} with Id = {userId}");

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

            var roles = await _service.GetUserRoles(userId);
            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
                _logger.LogInformation($"{username} has {role.Name}");
            }

            return await Task.FromResult(principal);
        }
    }
}