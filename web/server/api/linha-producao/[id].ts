import { serverSupabaseClient, serverSupabaseServiceRole, serverSupabaseUser } from '#supabase/server';
import { Database } from '~/types/database.types';

export default defineEventHandler(async (event) => {
  const linhaProducao = getRouterParam(event, "id");
  if (event.method === `GET`) {
    const client = await serverSupabaseClient<Database>(event);
    // const client = serverSupabaseServiceRole(event)
    let ls = await client.from('linha_producao').select('*');
    return ls;
  }
});
