<script setup lang="ts">
import type { CalendarProps } from "@nuxt/ui";

definePageMeta({});
let cal: CalendarProps["ui"] = {
  cellTrigger: ["rounded-none w-full h-full m-0"],
  cell: [
    "rounded-none w-full h-full border-accented min-w-auto border-r first:border-l",
  ],
  gridRow: ["border-b first:border-t border-accented"],
};
</script>

<template>
  <UContainer class="py-8">
    <UCalendar :ui="cal" size="xl">
      <template #day="{ day }">
        <NuxtLinkLocale
          :to="{
            name: 'controle-de-producao-dia-dia',
            params: { dia: day.toString() },
          }"
          class="w-full h-full block p-8"
        >
          {{ day.day }}
        </NuxtLinkLocale>
      </template>
    </UCalendar>
  </UContainer>
</template>

<style></style>
