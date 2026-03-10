using System.Security.Claims;
using Dindin.API.Data;
using Dindin.API.DTO;
using Dindin.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dindin.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class DepositoController : ControllerBase
{
    private readonly AppDbContext _context;

    public DepositoController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpPost("inserirDeposito")]
    public async Task<IActionResult> inserirDeposito(inserirDepositoDTOs dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var novoDeposito = new Depositos
        {
            Descricao = dto.Descricao,
            Quantidade = dto.Quantidade,
            Categoria = dto.Categoria,
            MetodoDeposito = dto.MetodoDeposito,
            UserId = userId
        };

        await _context.Depositos.AddAsync(novoDeposito);
        await _context.SaveChangesAsync();

        return Ok($"Deposito de descricao {dto.Descricao} inserido com sucesso no banco de dados!");
    }

    [Authorize]
    [HttpDelete("deletarDeposito")]
    public async Task<IActionResult> deletarDeposito(deletarDepositoDTOs dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var depositoExiste = await _context.Depositos.AnyAsync(d => d.UserId == userId && d.Id == dto.Id);

        if (depositoExiste)
        {
            try
            {
                var deposito = await _context.Depositos.FirstOrDefaultAsync(d => d.Id == dto.Id);

                _context.Depositos.Remove(deposito);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        return Ok($"Deposito excluido com sucesso!");
    }

    [Authorize]
    [HttpPatch("atualizarDeposito")]
    public async Task<IActionResult> atualizarDeposito(atualizarDepositoDTOs dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var depositoExiste = await _context.Depositos.FirstOrDefaultAsync(d => d.Id == dto.Id && d.UserId == userId);

        var deposito = await _context.Depositos.FirstOrDefaultAsync(d => d.UserId == userId);

        if (depositoExiste != null)
        {
            if (dto.Descricao != null) deposito.Descricao = dto.Descricao;
            if (dto.Categoria != null) deposito.Categoria = dto.Categoria;
            if (dto.Quantidade != null) deposito.Quantidade = dto.Quantidade.Value;
            if (dto.MetodoDeposito != null) deposito.MetodoDeposito = dto.MetodoDeposito.Value;
        }

        await _context.SaveChangesAsync();

        return Ok($"Deposito de descrição {dto.Descricao} atualizado com sucesso!");
    }

    [Authorize]
    [HttpGet("obterDepositos")]
    public async Task<IActionResult> obterDepositos([FromQuery] obterDepositosDTOs dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var query = _context.Depositos.Where(d => d.UserId == userId);

        if (dto.DataInicio.HasValue) query = query.Where(d => d.AddAt >= dto.DataInicio.Value.ToUniversalTime());
        if (dto.DataFim.HasValue) query = query.Where(d => d.AddAt <= dto.DataFim.Value.ToUniversalTime());

        var resultado = await query.ToListAsync();

        return Ok(resultado);
    }
}