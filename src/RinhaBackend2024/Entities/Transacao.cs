namespace RinhaBackend2024.Entities;

public class Transacao
{
    protected Transacao()
    { }

    public Transacao(int valor, string tipo, string descricao)
    {
        Data = DateTime.UtcNow;
        Valor = valor;
        Tipo = tipo;
        Descricao = descricao;
    }

    public int Id { get; private set; }
    public int ClienteId { get; private set; }
    public DateTime Data { get; private set; }
    public int Valor { get; private set; }
    public string Tipo { get; private set; }
    public string Descricao { get; private set; }

    public Cliente Cliente { get; private set; }
}