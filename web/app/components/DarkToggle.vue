<script setup lang="ts">
const color = useColorMode()

useHead({
  meta: [{
    id: 'theme-color',
    name: 'theme-color',
    content: () => color.value === 'dark' ? '#222222' : '#ffffff',
  }],
})

function toggleDark() {
  color.preference = color.value === 'dark' ? 'light' : 'dark'
}
</script>

<template>
  <button class="inline-flex justify-center items-center" @click="toggleDark">
    <el-icon>
      <ElIconMoon v-if="color.value === 'dark'"></ElIconMoon>
      <ElIconSunny v-else></ElIconSunny>
    </el-icon>
  </button>
</template>
