using Motohusaria.Services;
using Motohusaria.DomainClasses;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace Motohusaria.Web.Utils.Authorization
{
    [InjectableService(typeof(ITokenService))]
    public class TokenService : ITokenService
    {
        private readonly IRoleQueryService _roleQueryService;
        private readonly JWTOptions _jWTOptions;

        public TokenService(IOptions<JWTOptions> jWTOptions, IRoleQueryService roleQueryService)
        {
            _jWTOptions = jWTOptions.Value;
            _roleQueryService = roleQueryService;
        }

        public async Task<string> GenerateUserTokenAsync(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTOptions.Secret));
            var roles = await _roleQueryService.GetRolesByUserId(user.Id);
            IEnumerable<Claim> claims = new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddDays(5)).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
            };
            claims = claims.Concat(roles.Select(s => new Claim(ClaimTypes.Role, s.Name)));
            var token = new JwtSecurityToken(
                issuer: _jWTOptions.Issuer,
                audience: _jWTOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(5),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}
