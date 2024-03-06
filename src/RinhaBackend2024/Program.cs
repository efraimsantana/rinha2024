using Microsoft.EntityFrameworkCore;
using RinhaBackend2024.Commons;
using RinhaBackend2024.Context;
using RinhaBackend2024.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

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
    
    var tipo = inputModel.Tipo.ToLowerInvariant();
    
    var ehTipoValido = tipo == Contantes.Credito || tipo == Contantes.Debito;

    if (!ehTipoValido)
    {
        return Results.UnprocessableEntity();
    }

    if (string.IsNullOrEmpty(inputModel.Descricao) || inputModel.Descricao.Trim().Length > 10)
    {
        return Results.UnprocessableEntity();
    }

    var ehTransacaoValida = cliente.AdicionarTransacao(inputModel.Valor, inputModel.Tipo, inputModel.Descricao);
    
    if (!ehTransacaoValida)
    {
        return Results.UnprocessableEntity();
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


app.MapGet("/clientes/{id:int}/extrato/", async (int id, AppDbContext db) =>
{
    var cliente = await db.Clientes.Where(x => x.Id == id)
        .Select(x => new
        {
            Saldo = new
            {
                Total = x.Saldo,
                Data_extrato = DateTime.UtcNow,
                Limite = x.Limite
            },
            Ultimas_transacoes = x.Transacoes
                .OrderByDescending(t => t.Data)
                .Take(10)
                .Select(t => new
                {
                    t.Valor,
                    t.Tipo,
                    t.Descricao,
                    Realizada_em = t.Data
                })
        })
        .FirstOrDefaultAsync();

    return cliente is null ? Results.NotFound() : Results.Ok(cliente);
});


app.Run();