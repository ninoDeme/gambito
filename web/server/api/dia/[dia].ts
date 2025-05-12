export default defineEventHandler(async (event) => {
  const diaAtual = getRouterParam(event, "dia");
  return [
    { id: 1, date: diaAtual, produto: 1 },
    { id: 2, date: diaAtual, produto: 1 },
  ];
});
