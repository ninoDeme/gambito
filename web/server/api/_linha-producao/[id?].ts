// import { serverSupabaseClient } from "#supabase/server";
// import { linhaProducaoDtoBase } from "~/shared/models/linha-producao";
// import { Database } from "~/types/database.types";
//
// export default defineEventHandler(async (event) => {
//   const linhaProducao = getRouterParam(event, "id");
//   const client = await serverSupabaseClient<Database>(event);
//   if (linhaProducao) {
//     if (event.method === `GET`) {
//       return await client
//         .from("linha_producao")
//         .select("*")
//         .match({ id: linhaProducao });
//     }
//   } else {
//     if (event.method === `GET`) {
//       return await client.from("linha_producao").select("*");
//     } else if (event.method === "POST") {
//       let body = await readValidatedBody(event, linhaProducaoDtoBase.parse);
//     }
//   }
//
//   return createError({
//     status: 404,
//   });
// });
