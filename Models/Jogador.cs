namespace CSMatchTracker.Models;

public enum FuncaoJogador {
    Rifler,
    AWPer,
    EntryFragger,
    Support,
    IGL,
    Lurker
};

public class Jogador {
    public long Id { get; set; }

    public string Nome { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public FuncaoJogador Funcao { get; set; }

    public DateTime DataCadastro { get; set; }
}
