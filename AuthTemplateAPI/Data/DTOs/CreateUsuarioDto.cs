using System.ComponentModel.DataAnnotations;
using AuthTemplateAPI.Models;

namespace AuthTemplateAPI.Data.DTOs;

public record CreateUsuarioDto
{
    [Required] public string Username { get; init; }
    [Required] [DataType(DataType.Password)] public string Password { get; init; }
    [Required] [Compare("Password")] public string RePassword { get; init; }
    
    public static implicit operator Usuario(CreateUsuarioDto dto)
    {
        return new Usuario()
        {
            UserName = dto.Username
        };
    }
};