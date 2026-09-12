using CSMatchTracker.Models;

namespace CSMatchTracker.DTOs;

public class JogadorRankingDto {
    public long JogadorId { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public FuncaoJogador Funcao { get; set; }
    public int Partidas { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Assists { get; set; }
    public int Headshots { get; set; }
    public int Mvps { get; set; }
    public double Kd { get; set; }
}
