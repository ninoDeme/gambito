<script setup lang="ts">
import { appName } from "~/constants";

useHead({
  title: appName,
});

import dayjs from "dayjs";
import "dayjs/locale/pt-br";

dayjs.locale("pt-br");
</script>
<template>
  <NuxtLayout>
    <NuxtPage />
  </NuxtLayout>
</template>
