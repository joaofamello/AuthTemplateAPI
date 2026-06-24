using System.IdentityModel.Tokens.Jwt;
using AuthTemplateAPI.Models;
using AuthTemplateAPI.Services;
using AuthTemplateAPI.Settings;
using Microsoft.Extensions.Options;

namespace AuthTemplateAPI.Tests;

public class TokenServiceTests
{
    private TokenService CriarServico()
    {
        var settings = new JwtSettings
        {
            SecretKey = "ChaveDeTesteComPeloMenos32Caracteres!",
            Issuer = "JwtAuthTemplate.Tests",
            Audience = "ClienteDeTeste",
            ExpirationMinutes = 60
        };

        return new TokenService(Options.Create(settings));
    }

    [Fact]
    public void GenerateToken_DeveIncluirClaimsDeUsername_EId()
    {
        var servico = CriarServico();
        var usuario = new Usuario { Id = "abc-123", UserName = "teste" };
        
        var token  = servico.GenerateToken(usuario);

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        
        Assert.Equal("teste", jwt.Claims.First(c => c.Type == "username").Value);
        Assert.Equal("abc-123", jwt.Claims.First(c => c.Type == "id").Value);
        Assert.Equal("JwtAuthTemplate.Tests", jwt.Issuer);
    }
}