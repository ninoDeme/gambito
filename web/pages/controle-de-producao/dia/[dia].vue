<script setup lang="ts">
import { UButton } from "#components";
import { parseDate } from "@internationalized/date";
import { LinhaProducaoDiaDtoGetDetailed } from "~/shared/models/linha-producao";
import { getCoreRowModel, useVueTable, FlexRender, type ColumnDef, type RowData } from "@tanstack/vue-table";
import * as z from "zod";

const localePath = useLocalePath();
const localeRoute = useLocaleRoute();

const route = localeRoute("controle-de-producao-dia-dia")!;
const diaAtual = parseDate(route.params.dia);

const { data, status, error, refresh, clear } = await useFetch(
  `/api/linha-producao/dia/${diaAtual}`,
  {
    transform: (val) => z.array(LinhaProducaoDiaDtoGetDetailed).parseAsync(val),
  },
);

const produtos = computed(() => {
  return data.value
    ?.map((d) => ({
      ...d,
      detalhes_pedido: [
        ...d.detalhes_pedido,
        ...d.detalhes_pedido,
        ...d.detalhes_pedido,
      ],
    }))
    ?.flatMap((d) =>
      d.detalhes_pedido.map((p, i) => ({
        ...p,
        ...d,
        rowspan: i === 0 ? d.detalhes_pedido.length : 0,
        detalhes_pedido: undefined,
      })),
    ) ?? [];
});

effect(() => console.log(produtos.value));

export type TableColumn<T extends RowData, D = unknown> = ColumnDef<T, D>;

const columns: TableColumn<(typeof produtos.value)[0], {enableRowspan: boolean}>[] = [
  {
    id: "actions",
    header: () => h("span", {}, "Ações"),
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
    accessorKey: "linha_producao",
    id: "id",
    meta: {
      // @ts-ignore
      enableRowspan: true,
    },
    header: "#",
    cell: ({ row }) => `${row.original.linha_producao}`,
  },
  {
    accessorKey: "nome_peca",
    id: "nome_peca",
    header: "Item",
    cell: ({ row }) => row.original.nome_peca,
  },
  // {
  //   accessorKey: "data",
  //   header: "Date",
  //   cell: ({ row }) => {
  //     return new Date(row.getValue("date")).toLocaleString("pt-BR", {
  //       dateStyle: "short",
  //     });
  //   },
  // },
  {
    accessorKey: "tempo_peca",
    header: "Tempo Peça",
    cell: ({ row }) => `${row.original.tempo_peca / 60} minutos`,
  },
  {
    accessorKey: "qtd_costureiros",
    header: "Quantidade Costureiros",
    cell: ({ row }) => `${row.getValue("qtd_costureiros")}`,
  },
];

const tableApi = useVueTable({
  columns,
  getCoreRowModel: getCoreRowModel(),
  data: produtos,
});
</script>
<template>
  <div class="flex-1 divide-y divide-accented w-full container mx-auto">
    <div class="rounded border-red-500" v-if="error">
      {{ error.status }} - {{ error.message }}
    </div>
    <div class="flex items-center gap-2 px-4 py-3.5 overflow-x-auto">
      <UInput
        :model-value="tableApi?.getColumn('id')?.getFilterValue() as string"
        class="max-w-sm min-w-[12ch]"
        placeholder="Filtrar..."
        @update:model-value="tableApi?.getColumn('id')?.setFilterValue($event)"
      />

      <UButton
        color="neutral"
        label="Buscar"
        @click="
          clear();
          refresh();
        "
      />

      <UDropdownMenu
        :items="
          tableApi
            ?.getAllColumns()
            .filter((column) => column.getCanHide())
            .map((column) => ({
              label: column.id,
              type: 'checkbox' as const,
              checked: column.getIsVisible(),
              onUpdateChecked(checked: boolean) {
                tableApi?.getColumn(column.id)?.toggleVisibility(!!checked);
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

    <UCarousel />
    <table class="w-full text-start overflow-hidden">
      <thead
        class="relative [&>tr]:after:absolute [&>tr]:after:inset-x-0 [&>tr]:after:bottom-0 [&>tr]:after:h-px [&>tr]:after:bg-(--ui-border-accented)"
        :class="{
          'after:animate-[carousel_2s_ease-in-out_infinite] after:bg-primary after:absolute after:bottom-0 after:inset-x-0 after:h-px':
            status == 'pending',
        }"
      >
        <tr
          v-for="headerGroup in tableApi.getHeaderGroups()"
          :key="headerGroup.id"
        >
          <th
            class="px-4 py-3.5 text-sm text-highlighted text-left rtl:text-right font-semibold [&:has([role=checkbox])]:pe-0"
            v-for="header in headerGroup.headers"
            :key="header.id"
            :colSpan="header.colSpan"
          >
            <FlexRender
              v-if="!header.isPlaceholder"
              :render="header.column.columnDef.header"
              :props="header.getContext()"
            />
          </th>
        </tr>
      </thead>
      <tbody
        class="divide-y divide-default [&>tr]:data-[selectable=true]:focus-visible:outline-primary"
      >
        <tr v-for="row in tableApi.getRowModel().rows" :key="row.id">
          <td
            v-for="cell in row.getVisibleCells()"
            :key="cell.id"
            :hidden="(cell.column.columnDef?.meta as any)?.['enableRowspan'] && row.original.rowspan === 0"
            :rowspan="(cell.column.columnDef?.meta as any)?.['enableRowspan'] ? row.original.rowspan : 1"
            class="p-2 text-sm whitespace-nowrap [&:has([role=checkbox])]:pe-0 border-default border-b"
          >
            <FlexRender
              :render="cell.column.columnDef.cell"
              :props="cell.getContext()"
            />
          </td>
        </tr>
      </tbody>
      <tfoot>
        <tr
          v-for="footerGroup in tableApi.getFooterGroups()"
          :key="footerGroup.id"
        >
          <th
            v-for="header in footerGroup.headers"
            :key="header.id"
            :colSpan="header.colSpan"
          >
            <FlexRender
              v-if="!header.isPlaceholder"
              :render="header.column.columnDef.footer"
              :props="header.getContext()"
            />
          </th>
        </tr>
      </tfoot>
    </table>
    <!--
    <UTable
      ref="table"
      :data="produtos"
      :columns="columns"
      :grouping-options="grouping_options"
      :grouping="grouping"
      sticky
      :expanded-options="exp_opts"
      class="h-96 text-start"
      :loading="status === 'pending'"
      ,
      :ui="{
        root: 'min-w-full',
        td: 'empty:p-0', // helps with the colspaned row added for expand slot
      }"
    >
      <template #actions-cell="{ row }">
        <div v-if="row.getIsGrouped()" class="flex items-center">
          <span
            class="inline-block"
            :style="{ width: `calc(${row.depth} * 1rem)` }"
          />

          <UButton
            variant="outline"
            color="neutral"
            class="mr-2"
            size="xs"
            :icon="row.getIsExpanded() ? 'i-lucide-minus' : 'i-lucide-plus'"
            @click="row.toggleExpanded()"
          />
        </div>
      </template>
    </UTable>
 -->
  </div>
</template>
