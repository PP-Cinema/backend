using System;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.DTO;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Utilities
{
    public class JwtManager : IJwtManager
    {
        private readonly JwtConfig jwtConfig;
        private readonly ConcurrentDictionary<string, string> refreshTokens;
        private readonly byte[] secret;

        public JwtManager(JwtConfig jwtConfig)
        {
            this.jwtConfig = jwtConfig;

            refreshTokens = new ConcurrentDictionary<string, string>();
            secret = Encoding.ASCII.GetBytes(this.jwtConfig.Secret);
        }


        public AuthenticationDto GenerateTokens(string login, string role, DateTime startDate)
        {
            var accessToken = new JwtSecurityToken(
                jwtConfig.Issuer,
                jwtConfig.Audience,
                new[] {new Claim(ClaimTypes.Role, role), new Claim(ClaimTypes.Name, login)},
                expires: startDate.AddMinutes(jwtConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret),
                    SecurityAlgorithms.HmacSha256Signature)
            );
            var refreshToken = new JwtSecurityToken(
                jwtConfig.Issuer,
                jwtConfig.Audience,
                new[] {new Claim(ClaimTypes.Role, role), new Claim(ClaimTypes.Name, login)},
                expires: startDate.AddMinutes(jwtConfig.RefreshTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret),
                    SecurityAlgorithms.HmacSha256Signature)
            );

            var result = new AuthenticationDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken),
                Role = role
            };

            refreshTokens.TryAdd(result.RefreshToken, result.RefreshToken);
            return result;
        }

        public bool ContainsRefreshToken(string refreshToken)
        {
            return refreshTokens.ContainsKey(refreshToken);
        }
    }
}