namespace RinhaBackend2024.Entities;

public class Cliente
{
    protected Cliente()
    { }

    public Cliente(int id, int limite, int saldoInicial)
    {
        Id = id;
        Limite = limite;
        Saldo = saldoInicial;
    }

    public int Id { get; private set; }
    public int Limite { get; private set; }
    public int Saldo { get; set; }
}