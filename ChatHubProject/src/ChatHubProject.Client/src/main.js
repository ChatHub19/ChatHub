import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store.js";
import axios from "axios";
import process from "node:process";
import "./assets/main.css";
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap/dist/js/bootstrap.js";
import store from "./store.js";
import axios from "axios";
import process from "node:process";
import "./assets/main.css";
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap/dist/js/bootstrap.js";
import { library } from "@fortawesome/fontawesome-svg-core";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import {
  faArrowLeftLong,
  faArrowRightLong,
  faGear,
  faPlus,
} from "@fortawesome/free-solid-svg-icons";
library.add(faArrowLeftLong, faArrowRightLong, faGear, faPlus);
import {
  faArrowLeftLong,
  faArrowRightLong,
  faGear,
  faPlus,
} from "@fortawesome/free-solid-svg-icons";
library.add(faArrowLeftLong, faArrowRightLong, faGear, faPlus);

axios.defaults.baseURL =
  process.env.NODE_ENV == "production" ? "/api" : "https://localhost:7081/api";
axios.defaults.withCredentials = true;
axios.defaults.baseURL =
  process.env.NODE_ENV == "production" ? "/api" : "https://localhost:7081/api";
axios.defaults.withCredentials = true;
const app = createApp(App);
const app = createApp(App);

axios
  .get("user/userinfo")
  .then((r) => {
    store.commit("authenticate", r.data);
  })
  .finally(() => {
    app.use(router);
    app.use(store);
    app.component("font-awesome-icon", FontAwesomeIcon);
    app.mount("#app");
  })
  .catch(() => {
    router.push("/login");
  });
