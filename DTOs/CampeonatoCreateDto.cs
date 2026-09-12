using CSMatchTracker.Models;

namespace CSMatchTracker.DTOs;

public class CampeonatoCreateDto {
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; } = string.Empty;

    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }

    public StatusCampeonato Status { get; set; }
}
