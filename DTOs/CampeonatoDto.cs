using CSMatchTracker.Models;

namespace CSMatchTracker.DTOs;

public class CampeonatoDto {
    public long Id { get; set; }

    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }

    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }

    public StatusCampeonato Status { get; set; }
}
