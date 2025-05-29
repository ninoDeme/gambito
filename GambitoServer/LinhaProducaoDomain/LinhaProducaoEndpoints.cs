using GambitoServer.Db;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace GambitoServer.LinhaProducaoDomain;

public static class LinhaProducaoEndpoints
{
  public static void MapLinhaProducaoEndpoints(this IEndpointRouteBuilder router)
  {
    var group = router.MapGroup("api/linha-producao").WithTags("Linhas de Produção");

    group.MapGet("/", async Task<Ok<LinhaProducaoDtoGet[]>> (GambitoContext db) =>
    {
      var res = await db.LinhaProducaos
        .Select(l => new LinhaProducaoDtoGet(l.Id, l.Descricao))
        .ToArrayAsync();
      return TypedResults.Ok(res);
    });

    group.MapGet("/{linha_producao}", async Task<Results<Ok<LinhaProducaoDtoGet>, NotFound>> (int linha_producao, GambitoContext db) =>
    {
      var res = await db.LinhaProducaos.FindAsync(linha_producao);
      return res is null
        ? TypedResults.NotFound()
        : TypedResults.Ok(new LinhaProducaoDtoGet(res.Id, res.Descricao));
    })
    // .Produces(StatusCodes.Status200OK).ProducesProblem(StatusCodes.Status404NotFound)
    ;

    group.MapPost("/", async Task<Results<Created<LinhaProducaoDtoGet>, NotFound>> ([FromBody] LinhaProducaoDtoCreateUpdate body, GambitoContext db) =>
    {
      var res = await db.LinhaProducaos.AddAsync(body.ToEntity(null));
      await db.SaveChangesAsync();
      return TypedResults.Created($"api/linha-producao/{res.Entity.Id}", new LinhaProducaoDtoGet(res.Entity.Id, res.Entity.Descricao));
    });

    group.MapPut("/{linha_producao}", async Task<Results<Created<LinhaProducaoDtoGet>, NotFound>> (int linha_producao, [FromBody] LinhaProducaoDtoCreateUpdate body, GambitoContext db) =>
    {
      var res = await db.LinhaProducaos.FindAsync(linha_producao);
      if (res is null)
      {
        return TypedResults.NotFound();
      }
      db.LinhaProducaos.Update(body.ToEntity(linha_producao));
      await db.SaveChangesAsync();
      return TypedResults.Created($"api/linha-producao/{res.Id}", new LinhaProducaoDtoGet(res.Id, res.Descricao));
    });
    var dias = group.MapGroup("/{linha_producao}/dia");

    dias.MapGet("/", async Task<Results<Ok<LinhaProducaoDiaDtoGet[]>, NotFound>> (int linha_producao, GambitoContext db) =>
        {
          var res = await db.LinhaProducaoDia
            .Where(l => l.LinhaProducao == linha_producao)
            .Select(l => new LinhaProducaoDiaDtoGet(l.Id, l.LinhaProducao, l.Data, l.Invativo))
            .ToArrayAsync();
          return TypedResults.Ok(res);
        });
  }

}


public record LinhaProducaoDtoGet(int Id, string? Descricao);
public record LinhaProducaoDtoCreateUpdate(string? Descricao)
{
  public LinhaProducao ToEntity(int? Id) => new()
  {
    Id = Id ?? 0,
    Descricao = Descricao
  };
}

public class LinhaProducaoDiaDtoGet(int Id, int LinhaProducao, LocalDate Data, bool Invativo)
{
    public int Id { get; set; } = Id;
    public int LinhaProducao { get; set; } = LinhaProducao;
    public string Data { get; set; } = Data.ToString();
    public bool Invativo { get; set; } = Invativo;
};
// public record LinhaProducaoDtoCreateUpdate(string? Descricao)
// {
//   public LinhaProducao ToEntity(int? Id) => new()
//   {
//     Id = Id ?? 0,
//     Descricao = Descricao
//   };
// }
