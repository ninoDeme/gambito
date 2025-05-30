import z from 'zod';

export const linhaProducaoDiaDtoBase = z.object({
  id: z.number().optional(),
  linhaProducao: z.number(),
  data: z.date().optional(),
  inativo: z.boolean()
})

export const linhaProducaoDtoBase = z.object({
  id: z.number().min(0).int().optional(),
  descricao: z.string().max(100),
})

export const linhaProducaoDtoGet = linhaProducaoDtoBase.extend({
  dias: z.array(linhaProducaoDiaDtoBase)
})

export const linhaProducaoDtoPost = linhaProducaoDtoBase.extend({
})

export const tipoHora = z.enum(["HORA_EXTRA", "BANCO_HORAS"]);

export const linhaProducaoDiaHoraDtoBase = z.object({
  id: z.number().optional(),
  linha_producao: z.number().optional(),
  data: z.date(),
  hora: z.date(),
  pedido: z.number(),
  qtd_produzido: z.number().optional(),
  paralizacao: z.boolean().default(false),
  // hora_ini TIME,
  // hora_fim TIME,
  tipo: tipoHora.optional(),
})

export const pedido = z.object({
  descricao: z.string().max(256),
  produto: z.number()
})

export const linhaProducaoDiaHoraDtoCreate = linhaProducaoDiaHoraDtoBase.extend({
  pedido: z.union([z.number(), pedido])
})
