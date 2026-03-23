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
public class GrupoController : ControllerBase
{
    private readonly AppDbContext _context;

    public GrupoController(AppDbContext context)
    {
        _context = context;
    }
    
    private async Task<string> GenerateInviteCode()
    {
        var bytes = new byte[8];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        string code = Convert.ToHexString(bytes);
        string inviteCode = await VerifyInviteCode(code);
        return inviteCode;
    }

    private async Task<string> VerifyInviteCode(string inviteCode)
    {
        var codigoExiste = await _context.Grupo.FirstOrDefaultAsync(g => g.CodigoConvite == inviteCode);

        string newInviteCode = "";

        if (codigoExiste == null) return inviteCode;
        
        while (codigoExiste.ToString() == inviteCode)
        {
            newInviteCode = await GenerateInviteCode();
            codigoExiste = await _context.Grupo.FirstOrDefaultAsync(g => g.CodigoConvite == newInviteCode);
            if (codigoExiste == null) break;
        }

        return newInviteCode;
    }

    [Authorize]
    [HttpPost("createGroup")]
    public async Task<IActionResult> CreateGroup(CreateGroupDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var codigoConvite = await GenerateInviteCode();

        var newGrupo = new Group
        {
            CodigoConvite = codigoConvite,
            Nome = dto.Nome
        };

        _context.Grupo.Add(newGrupo);
        await _context.SaveChangesAsync();
        
        var newUserGrupo = new UserGroup
        {
            GrupoId = newGrupo.Id,
            UserId = userId
        };

        _context.UserGrupo.Add(newUserGrupo);
        await _context.SaveChangesAsync();

        string linkCodigoConvite = $"http/localhost:5113/api/{codigoConvite}";

        return Ok(new { CodigoConvite = codigoConvite, Link = linkCodigoConvite, GrupoId = newGrupo.Id });
    }

    [Authorize]
    [HttpPatch("{inviteCode}")]
    public async Task<IActionResult> JoinGroup([FromRoute] string inviteCode)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var grupo = await _context.Grupo.FirstOrDefaultAsync(g => g.CodigoConvite == inviteCode);

        if (grupo == null) return BadRequest("Código de convite inválido!");

        var userInGroup = await _context.UserGrupo.AnyAsync(u => u.UserId == userId && u.GrupoId == grupo.Id);

        if (userInGroup) return BadRequest("Usuário ja inserido no grupo!");

        var newUserInGroup = new UserGroup
        {
            UserId = userId,
            GrupoId = grupo.Id
        };

        _context.UserGrupo.Add(newUserInGroup);
        await _context.SaveChangesAsync();

        return Ok(new {GrupoId = grupo.Id});
    }  
}