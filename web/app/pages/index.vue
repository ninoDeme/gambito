<script setup lang="ts">
import type { CalendarProps } from "@nuxt/ui";

definePageMeta({});
let cal: CalendarProps["ui"] = {
  cellTrigger: ["rounded-none w-full h-full"],
  cell: ["rounded-none border w-full h-full min-w-auto"],
};
</script>

<template>
  <UCalendar :ui="cal" size="xl">
    <template #day="{ day }">
      <NuxtLinkLocale
        :to="{ name: 'dia-dia', params: { dia: day.toString() } }"
        class="w-full h-full block p-8"
      >
        {{ day.day }}
      </NuxtLinkLocale>
    </template>
  </UCalendar>
</template>

<style></style>
