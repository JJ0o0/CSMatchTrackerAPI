using CSMatchTracker.Models;

namespace CSMatchTracker.DTOs;

public class JogadorDto {
    public long Id { get; set; }

    public string Nome { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public FuncaoJogador Funcao { get; set; }

    public DateTime DataCadastro { get; set; }
}
