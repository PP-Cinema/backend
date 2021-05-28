using System;
using Backend.DTO;

namespace Backend.Utilities
{
    public interface IJwtManager
    {
        AuthenticationDto GenerateTokens(string login, string role, DateTime startDate);
        bool ContainsRefreshToken(string refreshToken);
    }
}