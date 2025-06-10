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

    group.MapGet(
      "/",
      async Task<Results<Ok<LinhaProducaoDtoGetSummary[]>, ProblemHttpResult>> (
        GambitoContext db
      ) =>
      {
        // LocalDate? parsed_data = null;
        // if (Data is not null and not "null")
        // {
        //   var parse_result = NodaTime.Text.LocalDatePattern.Iso.Parse(Data);
        //   if (!parse_result.Success)
        //   {
        //     return TypedResults.Problem("Invalid Date");
        //   }
        //   parsed_data = parse_result.Value;
        // }
        var builder = db.LinhaProducaos;
        // .Where(l => Data == null || l.LinhaProducaoDia.Count > 0);
        var res = await builder.ToListAsync();
        // if (Data is not null)
        // {
        //   res = [.. res.Where(l => l.LinhaProducaoDia.Count > 0)];
        // }
        return TypedResults.Ok(
          res.Select(l => new LinhaProducaoDtoGetSummary(
              l.Id,
              l.Descricao
            // l.LinhaProducaoDia.Select(d => new LinhaProducaoDiaDtoGet(d)).ToArray()
            ))
            .ToArray()
        );
      }
    );

    group.MapGet(
      "/{linha_producao}",
      async Task<Results<Ok<LinhaProducaoDtoGet>, ProblemHttpResult, NotFound>> (
        GambitoContext db,
        int linha_producao,
        [FromQuery(Name = "data")] string? Data
      ) =>
      {
        LocalDate? parsed_data = null;
        if (Data is not null)
        {
          var parse_result = NodaTime.Text.LocalDatePattern.Iso.Parse(Data);
          if (!parse_result.Success)
          {
            return TypedResults.Problem("Invalid Date");
          }
          parsed_data = parse_result.Value;
        }
        var res = await db
          .LinhaProducaos.Include(l =>
            l.LinhaProducaoDia.Where(d => parsed_data == null || d.Data == parsed_data)
          )
          .Where(l => l.Id == linha_producao)
          .SingleOrDefaultAsync(l => l.Id == linha_producao);
        return res is null
          ? TypedResults.NotFound()
          : TypedResults.Ok(
            new LinhaProducaoDtoGet(
              res.Id,
              res.Descricao,
              res.LinhaProducaoDia.Select(d => new LinhaProducaoDiaDtoGet(d)).ToArray()
            )
          );
      }
    )
      // .Produces(StatusCodes.Status200OK).ProducesProblem(StatusCodes.Status404NotFound)
    ;

    group.MapPost(
      "/",
      async Task<Results<Created<LinhaProducaoDtoGetSummary>, NotFound>> (
        [FromBody] LinhaProducaoDtoCreateUpdate body,
        GambitoContext db
      ) =>
      {
        var res = await db.LinhaProducaos.AddAsync(body.ToEntity(null));
        await db.SaveChangesAsync();
        return TypedResults.Created(
          $"api/linha-producao/{res.Entity.Id}",
          new LinhaProducaoDtoGetSummary(res.Entity.Id, res.Entity.Descricao)
        );
      }
    );

    group.MapPut(
      "/{linha_producao}",
      async Task<Results<Created<LinhaProducaoDtoGetSummary>, NotFound>> (
        int linha_producao,
        [FromBody] LinhaProducaoDtoCreateUpdate body,
        GambitoContext db
      ) =>
      {
        var res = await db.LinhaProducaos.FindAsync(linha_producao);
        if (res is null)
        {
          return TypedResults.NotFound();
        }
        db.LinhaProducaos.Update(body.ToEntity(linha_producao));
        await db.SaveChangesAsync();
        return TypedResults.Created(
          $"api/linha-producao/{res.Id}",
          new LinhaProducaoDtoGetSummary(res.Id, res.Descricao)
        );
      }
    );
    var dias = group.MapGroup("/");

    dias.MapGet(
      "/dia/{data}",
      async Task<Results<Ok<LinhaProducaoDiaDtoGetDetailed[]>, ProblemHttpResult>> (
        string data,
        GambitoContext db
      ) =>
      {
        LocalDate? parsed_data = null;
        if (data is not "null")
        {
          var parse_result = NodaTime.Text.LocalDatePattern.Iso.Parse(data);
          if (!parse_result.Success)
          {
            return TypedResults.Problem("Invalid Date");
          }
          parsed_data = parse_result.Value;
        }

        var res = await db
          .LinhaProducaoDia.Where(d => d.Data == parsed_data)
          .Select(l => new LinhaProducaoDiaDtoGetDetailed(
            l.Id,
            l.LinhaProducao,
            l.Data,
            l.Invativo,
            db.ProdutoConfigs.Where(pc =>
                pc.LinhaProducaoHoras.Any(h => h.LinhaProducaoDia == l.Id)
              )
              .GroupJoin(
                db.LinhaProducaoHoras,
                pc => pc.Id,
                h => h.ProdutoConfig,
                (pc, h) =>
                  new LinhaProducaoDiaProdutoConfDto(
                    h.SelectMany(he => he.LinhaProducaoHoraEtapas)
                      .SelectMany(e => e.Funcionarios)
                      .Distinct()
                      .Count(),
                    pc.ProdutoNavigation.Nome,
                    pc.ProdutoConfigEtapas.Sum(e => e.Segundos),
                    h.Sum(hp =>
                      hp.LinhaProducaoHoraEtapas.Sum(
                        int (hpe) =>
                          hpe.Funcionarios.Count * pc.ProdutoConfigEtapas.Sum(int (e) => e.Segundos)
                      )
                    ),
                    h.Sum(hp => hp.QtdProduzido ?? 0),
                    h.Sum(hp =>
                      hp.LinhaProducaoHoraDefeitos.Where(hd => hd.Retrabalhado)
                        .Sum(int (hd) => hd.QtdPecas)
                    )
                  )
              )
              .ToArray()
          ))
          .ToListAsync();
        return TypedResults.Ok(res);
      }
    );
  }
}

public record LinhaProducaoDtoGetSummary(int Id, string? Descricao);

public record LinhaProducaoDtoGet(int Id, string? Descricao, LinhaProducaoDiaDtoGet[] Dias);

public record LinhaProducaoDtoCreateUpdate(string? Descricao)
{
  public LinhaProducao ToEntity(int? Id) => new() { Id = Id ?? 0, Descricao = Descricao };
}

public record LinhaProducaoDiaProdutoConfDto(
  int QtdCostureiros, // Total de costureiros unicos no dia
  string NomePeca,
  int TempoPeca,
  // meta por funcionario = 60/tempo
  int MetaDiaPeca, // Soma de todas as metas do dia
  int QtdPeca,
  int RetrabalhoDiaPeca
);

public record LinhaProducaoDiaDtoGetDetailed(
  int Id,
  int LinhaProducao,
  LocalDate? Data,
  bool Invativo,
  LinhaProducaoDiaProdutoConfDto[] DetalhesPedido
)
{
  public int Id { get; set; } = Id;
  public int LinhaProducao { get; set; } = LinhaProducao;
  public LocalDate? Data { get; set; } = Data;
  public bool Invativo { get; set; } = Invativo;
};

public class LinhaProducaoDiaDtoGet(int Id, int LinhaProducao, LocalDate? Data, bool Invativo)
{
  public LinhaProducaoDiaDtoGet(LinhaProducaoDia entity)
    : this(entity.Id, entity.LinhaProducao, entity.Data, entity.Invativo) { }

  public int Id { get; set; } = Id;
  public int LinhaProducao { get; set; } = LinhaProducao;
  public LocalDate? Data { get; set; } = Data;
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
