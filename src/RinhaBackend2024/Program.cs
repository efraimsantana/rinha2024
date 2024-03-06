using Microsoft.EntityFrameworkCore;
using RinhaBackend2024.Context;
using RinhaBackend2024.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

const string credito = "c";
const string debito = "d";

app.MapPost("/clientes/{id:int}/transacoes/", async (int id, TransacaoInputModel inputModel, AppDbContext db) =>
{
    var cliente = await db.Clientes.FirstOrDefaultAsync(x => x.Id == id);

    if (cliente is null)
    {
        return Results.NotFound();
    }

    if (inputModel.Valor <= 0)
    {
        return Results.UnprocessableEntity();
    }

    if (!inputModel.Tipo.Equals(credito, StringComparison.InvariantCultureIgnoreCase) 
        && !inputModel.Tipo.Equals(debito, StringComparison.InvariantCultureIgnoreCase))
    {
        return Results.UnprocessableEntity();
    }

    if (string.IsNullOrEmpty(inputModel.Descricao) || inputModel.Descricao.Trim().Length > 10)
    {
        return Results.UnprocessableEntity();
    }

    if (inputModel.Tipo.Equals(debito, StringComparison.InvariantCultureIgnoreCase))
    {
        var saldoRestante = cliente.Saldo - inputModel.Valor;

        if (int.IsNegative(saldoRestante) && Math.Abs(saldoRestante) > cliente.Limite)
        {
            return Results.UnprocessableEntity();
        }

        cliente.Saldo = saldoRestante;
    }
    else
    {
        cliente.Saldo += inputModel.Valor;
    }

    try
    {
        await db.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException ex)
    {
        return Results.UnprocessableEntity(); 
    }

    return Results.Ok(new
    {
        cliente.Limite,
        cliente.Saldo
    });
});
    
    
app.Run();