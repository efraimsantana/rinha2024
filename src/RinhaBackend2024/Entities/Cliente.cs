using RinhaBackend2024.Commons;

namespace RinhaBackend2024.Entities;

public class Cliente
{
    protected Cliente()
    {
        Transacoes = new List<Transacao>();
    }

    public Cliente(int id, int limite, int saldoInicial) : this()
    {
        Id = id;
        Limite = limite;
        Saldo = saldoInicial;
    }

    public int Id { get; private set; }
    public int Limite { get; private set; }
    public int Saldo { get; private set; }
    
    public ICollection<Transacao> Transacoes { get; private set; }

    public bool AdicionarTransacao(int valor, string tipo, string descricao)
    {
        if (tipo.Equals(Contantes.Debito, StringComparison.InvariantCultureIgnoreCase))
        {
            var saldoRestante = Saldo - valor;

            if (int.IsNegative(saldoRestante) && Math.Abs(saldoRestante) > Limite)
            {
                return false;
            }

            Saldo = saldoRestante;
        }
        else
        {
            Saldo += valor;
        }
        
        var transacao = new Transacao(valor, tipo, descricao);
        Transacoes.Add(transacao);

        return true;
    }
}