<script setup lang="ts">
let producoes = [
  { id: 1, nome: "Linha de produção 1", ativo: true },
  { id: 2, nome: "Linha de produção 2", ativo: false },
];
let alert = (arg: any) => {
  window.alert(arg);
};
</script>

<template>
  <div class="p-4">
    <h1>Linhas de produção</h1>
    <button
      type="button"
      class="border border-black rounded p-1 mb-4 cursor-pointer"
      @click="alert('novo')"
    >
      Criar novo
    </button>
    <table>
      <thead>
        <tr>
          <td class="border border-black">Nome</td>
          <td class="border border-black">Ativo</td>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in producoes">
          <td class="p-2 border border-black">
            <NuxtLink
              :to="{ name: 'linha-producao-id', params: { id: item.id } }"
            >
              {{ item.nome }}
            </NuxtLink>
          </td>
          <td class="p-2 border border-black">
            {{ item.ativo ? "ativo" : "inativo" }}
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
