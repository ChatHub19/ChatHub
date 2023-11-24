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
import { faArrowLeftLong, faArrowRightLong, faGear } from '@fortawesome/free-solid-svg-icons'
library.add(faArrowLeftLong, faArrowRightLong, faGear)

axios.defaults.baseURL = process.env.NODE_ENV == 'production' ? "/api" : "https://localhost:7081/api";

const app = createApp(App)

app.use(router)
app.use(store)
app.component('font-awesome-icon', FontAwesomeIcon)
app.mount('#app')
