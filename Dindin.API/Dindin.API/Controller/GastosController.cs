using System.Security.Claims;
using System.Security.Cryptography;
using Dindin.API.Data;
using Dindin.API.DTO;
using Dindin.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dindin.API.Controller;

[ApiController]
[Route("/api/[controller]")]
public class GastosController : ControllerBase
{
    private readonly AppDbContext _context;

    public GastosController(AppDbContext context)
    {
        _context = context;
    }

    private string GenerateInviteCode()
    {
        var bytes = new byte[8];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToHexString(bytes);
    }

    private async Task<string> VerifyInviteCode(string inviteCode)
    {
        var codigoExiste = await _context.Grupo.FirstOrDefaultAsync(g => g.CodigoConvite == inviteCode);

        string newInviteCode = "";

        if (codigoExiste == null)
        {
            newInviteCode = inviteCode; 
            return newInviteCode ;
        }

        if (inviteCode != "")
        {
            while (codigoExiste.ToString() == inviteCode)
            {
                newInviteCode = GenerateInviteCode();
                codigoExiste = await _context.Grupo.FirstOrDefaultAsync(g => g.CodigoConvite == newInviteCode);
                if (codigoExiste == null) break;
            }
        }

        return newInviteCode;
    }

    [Authorize]
    [HttpPost("inserirGastos")]
    public async Task<IActionResult> InserirGastos(InserirGastoDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        Group? groupExists = null;
        
        if (dto.GrupoId.HasValue) groupExists = await _context.Grupo.FirstOrDefaultAsync(g => g.Id == dto.GrupoId);

        bool userInGroup = false;
        
        if (groupExists != null)
        {
            userInGroup = await _context.UserGrupo.AnyAsync(ug => ug.UserId == userId && ug.GrupoId == groupExists.Id);

            if (!userInGroup) return BadRequest("Usuário nao pertence ao grupo!");
        }

        Gastos? novoGasto = null;

        if (userInGroup)
        {
            novoGasto = new Gastos
            {
                Descricao = dto.Descricao,
                Quantidade = dto.Quantidade,
                MetodoPagamento = dto.MetodoPagamento,
                Categoria = dto.Categoria,
                UserId = userId,
                GrupoId = groupExists.Id
            };
        }
        else
        {
            novoGasto = new Gastos
            {
                Descricao = dto.Descricao,
                Quantidade = dto.Quantidade,
                MetodoPagamento = dto.MetodoPagamento,
                Categoria = dto.Categoria,
                UserId = userId
            };
        }
        
        _context.Gastos.Add(novoGasto);
        await _context.SaveChangesAsync();

        return Ok($"Gasto de descrição {dto.Descricao} de valor {dto.Quantidade} inserido no banco de dados");
    }
    
    [Authorize]
    [HttpPatch("atualizarGastos")]
    public async Task<IActionResult> AtualizarGastos(AtualizarGastoDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var gastoExiste = await _context.Gastos.FirstOrDefaultAsync(g => g.UserId == userId && g.Id == dto.Id);

        if (gastoExiste != null)
        {
            if (dto.Categoria != null) gastoExiste.Categoria = dto.Categoria;
            if (dto.Descricao != null) gastoExiste.Descricao = dto.Descricao;
            if (dto.MetodoPagamento != null) gastoExiste.MetodoPagamento = dto.MetodoPagamento.Value;
            if (dto.Quantidade != null) gastoExiste.Quantidade = dto.Quantidade.Value;

            await _context.SaveChangesAsync();
        }
        else
        {
            return BadRequest("Gasto inexistente! (id nao foi identificado no banco de dados)");
        }
        
        return Ok($"Gasto de descrição {dto.Descricao} de valor {dto.Quantidade} atualizado no banco de dados");
    }

    [Authorize]
    [HttpDelete("deletarGastos")]
    public async Task<IActionResult> DeletarGastos(DeletarGastoDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var gastoUsuario = await _context.Gastos.FirstOrDefaultAsync(g => userId == g.UserId && g.Id == dto.Id);
        
        if (gastoUsuario == null) return BadRequest("Usuario ou ID incorretos");
        
        try
        {
            var gasto = await _context.Gastos.FirstOrDefaultAsync(g => g.Id == dto.Id);
                
            _context.Gastos.Remove(gasto);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }

        return Ok($"Gasto de id {dto.Id} deletado com sucesso!");
    }

    [Authorize]
    [HttpGet("obterGastos")]
    public async Task<IActionResult> ObterGastos([FromQuery] ObterGastosDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        Group? groupExists = null;
        
        if (dto.GroupId.HasValue)
        {
            groupExists = await _context.Grupo.FirstOrDefaultAsync(g => g.Id == dto.GroupId);
        }

        bool userInGroup = false;
        
        if (groupExists != null)
        {
            userInGroup = await _context.UserGrupo.AnyAsync(ug => ug.UserId == userId && ug.GrupoId == groupExists.Id);

            if (!userInGroup)
            {
                return BadRequest("Usuário nao pertence ao grupo!");
            }
        }

        IQueryable<Gastos> query = null;

        query = userInGroup
            ? _context.Gastos.Where(g => g.UserId == userId && g.GrupoId == groupExists.Id)
            : _context.Gastos.Where(g => g.UserId == userId);
        
        if (dto.DataInicio.HasValue) query = query.Where(g => g.AddAt >= dto.DataInicio.Value.ToUniversalTime());
        if (dto.DataFim.HasValue) query = query.Where(g => g.AddAt <= dto.DataFim.Value.ToUniversalTime());

        var resultado = await query.ToListAsync();
        
        return resultado.Any() ? Ok(resultado) : Ok("Não existem dados cadastrados");
    }
}