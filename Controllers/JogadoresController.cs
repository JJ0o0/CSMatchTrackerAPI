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
public class JogadoresController(AppDbContext context) : ControllerBase {
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JogadorDto>>> GetJogadores() {
        return await context.Jogadores
            .Select(jogador => new JogadorDto {
                Id = jogador.Id,
                Nome = jogador.Nome,
                Nickname = jogador.Nickname,
                Funcao = jogador.Funcao,
                DataCadastro = jogador.DataCadastro
            }).ToListAsync();
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<JogadorDto>> GetJogador(long id) {
        var jogador = await context.Jogadores
            .Select(j => new JogadorDto {
                Id = j.Id,
                Nome = j.Nome,
                Nickname = j.Nickname,
                Funcao = j.Funcao,
                DataCadastro = j.DataCadastro
            }).FirstOrDefaultAsync(j => j.Id == id);

        if (jogador is null) return NotFound();

        return jogador;
    }

    [Authorize]
    [HttpGet("top")]
    public async Task<ActionResult> GetTopJogadores() {
        var ranking = context.ParticipacaoPartidas
            .GroupBy(p => p.JogadorId)
            .Select(grupo => new JogadorRankingDto{
                JogadorId = grupo.Key,
                Nickname = grupo.First().Jogador.Nickname,
                Funcao = grupo.First().Jogador.Funcao,
                Partidas = grupo.Count(),
                Kills = grupo.Sum(p => p.Kills),
                Deaths = grupo.Sum(p => p.Deaths),
                Assists = grupo.Sum(p => p.Assists),
                Headshots = grupo.Sum(p => p.Headshots),
                Mvps = grupo.Sum(p => p.Mvp ? 1 : 0),
                Kd = grupo.Sum(p => p.Deaths) == 0
                     ? grupo.Sum(p => p.Kills)
                     : (double)grupo.Sum(p => p.Kills) / grupo.Sum(p => p.Deaths)
            })
            .OrderByDescending(j => j.Kd);

        return Ok(ranking);
    }

    [Authorize(Policy = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Jogador>> PostJogador(JogadorCreateDto jogadorDto) {
        var jogador = new Jogador {
            Nome = jogadorDto.Nome,
            Nickname = jogadorDto.Nickname,
            Funcao = jogadorDto.Funcao
        };

        context.Jogadores.Add(jogador);
        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetJogador),
            new { id = jogador.Id },
            jogador
        );
    }

    [Authorize(Policy = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutJogador(long id, JogadorUpdateDto jogadorDto) {
        var jogadorExistente = await context.Jogadores.FindAsync(id);
        if (jogadorExistente is null) return NotFound();

        jogadorExistente.Nome = jogadorDto.Nome;
        jogadorExistente.Nickname = jogadorDto.Nickname;
        jogadorExistente.Funcao = jogadorDto.Funcao;

        await context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Policy = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJogador(long id) {
        var jogadorExistente = await context.Jogadores.FindAsync(id);
        if (jogadorExistente is null) return NotFound();

        context.Jogadores.Remove(jogadorExistente);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
