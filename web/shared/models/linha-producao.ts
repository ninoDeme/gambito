import z from 'zod';

export const LinhaProducaoDiaProdutoConfDto = z.object(
  {
    qtd_costureiros: z.number(), // Total de costureiros unicos no dia
    nome_peca: z.string(),
    tempo_peca: z.number(),
    // meta por funcionario = 60/tempo
    meta_dia_peca: z.number(), // Soma de todas as metas do dia
    qtd_peca: z.number(),
    retrabalho_dia_peca: z.number()
  }
);

export type LinhaProducaoDiaProdutoConfDto = z.infer<typeof LinhaProducaoDiaProdutoConfDto>


export const LinhaProducaoDiaDtoGetDetailed = z.object(
  {
    id: z.number(),
    linha_producao: z.number(),
    data: z.date({ coerce: true }).nullable(),
    invativo: z.boolean(),
    detalhes_pedido: z.array(LinhaProducaoDiaProdutoConfDto)
  }
)

export type LinhaProducaoDiaDtoGetDetailed = z.infer<typeof LinhaProducaoDiaDtoGetDetailed>


//
// export const linhaProducaoDiaDtoBase = z.object({
//   id: z.number().optional(),
//   linhaProducao: z.number(),
//   data: z.date().optional(),
//   inativo: z.boolean()
// })
//
// export const linhaProducaoDtoBase = z.object({
//   id: z.number().min(0).int().optional(),
//   descricao: z.string().max(100),
// })
//
// export const linhaProducaoDtoGet = linhaProducaoDtoBase.extend({
//   dias: z.array(linhaProducaoDiaDtoBase)
// })
//
// export const linhaProducaoDtoPost = linhaProducaoDtoBase.extend({
// })
//
// export const tipoHora = z.enum(["HORA_EXTRA", "BANCO_HORAS"]);
//
// export const linhaProducaoDiaHoraDtoBase = z.object({
//   id: z.number().optional(),
//   linha_producao: z.number().optional(),
//   data: z.date(),
//   hora: z.date(),
//   pedido: z.number(),
//   qtd_produzido: z.number().optional(),
//   paralizacao: z.boolean().default(false),
//   // hora_ini TIME,
//   // hora_fim TIME,
//   tipo: tipoHora.optional(),
// })
//
// export const pedido = z.object({
//   descricao: z.string().max(256),
//   produto: z.number()
// })
//
// export const linhaProducaoDiaHoraDtoCreate = linhaProducaoDiaHoraDtoBase.extend({
//   pedido: z.union([z.number(), pedido])
// })
