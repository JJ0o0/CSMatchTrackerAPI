using CSMatchTracker.Models;

namespace CSMatchTracker.DTOs;

public class PartidaDto {
    public long Id { get; set; }
    public long? CampeonatoId { get; set; }

    public DateTime DataHora { get; set; }
    public string Mapa { get; set; } = string.Empty;

    public int PlacarTR { get; set; }
    public int PlacarCT { get; set; }
    public StatusPartida Status { get; set; }
}
