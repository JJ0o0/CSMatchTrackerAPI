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
public class PartidasController(AppDbContext context) : ControllerBase {
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PartidaDto>>> GetPartidas() {
        return await context.Partidas
            .Select(partida => new PartidaDto {
                Id = partida.Id,
                CampeonatoId = partida.CampeonatoId,
                DataHora = partida.DataHora,
                Mapa = partida.Mapa,
                PlacarTR = partida.PlacarTR,
                PlacarCT = partida.PlacarCT,
                Status = partida.Status
            }).ToListAsync();
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<PartidaDto>> GetPartida(long id) {
        var partida = await context.Partidas
            .Select(p => new PartidaDto {
                Id = p.Id,
                CampeonatoId = p.CampeonatoId,
                DataHora = p.DataHora,
                Mapa = p.Mapa,
                PlacarTR = p.PlacarTR,
                PlacarCT = p.PlacarCT,
                Status = p.Status
            }).FirstOrDefaultAsync(p => p.Id == id);

        if (partida is null) return NotFound();

        return partida;
    }

    [Authorize(Policy = "Admin")]
    [HttpPost]
    public async Task<ActionResult<PartidaDto>> PostPartida(PartidaCreateDto partidaDto) {
        var partida = new Partida {
            CampeonatoId = partidaDto.CampeonatoId,
            DataHora = partidaDto.DataHora,
            Mapa = partidaDto.Mapa,
            PlacarTR = partidaDto.PlacarTR,
            PlacarCT = partidaDto.PlacarCT,
            Status = partidaDto.Status
        };

        if (partida.CampeonatoId is not null) {
            var campeonatoExiste = await context.Campeonatos
                .AnyAsync(c => c.Id == partida.CampeonatoId);

            if (!campeonatoExiste) return BadRequest("Campeonato não encontrado.");
        }

        context.Partidas.Add(partida);
        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetPartida),
            new { id = partida.Id },
            new PartidaDto {
                Id = partida.Id,
                CampeonatoId = partida.CampeonatoId,
                DataHora = partida.DataHora,
                Mapa = partida.Mapa,
                PlacarTR = partida.PlacarTR,
                PlacarCT = partida.PlacarCT,
                Status = partida.Status
            }
        );
    }

    [Authorize(Policy = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPartida(long id, PartidaUpdateDto partidaDto) {
        var partidaExistente = await context.Partidas.FindAsync(id);
        if (partidaExistente is null) return NotFound();

        if (partidaDto.CampeonatoId is not null) {
            var campeonatoExiste = await context.Campeonatos
                .AnyAsync(c => c.Id == partidaDto.CampeonatoId);

            if (!campeonatoExiste) return BadRequest("Campeonato não encontrado.");
        }

        partidaExistente.CampeonatoId = partidaDto.CampeonatoId;
        partidaExistente.DataHora = partidaDto.DataHora;
        partidaExistente.Mapa = partidaDto.Mapa;
        partidaExistente.PlacarTR = partidaDto.PlacarTR;
        partidaExistente.PlacarCT = partidaDto.PlacarCT;
        partidaExistente.Status = partidaDto.Status;

        await context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Policy = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePartida(long id) {
        var partidaExistente = await context.Partidas.FindAsync(id);
        if (partidaExistente is null) return NotFound();

        context.Partidas.Remove(partidaExistente);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
