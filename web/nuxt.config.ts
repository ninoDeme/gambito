// https://v3.nuxtjs.org/docs/directory-structure/nuxt.config
export default defineNuxtConfig({
  modules: [
    "@nuxt/eslint",
    "@nuxt/ui",
    "@vueuse/nuxt",
    "@pinia/nuxt",
    "@nuxtjs/color-mode",
    "@nuxt/fonts",
    "@nuxt/icon",
    "@nuxtjs/i18n",
    "@nuxtjs/supabase",
  ],

  i18n: {
    defaultLocale: "pt_br",
    locales: [
      { code: "en", name: "English", file: "en.json" },
      { code: "pt_br", name: "Português", file: "pt_br.json" },
      { code: "es", name: "Español", file: "es.json" },
    ],
  },

  devtools: {
    enabled: true,
  },

  app: {
    // head
    head: {
      title: "Gambito Web",
      meta: [
        { name: "viewport", content: "width=device-width, initial-scale=1" },
        {
          name: "description",
          content: "Gambito Web",
        },
      ],
      link: [{ rel: "icon", type: "image/x-icon", href: "/favicon.ico" }],
    },
  },

  // css
  css: ["~/assets/css/index.css"],

  // vueuse
  vueuse: {
    ssrHandlers: true,
  },

  // colorMode
  colorMode: {
    classSuffix: "",
    storage: "cookie",
  },

  future: {
    compatibilityVersion: 4,
  },

  experimental: {
    // when using generate, payload js assets included in sw precache manifest
    // but missing on offline, disabling extraction it until fixed
    payloadExtraction: false,
    renderJsonPayloads: true,
    typedPages: true,
  },

  supabase: {
    redirectOptions: {
      login: "/login",
      callback: "/confirm",
      exclude: ["/", "/docs", "/docs/**", "/api/**"],
      saveRedirectToCookie: true,
    },
  },

  compatibilityDate: "2025-05-26",

  nitro: {
    experimental: {
      openAPI: true,
    },
    // prerender: {
    //   crawlLinks: false,
    //   routes: ['/'],
    //   ignore: ['/hi'],
    // },
  },
});
