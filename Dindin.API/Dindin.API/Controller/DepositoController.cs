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
    public async Task<IActionResult> InserirDeposito(InserirDepositoDto dto)
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
    public async Task<IActionResult> DeletarDeposito(DeletarDepositoDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var depositoExiste = await _context.Depositos.AnyAsync(d => d.UserId == userId && d.Id == dto.Id);

        if (!depositoExiste) return NotFound("Depósito não existe");
        
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

        return Ok($"Deposito excluido com sucesso!");
    }

    [Authorize]
    [HttpPatch("atualizarDeposito")]
    public async Task<IActionResult> AtualizarDeposito(AtualizarDepositoDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        Group? groupExists = null;

        if (dto.GroupId.HasValue) groupExists = await _context.Grupo.FirstOrDefaultAsync(g => g.Id == dto.GroupId);

        bool userInGroup = false;

        if (groupExists != null)
        {
            userInGroup = await _context.UserGrupo.AnyAsync(u => u.GrupoId == dto.GroupId && u.UserId == userId);

            if (!userInGroup) return BadRequest("Usuário não pertence ao grupo");
        }
        
        var depositoExiste = await _context.Depositos.FirstOrDefaultAsync(d => d.Id == dto.Id && d.UserId == userId);

        if (depositoExiste != null)
        {
            if (dto.Descricao != null) depositoExiste.Descricao = dto.Descricao;
            if (dto.Categoria != null) depositoExiste.Categoria = dto.Categoria;
            if (dto.Quantidade != null) depositoExiste.Quantidade = dto.Quantidade.Value;
            if (dto.MetodoDeposito != null) depositoExiste.MetodoDeposito = dto.MetodoDeposito.Value;
            
            await _context.SaveChangesAsync();
        }
        else
        {
            return BadRequest("deposito não existe");
        }
        
        return Ok($"Deposito de descrição {dto.Descricao} atualizado com sucesso!");
    }

    [Authorize]
    [HttpGet("obterDepositos")]
    public async Task<IActionResult> ObterDepositos([FromQuery] ObterDepositosDto dto)
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