using System.ComponentModel.DataAnnotations;

namespace AuthTemplateAPI.Data.DTOs;

public record LoginUsuarioDto(
    [Required] string Username,
    [Required] string Password);