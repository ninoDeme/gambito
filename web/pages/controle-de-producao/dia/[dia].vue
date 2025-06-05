<script setup lang="ts">
import { UBadge, UButton } from "#components";
import {
  DateFormatter,
  parseDate,
  getLocalTimeZone,
} from "@internationalized/date";
import { LinhaProducaoDiaDtoGetDetailed } from "~/shared/models/linha-producao";
import {
  getCoreRowModel,
  useVueTable,
  FlexRender,
  type ColumnDef,
  type RowData,
} from "@tanstack/vue-table";
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

const locale = useLocale();

const formatedDate = computed(() =>
  new DateFormatter(locale.code.value, { dateStyle: "short" }).format(
    diaAtual.toDate(getLocalTimeZone()),
  ),
);

const produtos = computed(() => {
  return (
    data.value?.flatMap((d) =>
      d.detalhes_pedido.map((p, i) => ({
        ...p,
        ...d,
        rowspan: i === 0 ? d.detalhes_pedido.length : 0,
        last: i === d.detalhes_pedido.length - 1,
        detalhes_pedido: undefined,
      })),
    ) ?? []
  );
});

effect(() => console.log(produtos.value));

export type TableColumn<T extends RowData, D = unknown> = ColumnDef<T, D>;

declare module "@tanstack/table-core" {
  interface ColumnMeta<TData extends RowData, TValue> {
    enableRowspan: boolean;
  }
}

const columns: TableColumn<(typeof produtos.value)[0]>[] = [
  {
    id: "actions",
    header: () => h("span", {}, "Ações"),
    enableHiding: false,
    meta: {
      enableRowspan: true,
    },
    enableSorting: false,
    cell: ({ row }) =>
      h(UButton, {
        to: localePath({
          name: "controle-de-producao-linha-producao-id",
          params: { id: row.getValue("id") },
        }),
        label: "Editar",
      }),
  },
  {
    accessorKey: "linha_producao",
    id: "id",
    meta: {
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
  {
    accessorKey: "invativo",
    header: "Ativo",
    cell: ({ row }) =>
      row.getValue("inativo")
        ? h(UBadge, { variant: "subtle", color: "error" }, "Invativo")
        : h(UBadge, { variant: "subtle", color: "success" }, "Ativo"),
  },
];

const tableApi = useVueTable({
  columns,
  getCoreRowModel: getCoreRowModel(),
  data: produtos,
});
</script>
<template>
  <div class="flex-1 w-full container mx-auto py-4">
    <h1 class="text-xl mb-4">
      Controle diário de produção -
      {{ formatedDate }}
    </h1>
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
        class="[&>tr]:data-[selectable=true]:focus-visible:outline-primary"
      >
        <tr v-for="row in tableApi.getRowModel().rows" :key="row.id">
          <td
            v-for="cell in row.getVisibleCells()"
            :key="cell.id"
            :hidden="
              cell.column.columnDef?.meta?.enableRowspan &&
              row.original.rowspan === 0
            "
            :rowspan="
              cell.column.columnDef?.meta?.enableRowspan
                ? row.original.rowspan
                : 1
            "
            :class="{
              'border-default border-b':
                row.original.last || cell.column.columnDef?.meta?.enableRowspan,
            }"
            class="p-2 text-sm whitespace-nowrap [&:has([role=checkbox])]:pe-0"
          >
            <FlexRender
              :render="cell.column.columnDef.cell"
              :props="cell.getContext()"
            />
          </td>
        </tr>
      </tbody>
      <tfoot>
        <tr>
          <td :colspan="tableApi.getAllColumns().length" class="w-full">
            <NuxtLinkLocale
              :to="'controle-de-producao-linha-producao-novo'"
              class="px-4 w-full flex flex-row items-center py-3 gap-2 group cursor-pointer"
            >
              <div
                class="h-px bg-accented grow group-hover:h-0.5 group-hover:bg-white transition-[height_background]"
              ></div>
              <UIcon name="i-lucide-plus"></UIcon>
              <span class="leading-none">Adicionar linha de produção</span>
              <div
                class="h-px bg-accented grow group-hover:h-0.5 group-hover:bg-white transition-[height_background]"
              ></div>
            </NuxtLinkLocale>
          </td>
        </tr>
      </tfoot>
    </table>
  </div>
</template>
