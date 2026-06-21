using AuthTemplateAPI.Data.DTOs;
using AuthTemplateAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthTemplateAPI.Services;

public class UsuarioService
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly TokenService _tokenService;

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
        {
            var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ApplicationException("Falha ao cadastrar utilizador: " + errorMessages);
        }
    }

    public async Task<string> Login(LoginUsuarioDto dto)
    {
        var result = await _signInManager.PasswordSignInAsync(dto.Username, dto.Password, false, true);

        if (!result.Succeeded)
            throw new ApplicationException("Usuário não autenticado ou credenciais inválidas.");

        var usuario = await _userManager.FindByNameAsync(dto.Username);

        if (usuario is null)
            throw new ApplicationException("Falha interna ao carregar os dados do utilizador.");
        
        var token = _tokenService.GenerateToken(usuario);
        return token;
    }
}