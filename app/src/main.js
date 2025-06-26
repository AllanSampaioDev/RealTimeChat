import { createApp } from 'vue'
import App from './App.vue'
import { Quasar } from 'quasar'
import quasarUserOptions from './quasar-user-options'
import router from './router'
import { createPinia } from 'pinia'
import piniaPersistedstate from 'pinia-plugin-persistedstate'
import Toast from "vue-toastification";
import "vue-toastification/dist/index.css";

const pinia = createPinia()
pinia.use(piniaPersistedstate)

createApp(App)
  .use(router)
  .use(pinia)
  .use(Quasar, quasarUserOptions)
  .use(Toast, {
    timeout: 3000,
    position: 'top-right',
  })
  .mount('#app')