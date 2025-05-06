<script setup lang="ts">
  const route = useRoute();
</script>
<template>
  <span> {{ route.query }}</span>
</template>
