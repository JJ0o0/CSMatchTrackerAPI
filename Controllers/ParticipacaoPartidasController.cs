using CSMatchTracker.Data;
using CSMatchTracker.DTOs;
using CSMatchTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSMatchTracker.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParticipacaoPartidasController(AppDbContext context) : ControllerBase {
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParticipacaoPartidaDto>>> GetParticipacaoPartidas() {
        return await context.ParticipacaoPartidas
            .Select(participacao => new ParticipacaoPartidaDto {
                Id = participacao.Id,
                JogadorId = participacao.JogadorId,
                PartidaId = participacao.PartidaId,
                Lado = participacao.Lado,
                Kills = participacao.Kills,
                Deaths = participacao.Deaths,
                Assists = participacao.Assists,
                Headshots = participacao.Headshots,
                Mvp = participacao.Mvp,
            }).ToListAsync();
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<ParticipacaoPartidaDto>> GetParticipacaoPartida(long id) {
        var participacao = await context.ParticipacaoPartidas
            .Select(participacao => new ParticipacaoPartidaDto {
                Id = participacao.Id,
                JogadorId = participacao.JogadorId,
                PartidaId = participacao.PartidaId,
                Lado = participacao.Lado,
                Kills = participacao.Kills,
                Deaths = participacao.Deaths,
                Assists = participacao.Assists,
                Headshots = participacao.Headshots,
                Mvp = participacao.Mvp,
            }).FirstOrDefaultAsync(p => p.Id == id);

        if (participacao is null) return NotFound();

        return participacao;
    }

    [Authorize(Policy = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ParticipacaoPartidaDto>> PostParticipacaoPartida(ParticipacaoPartidaCreateDto participacaoDto) {
        var participacao = new ParticipacaoPartida {
            JogadorId = participacaoDto.JogadorId,
            PartidaId = participacaoDto.PartidaId,
            Lado = participacaoDto.Lado,
            Kills = participacaoDto.Kills,
            Deaths = participacaoDto.Deaths,
            Assists = participacaoDto.Assists,
            Headshots = participacaoDto.Headshots,
            Mvp = participacaoDto.Mvp,
        };

        var jogadorExiste = await context.Jogadores.AnyAsync(j => j.Id == participacao.JogadorId);
        if (!jogadorExiste) return BadRequest("Jogador não encontrado.");

        var partidaExiste = await context.Partidas.AnyAsync(p => p.Id == participacao.PartidaId);
        if (!partidaExiste) return BadRequest("Partida não encontrada.");

        var participacaoExiste = await context.ParticipacaoPartidas
                                              .AnyAsync(
                                                p => p.JogadorId == participacao.JogadorId &&
                                                     p.PartidaId == participacao.PartidaId
                                              );

        if (participacaoExiste) return Conflict("Participação já existe!");

        context.ParticipacaoPartidas.Add(participacao);
        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetParticipacaoPartida),
            new { id = participacao.Id },
            new ParticipacaoPartidaDto {
                Id = participacao.Id,
                JogadorId = participacao.JogadorId,
                PartidaId = participacao.PartidaId,
                Lado = participacao.Lado,
                Kills = participacao.Kills,
                Deaths = participacao.Deaths,
                Assists = participacao.Assists,
                Headshots = participacao.Headshots,
                Mvp = participacao.Mvp,
            }
        );
    }

    [Authorize(Policy = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutParticipacaoPartida(long id, ParticipacaoPartidaUpdateDto participacaoDto) {
        var participacaoExistente = await context.ParticipacaoPartidas.FindAsync(id);
        if (participacaoExistente is null) return NotFound();

        participacaoExistente.Lado = participacaoDto.Lado;
        participacaoExistente.Kills = participacaoDto.Kills;
        participacaoExistente.Deaths = participacaoDto.Deaths;
        participacaoExistente.Assists = participacaoDto.Assists;
        participacaoExistente.Headshots = participacaoDto.Headshots;
        participacaoExistente.Mvp = participacaoDto.Mvp;

        await context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Policy = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteParticipacaoPartida(long id) {
        var participacaoExistente = await context.ParticipacaoPartidas.FindAsync(id);
        if (participacaoExistente is null) return NotFound();

        context.ParticipacaoPartidas.Remove(participacaoExistente);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
