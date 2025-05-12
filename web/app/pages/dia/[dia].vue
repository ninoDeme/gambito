<script setup lang="ts">
import { UBadge } from "#components";
import { parseDate } from "@internationalized/date";
import type { TableColumn } from "@nuxt/ui";
const route = useRoute("dia-dia___pt_br");
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
    accessorKey: "id",
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
</script>
<template>
  <div>{{ diaAtual?.toString() }}</div>
  <UButton @click="refresh()">Recarregar</UButton>
  <UTable
    :data="data"
    :loading="status === 'pending'"
    :columns="columns"
  ></UTable>
</template>
