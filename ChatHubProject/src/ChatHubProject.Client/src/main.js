import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import './assets/main.css'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap/dist/js/bootstrap.js'
import { library } from '@fortawesome/fontawesome-svg-core'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import { faArrowLeftLong, faArrowRightLong } from '@fortawesome/free-solid-svg-icons'
library.add(faArrowLeftLong, faArrowRightLong)

const app = createApp(App)

app.use(router)
app.component('font-awesome-icon', FontAwesomeIcon)
app.mount('#app')
