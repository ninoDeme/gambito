// using GambitoServer.Db;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using NodaTime;
//
// namespace GambitoServer.LinhaProducao;
//
// public static class LinhaProducaoEndpoints
// {
//   public static void MapLinhaProducaoEndpoints(this IEndpointRouteBuilder router)
//   {
//     var group = router.MapGroup("api/linha-producao").WithTags("Linhas de Produção");
//
//     group.MapGet("/", async (GambitoContext db) =>
//     {
//       var res = await db.LinhaProducaos
//         .Select(l => new LinhaProducaoModel(l))
//         .ToListAsync();
//       return res;
//     });
//
//     group.MapGet("/{id}", async (int id, GambitoContext db) =>
//     {
//       var res = await db.LinhaProducaos.FindAsync(id);
//       return res is null
//         ? (IResult)TypedResults.NotFound()
//         : TypedResults.Ok(new LinhaProducaoModel(res));
//     });
//
//     group.MapPost("/", async ([FromBody] LinhaProducaoModel body, GambitoContext db) =>
//     {
//
//       var res1 = await db.LinhaProducaoHoras.Where(l => l.LinhaProducaoDium.Data == null).ToListAsync();
//
//       var res = await db.LinhaProducaos.AddAsync(new LinhaProducao());
//       await db.SaveChangesAsync();
//       return TypedResults.Created($"api/linha-producao/{res.Entity.Id}", new LinhaProducaoModel(res.Entity));
//     });
//   }
// }
