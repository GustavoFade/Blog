using Blog.Data;
using Blog.Models;
using Blogs.Services;
using Blogs.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blogs.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
    public readonly TokenService _tokenService;
    public AccountController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }
    
    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login([FromServices]BlogDataContext context, LoginViewModel model)
    {
        try
        {
            var user = await context.Users.AsNoTracking()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null) return StatusCode(401, "Usuário ou senha inválido!");

            var passwordIncorreto = PasswordHasher.Verify(user.PasswordHash, model.Password);
            if (passwordIncorreto) return StatusCode(401, "Usuário ou senha inválido!");

            var token = _tokenService.GenerateToken(user);
            return Ok(token);
        }
        catch (Exception ex)
        {
           return StatusCode(500, "Falha interna no servidor" + ex);
        }
    }

    [HttpPost("v1/accounts")]
    public async Task<IActionResult> Post([FromBody]RegisterViewModel model, 
                              [FromServices]BlogDataContext context)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Slug = model.Email.Replace("@","-").Replace(".","-")
            };
            var password = PasswordGenerator.Generate(10);
            user.PasswordHash = PasswordHasher.Hash(password);

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return Ok(user);
        }
        catch (Exception ex)
        {
           return StatusCode(500, "Falha interna no servidor" + ex);
        }
    }
}