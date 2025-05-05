<script setup lang="ts">
definePageMeta({});
function log(val: any) {
  console.log(val);
}
</script>

<template>
  <el-calendar @input="log($event)">
    <template #date-cell="{ data }">
      <NuxtLink
        :to="{ name: 'linha-producao', query: { dia: data.day } }"
        class="w-full h-full block"
        :class="data.type"
      >
        {{ data.date.getDate() }}
      </NuxtLink>
    </template>
  </el-calendar>
</template>

<style>
.prev-month,
.next-month {
  color: gray;
}
</style>
