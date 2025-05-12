<script setup lang="ts">
import { appName } from "~/constants";
import * as locales from '@nuxt/ui/locale'

const { locale } = useI18n()

useHead({
  title: appName,
});

</script>
<template>
  <UApp :locale="locales[locale]">
    <NuxtLayout>
      <NuxtPage />
    </NuxtLayout>
  </UApp>
</template>
