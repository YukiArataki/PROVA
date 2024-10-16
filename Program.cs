using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

app.MapPost("/api/funcionario/cadastrar", async (Funcionario funcionario, AppDbContext db) =>
{
    db.Funcionarios.Add(funcionario);
    await db.SaveChangesAsync();
    return Results.Created($"/api/funcionario/{funcionario.Id}", funcionario);
});

app.MapGet("/api/funcionario/listar", async (AppDbContext db) =>
{
    return await db.Funcionarios.ToListAsync();
});

app.MapPost("/api/folha/cadastrar", async (FolhaPagamento folha, AppDbContext db) =>
{
    var funcionario = await db.Funcionarios.FindAsync(folha.FuncionarioId);
    if (funcionario == null) return Results.NotFound();

    folha.Valor = funcionario.Salario; 

    db.FolhasPagamento.Add(folha);
    await db.SaveChangesAsync();
    return Results.Created($"/api/folha/{folha.Id}", folha);
});

app.MapGet("/api/folha/listar", async (AppDbContext db) =>
{
    return await db.FolhasPagamento.Include(f => f.Funcionario).ToListAsync();
});

app.MapGet("/api/folha/buscar/{cpf}/{mes}/{ano}", async (string cpf, int mes, int ano, AppDbContext db) =>
{
    var folha = await db.FolhasPagamento
        .Include(f => f.Funcionario)
        .FirstOrDefaultAsync(f => f.Funcionario.CPF == cpf && f.Mes == mes && f.Ano == ano);
    
    return folha != null ? Results.Ok(folha) : Results.NotFound();
});

app.Run();
