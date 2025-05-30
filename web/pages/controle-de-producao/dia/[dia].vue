<script setup lang="ts">
import { UButton } from "#components";
import { parseDate } from "@internationalized/date";
import type { TableColumn } from "@nuxt/ui";
import {
  linhaProducaoDiaDtoBase,
  linhaProducaoDtoGet,
} from "~/shared/models/linha-producao";
import * as z from "zod";

const localePath = useLocalePath();
const localeRoute = useLocaleRoute();

const route = localeRoute("controle-de-producao-dia-dia")!;
const diaAtual = parseDate(route.params.dia);
const { data, status, error, refresh, clear } = await useFetch(
  `/api/linha-producao/${diaAtual}`,
  {
    transform: (val) => z.array(linhaProducaoDtoGet).parseAsync(val),
  },
);

const dias = computed(() => data.value?.flatMap((d) => d.dias));

const columns: TableColumn<typeof linhaProducaoDiaDtoBase._type>[] = [
  {
    id: "actions",
    header: () => h("span", { class: "w-max" }, "Ações"),
    enableHiding: false,
    enableSorting: false,
    cell: ({ row }) =>
      h(UButton, {
        to: localePath({
          name: "controle-de-producao-linha-producao",
          params: { codigo: row.getValue("id") },
        }),
        label: "Editar",
      }),
  },
  {
    accessorKey: "id",
    id: "id",
    header: "#",
    cell: ({ row }) => `#${row.getValue("id")}`,
  },
  {
    accessorKey: "data",
    header: "Date",
    cell: ({ row }) => {
      return new Date(row.getValue("date")).toLocaleString("pt-BR", {
        dateStyle: "short",
      });
    },
  },
  // {
  //   accessorKey: "produto",
  //   header: "Produto",
  //   cell: ({ row }) => row.getValue("produto"),
  // },
];

const table = useTemplateRef("table");
</script>
<template>
  <div class="flex-1 divide-y divide-accented w-full">
    <div class="flex items-center gap-2 px-4 py-3.5 overflow-x-auto">
      <UInput
        :model-value="
          table?.tableApi?.getColumn('id')?.getFilterValue() as string
        "
        class="max-w-sm min-w-[12ch]"
        placeholder="Filtrar..."
        @update:model-value="
          table?.tableApi?.getColumn('id')?.setFilterValue($event)
        "
      />

      <UButton color="neutral" label="Buscar" @click="refresh()" />

      <UDropdownMenu
        :items="
          table?.tableApi
            ?.getAllColumns()
            .filter((column) => column.getCanHide())
            .map((column) => ({
              label: column.id,
              type: 'checkbox' as const,
              checked: column.getIsVisible(),
              onUpdateChecked(checked: boolean) {
                table?.tableApi
                  ?.getColumn(column.id)
                  ?.toggleVisibility(!!checked);
              },
              onSelect(e?: Event) {
                e?.preventDefault();
              },
            }))
        "
        :content="{ align: 'end' }"
      >
        <UButton
          label="Colunas"
          color="neutral"
          variant="outline"
          trailing-icon="i-lucide-chevron-down"
          class="ml-auto"
          aria-label="Columns select dropdown"
        />
      </UDropdownMenu>
    </div>

    <UTable
      ref="table"
      :data="dias"
      :columns="columns"
      sticky
      class="h-96 text-start"
      :loading="status === 'pending'"
    >
    </UTable>
  </div>
</template>
