using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TAM.LogisticSystem.Models;
using TAM.Passport.SDK;
using Microsoft.EntityFrameworkCore;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Services
{
    public class AuthenticationService
    {
        public const string CookieAuthenticationScheme = "TLS_Authentication_Cookie";

        public string LocationClaimType => "Location";

        private readonly LogisticDbContext DB;
        private readonly WebEnvironmentService Env;

        public AuthenticationService(LogisticDbContext db, WebEnvironmentService env)
        {
            this.DB = db;
            this.Env = env;
        }

        public async Task<ClaimsPrincipal> CreateLoginCookie(UserClaims claims)
        {
            var id = new ClaimsIdentity(CookieAuthenticationScheme);

            id.AddClaim(new Claim(ClaimTypes.NameIdentifier, claims.Username));
            id.AddClaim(new Claim(ClaimTypes.Name, claims.Name));
            id.AddClaim(new Claim(ClaimTypes.Sid, claims.SessionId.ToString()));

            // Integrate menu filtration to MVC Role!
            var menus = await DB.AppRoleMenuMapping.Where(Q => claims.Roles.Contains(Q.AppRoleName)).Select(Q => Q.AppMenuName).ToListAsync();
            claims.Roles.AddRange(menus);
            claims.Roles = claims.Roles.Distinct().ToList();

            foreach (var role in claims.Roles)
            {
                id.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            if (string.IsNullOrEmpty(claims.LocationCode) == false)
            {
                id.AddClaim(new Claim(LocationClaimType, claims.LocationCode));
            }

            return new ClaimsPrincipal(id);
        }

        public async Task<UserClaims> CompileToTangoUserInfo(LoginClaims claims)
        {
            var mapping = await DB.UserMapping.FirstOrDefaultAsync(Q => Q.Username == claims.Username);

            return new UserClaims
            {
                Name = claims.Name,
                Username = claims.Username,
                Roles = claims.Roles,
                SessionId = claims.SessionId,
                LocationCode = mapping?.LocationCode
            };
        }

        public UserClaims GetCurrentTangoUser()
        {
            var id = Env.GetCurrentUserPrincipal();

            var user = new UserClaims();
            user.Name = id.Claims.First(Q => Q.Type == ClaimTypes.Name).Value;
            user.Username = id.Claims.First(Q => Q.Type == ClaimTypes.NameIdentifier).Value;
            user.Roles = id.Claims.Where(Q => Q.Type == ClaimTypes.Role).Select(Q => Q.Value).ToList();
            user.SessionId = Guid.Parse(id.Claims.First(Q => Q.Type == ClaimTypes.Sid).Value);
            user.LocationCode = id.Claims.FirstOrDefault(Q => Q.Type == LocationClaimType)?.Value;

            return user;
        }
    }
}
