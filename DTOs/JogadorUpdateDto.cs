using CSMatchTracker.Models;

namespace CSMatchTracker.DTOs;

public class JogadorUpdateDto {
    public string Nome { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public FuncaoJogador Funcao { get; set; }
}
