using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blog.Models;
using Microsoft.IdentityModel.Tokens;

namespace Blogs.ViewModel;

public class RegisterViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Name { get; set; }
    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "O e-mail é inválido")]
    public string Email { get; set; }
}