using AuthTemplateAPI.Data.DTOs;
using AuthTemplateAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthTemplateAPI.Services;

public class UsuarioService
{
    private UserManager<Usuario> _userManager;
    private SignInManager<Usuario> _signInManager;
    private TokenService _tokenService;

    public UsuarioService(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, TokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task Cadastrar(CreateUsuarioDto dto)
    {
        Usuario usuario = dto;
        var result = await _userManager.CreateAsync(usuario, dto.Password);

        if (!result.Succeeded)
            throw new ApplicationException("Falha ao cadastrar usuário: " + result.Errors);
    }

    public async Task<string> Login(LoginUsuarioDto dto)
    {
        var result = await _signInManager.PasswordSignInAsync(dto.Username, dto.Password, false, false);

        if (!result.Succeeded)
            throw new ApplicationException("Usuário não autenticado.");

        var usuario = _signInManager
            .UserManager
            .Users
            .FirstOrDefault(u => u.NormalizedUserName == dto.Username.ToUpper());
        
        var token = _tokenService.GenerateToken(usuario);
        return token;
    }
}