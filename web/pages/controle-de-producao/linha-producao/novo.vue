<script setup lang="ts">
import * as z from "zod";
import type { FormSubmitEvent } from "@nuxt/ui";
import { LinhaProducaoDiaHoraDtoBase } from "~/shared/models/linha-producao";

const schema = z.object({
  descricao: z.string(),
  diaPadrao: z.object({
    data: z.date().optional(),
    inativo: z.boolean(),
    horas: z.array(LinhaProducaoDiaHoraDtoBase),
  }),
});

type Schema = z.infer<typeof schema>;

const state = reactive<Schema>({
  descricao: "",
  diaPadrao: {
    data: new Date(),
    inativo: false,
    horas: [],
  },
});

const toast = useToast();
async function onSubmit(event: FormSubmitEvent<Schema>) {
  toast.add({
    title: "Success",
    description: "The form has been submitted.",
    color: "success",
  });
  console.log(event.data);
}
</script>

<template>
  <div class="container mx-auto py-4">
    <h1>Nova linha de produção</h1>
    <UForm
      :schema="schema"
      :state="state"
      class="space-y-4 w-max mx-auto"
      @submit="onSubmit"
    >
      <UFormField label="Descricao" name="email">
        <UInput v-model="state.descricao" />
      </UFormField>

      <UCard>
        <h1>Configuração padrão</h1>
        <UFormField label="Descricao" name="email">
          <UInput v-model="state.descricao" />
        </UFormField>
        <UFormField label="Descricao" name="email">
          <UInput v-model="state.descricao" />
        </UFormField>
        <UFormField label="Descricao" name="email">
          <UInput v-model="state.descricao" />
        </UFormField>
      </UCard>

      <UButton type="submit"> Criar </UButton>
    </UForm>
  </div>
</template>
