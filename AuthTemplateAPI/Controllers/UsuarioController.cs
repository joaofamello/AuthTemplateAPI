using AuthTemplateAPI.Data.DTOs;
using AuthTemplateAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthTemplateAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _service;

    public UsuarioController(UsuarioService service)
    {
        _service = service;
    }

    [HttpPost("cadastro")]
    public async Task<IActionResult> CadastrarUsuario(CreateUsuarioDto dto)
    {
        await _service.Cadastrar(dto);
        return Ok("Usuário cadastrado!");
    } 
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUsuarioDto dto)
    {
        var token = await _service.Login(dto);
        return Ok(token);
    }
}