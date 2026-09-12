using CSMatchTracker.Models;

namespace CSMatchTracker.DTOs;

public class ParticipacaoPartidaDto {
    public long Id { get; set; }
    public long JogadorId { get; set; }
    public long PartidaId { get; set; }

    public LadoPartida Lado { get; set; }

    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Assists { get; set; }
    public int Headshots { get; set; }

    public bool Mvp { get; set; }
}
