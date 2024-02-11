import store from "./store.js";
import "./assets/main.css";

import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import { library } from "@fortawesome/fontawesome-svg-core";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import { faPlus } from "@fortawesome/free-solid-svg-icons";
library.add(faPlus);

const app = createApp(App);

app.use(router);
app.use(store);
app.component("font-awesome-icon", FontAwesomeIcon);

app.mount("#app");

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store.js'
import axios from "axios";
import process from 'node:process'
import './assets/main.css'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap/dist/js/bootstrap.js'
import { library } from '@fortawesome/fontawesome-svg-core'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import { faArrowLeftLong, faArrowRightLong, faCaretLeft, faGear, faX } from '@fortawesome/free-solid-svg-icons'
library.add(faArrowLeftLong, faArrowRightLong, faGear, faX, faCaretLeft)

axios.defaults.baseURL = process.env.NODE_ENV == 'production' ? "/api" : "https://localhost:7081/api";
axios.defaults.withCredentials = true;

axios
  .get("user/userinfo")
  .then((r) => {
    store.commit("authenticate", r.data);
  })
  .finally(() => {
    const app = createApp(App);
    app.use(router)
    app.use(store)
    app.component('font-awesome-icon', FontAwesomeIcon)
    app.mount('#app')
  })
  .catch(() => {
    router.push("/login");
  })