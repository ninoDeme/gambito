<script setup lang="ts">
import { UButton } from "#components";
import { parseDate } from "@internationalized/date";
import type { TableColumn } from "@nuxt/ui";

const localePath = useLocalePath();
const localeRoute = useLocaleRoute();

const route = localeRoute('dia-dia')!;
const diaAtual = parseDate(route.params.dia);
const { data, status, error, refresh, clear } = await useFetch(
  `/api/dia/${diaAtual}`,
);

const columns: TableColumn<{
  id: number;
  date: string | undefined;
  produto: number;
}>[] = [
  {
    id: "actions",
    header: () => h("span", { class: "w-max" }, "Ações"),
    enableHiding: false,
    enableSorting: false,
    cell: ({ row }) =>
      h(UButton, {
        to: localePath({
          name: "linha-producao",
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
    accessorKey: "date",
    header: "Date",
    cell: ({ row }) => {
      return new Date(row.getValue("date")).toLocaleString("pt-BR", {
        dateStyle: "short",
      });
    },
  },
  {
    accessorKey: "produto",
    header: "Produto",
    cell: ({ row }) => row.getValue("produto"),
  },
];

const table = useTemplateRef("table");
</script>
<template>
  <div>{{ diaAtual?.toString() }}</div>
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
      :data="data"
      :columns="columns"
      sticky
      class="h-96"
      :loading="status === 'pending'"
    >
    </UTable>
  </div>
</template>
