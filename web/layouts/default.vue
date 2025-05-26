<script setup lang="ts">
const localePath = useLocalePath();
const { locale: activeLocale, locales } = useI18n();

const locales2 = computed(() =>
  locales.value.map((l) => ({ label: l.name, code: l.code })),
);
</script>
<template>
  <div class="flex flex-col min-h-dvh">
    <header class="border-b border-b-accented w-full">
      <div class="flex flex-row items-center p-2 container mx-auto">
        <div class="min-w-1/4">
          <UButton
            :variant="'link'"
            :color="'neutral'"
            :active-color="'primary'"
            :to="localePath('index')"
          >
            (Logo)
          </UButton>
        </div>
        <div class="flex-1"></div>
        <UButton
          variant="ghost"
          color="primary"
          active-variant="subtle"
          :to="localePath('controle-de-producao')"
        >
          {{ $t("controle-de-producao") }}
        </UButton>
        <div class="flex-1"></div>

        <div class="min-w-1/4 flex justify-end items-center gap-4">
          <USwitch
            checked-icon="i-lucide-sun"
            unchecked-icon="i-lucide-moon"
            :model-value="$colorMode.preference === 'light'"
            @change="
              $colorMode.preference =
                $colorMode.preference === 'light' ? 'dark' : 'light'
            "
          >
          </USwitch>
          <USelect
            class="min-w-max w-32"
            :items="locales2"
            value-key="code"
            @update:model-value="$i18n.setLocale($event)"
            :model-value="activeLocale"
          >
          </USelect>
        </div>
      </div>
    </header>
    <main class="text-center flex-1">
      <slot />
    </main>
  </div>
</template>
