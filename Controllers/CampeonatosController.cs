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
public class CampeonatosController(AppDbContext context) : ControllerBase {
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CampeonatoDto>>> GetCampeonatos() {
        return await context.Campeonatos
            .Select(campeonato => new CampeonatoDto {
                Id = campeonato.Id,
                Nome = campeonato.Nome,
                Descricao = campeonato.Descricao,
                DataInicio = campeonato.DataInicio,
                DataFim = campeonato.DataFim,
                Status = campeonato.Status
            }).ToListAsync();
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<CampeonatoDto>> GetCampeonato(long id) {
        var campeonato = await context.Campeonatos
            .Select(campeonato => new CampeonatoDto {
                Id = campeonato.Id,
                Nome = campeonato.Nome,
                Descricao = campeonato.Descricao,
                DataInicio = campeonato.DataInicio,
                DataFim = campeonato.DataFim,
                Status = campeonato.Status,
            }).FirstOrDefaultAsync(c => c.Id == id);

        if (campeonato is null) return NotFound();

        return campeonato;
    }

    [Authorize(Policy = "Admin")]
    [HttpPost]
    public async Task<ActionResult<CampeonatoDto>> PostCampeonato(CampeonatoCreateDto campeonatoDto) {
        var campeonato = new Campeonato {
            Nome = campeonatoDto.Nome,
            Descricao = campeonatoDto.Descricao,
            DataInicio = campeonatoDto.DataInicio,
            DataFim = campeonatoDto.DataFim,
            Status = campeonatoDto.Status
        };
        
        context.Campeonatos.Add(campeonato);
        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCampeonato),
            new { id = campeonato.Id },
            new CampeonatoDto {
                Id = campeonato.Id,
                Nome = campeonato.Nome,
                Descricao = campeonato.Descricao,
                DataInicio = campeonato.DataInicio,
                DataFim = campeonato.DataFim,
                Status = campeonato.Status,
            }
        );
    }

    [Authorize(Policy = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCampeonato(long id, CampeonatoUpdateDto campeonatoDto) {
        var campeonatoExistente = await context.Campeonatos.FindAsync(id);
        if (campeonatoExistente is null) return NotFound();

        campeonatoExistente.Nome = campeonatoDto.Nome;
        campeonatoExistente.Descricao = campeonatoDto.Descricao;
        campeonatoExistente.DataInicio = campeonatoDto.DataInicio;
        campeonatoExistente.DataFim = campeonatoDto.DataFim;
        campeonatoExistente.Status = campeonatoDto.Status;

        await context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Policy = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCampeonato(long id) {
        var campeonatoExistente = await context.Campeonatos.FindAsync(id);
        if (campeonatoExistente is null) return NotFound();

        context.Campeonatos.Remove(campeonatoExistente);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
