using CSMatchTracker.Models;

namespace CSMatchTracker.DTOs;

public class PartidaCreateDto {
    public long? CampeonatoId { get; set; }
    public DateTime DataHora { get; set; }
    public string Mapa { get; set; } = string.Empty;
    public int PlacarTR { get; set; }
    public int PlacarCT { get; set; }
    public StatusPartida Status { get; set; }
}
