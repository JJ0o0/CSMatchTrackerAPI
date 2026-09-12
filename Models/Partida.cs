namespace CSMatchTracker.Models;

public enum StatusPartida {
    Agendada,
    EmAndamento,
    Finalizada,
    Cancelada
}

public class Partida {
    public long Id { get; set; }
    public long? CampeonatoId { get; set; }

    public DateTime DataHora { get; set; }
    public string Mapa { get; set; } = string.Empty;

    public int PlacarTR { get; set; }
    public int PlacarCT { get; set; }
    public StatusPartida Status { get; set; }

    public Campeonato? Campeonato { get; set; }
}
