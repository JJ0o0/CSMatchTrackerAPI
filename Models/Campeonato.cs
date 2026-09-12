namespace CSMatchTracker.Models;

public enum StatusCampeonato {
    InscricoesAbertas,
    EmAndamento,
    Finalizado,
    Cancelado
}

public class Campeonato {
    public long Id { get; set; }

    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }

    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }

    public ICollection<Partida> Partidas { get; set; } = [];

    public StatusCampeonato Status { get; set; }
}
