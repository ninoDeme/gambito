<script setup lang="ts">
import dayjs from 'dayjs';
const route = useRoute();
const diaAtual = dayjs(route.params.dia, 'YYYY-MM-DD', true);
if (!diaAtual.isValid()) {
  throw new Error("Data invalida")
}
let data = [
  { id: 1, data: diaAtual, produto: 1},
  { id: 2, data: diaAtual, produto: 1}
]
</script>
<template>
  <div>{{ diaAtual?.toISOString() }}</div>
  <ElTable :data=data>
    <ElTableColumn prop="id" label="ID"></ElTableColumn>
    <ElTableColumn prop="data" label="Data"></ElTableColumn>
    <ElTableColumn prop="produto" label="Produto"></ElTableColumn>
  </ElTable>
</template>
