using Blog.Data;
using Blog.Models;
using Blogs.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blogs.Controllers;

[Authorize(Roles = "admin")]
[ApiController]
public class CategoryController : ControllerBase
{
    public BlogDataContext _blogDataContext { get; set; }
    public CategoryController(
        [FromServices] BlogDataContext blogDataContext
        )
    {
        _blogDataContext = blogDataContext;
    }

    [HttpPost("v1/categories")]
    public async Task<IActionResult> PostAsync([FromBody] CreateCategoryViewModel model)
    {
        try
        {
            var category = new Category
            {
                Id = 0,
                Name = model.Name,
                Slug = model.Slug
            };
            _blogDataContext.Categories.Add(category);
            await _blogDataContext.SaveChangesAsync();
            return Created($"v1/categories/{category.Id}", category);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, "Não foi possível incluir a categoria");
        }
        catch
        {
           return StatusCode(500, "Falha interna no servidor");
        }
    }

    [HttpGet("v1/categories")]
    public async Task<IActionResult> GetAsync()
    {
        var categories = await _blogDataContext
            .Categories
            .AsNoTracking()
            .ToListAsync();
        return Ok(categories);
    }

    [HttpGet("v1/categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var category = await _blogDataContext
            .Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (category == null) return NotFound();
        
        return Ok(category);
    }

    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] Category model)
    {
        var category = await _blogDataContext
            .Categories
            .FirstOrDefaultAsync(x => x.Id == id);

        if (category == null) return NotFound();
        
        category.Name = model.Name;
        category.Posts = model.Posts;
        category.Slug = model.Slug;

        _blogDataContext.Update(category);
        await _blogDataContext.SaveChangesAsync();

        return Ok(model);
    }

    [HttpDelete("v1/categories/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var category = await _blogDataContext
            .Categories
            .FirstOrDefaultAsync(x => x.Id == id);

        if (category == null) return NotFound();
        
        _blogDataContext.Remove(category);
        await _blogDataContext.SaveChangesAsync();

        return Ok(category);
    }
}