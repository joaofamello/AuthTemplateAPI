using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthTemplateAPI.Models;
using AuthTemplateAPI.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthTemplateAPI.Services;

public class TokenService
{
    private readonly JwtSettings _jwtSettings;

    public TokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateToken(Usuario usuario)
    {
        Claim[] claims = new[]
        {
            new Claim("username", usuario.UserName),
            new Claim("id", usuario.Id),
        };
        
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        
        var signingCreadentials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            claims: claims,
            signingCredentials: signingCreadentials,
            audience: _jwtSettings.Audience,
            issuer: _jwtSettings.Issuer);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}