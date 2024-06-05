<script setup>
import axios from "axios";
import store from '../store.js'
import PasswordButton from "../components/PasswordButton.vue"
import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';
</script>

<template>
  <div class="wrapper">
    <div id="flex">
      <h1> Account </h1>
      <router-link id="redirect-btn" to="/"> 
        <font-awesome-icon class="icon" icon="fa-solid fa-x" />
      </router-link>
    </div>
    <section>
      <div>
        <h6> Displayname </h6> 
        <input 
          type="text" :placeholder="displayname" v-model="accountModel.displayname" 
          @keyup.enter="setDisplayname()"
          ref="displaynamefield"
        >
      </div>
      <div>
        <h6> Email </h6> 
        <input 
          type="email" :placeholder="email" v-model="accountModel.email"
          @keyup.enter="setEmail()"
          ref="emailfield"
        >
      </div>
      <div id="password">
        <h6> Password </h6> 
        <PasswordButton/>
      </div>
      <div id="delete">
        <h6> Delete Account </h6> 
        <button class="button" @click="deleteAccount()"> Delete Account </button>
      </div>
    </section>
  </div>
</template>

<script>
export default {
  async mounted() {
    await this.getUserdata();
    await this.getDisplayname();
    await this.getEmail();
  },
  computed: {
    guid() {
      return this.$store.state.userdata.userGuid;
    },
    displayname() { 
      return "";
    },
    email() {
      return "";
    }
  },
  data() {
    return {
      accountModel: {
        displayname: "",
        email: "",
      }
    }
  },
  methods: {
    async getUserdata() {
      try {
        var userdata = (await axios.get("user/userinfo")).data
        store.commit("authenticate", userdata)
      } 
      catch (e) { e.response.data }
    },
    async getDisplayname() {
      this.accountModel.displayname = (await axios.get(`/user/${this.guid}`)).data.displayname
    },
    async setDisplayname() {
      try {await axios.put(`/user/displayname/${this.guid}`, this.accountModel) }
      catch(e) { toast.error(e.response.data) }
      this.$refs.displaynamefield.blur()
      toast.success("Success")
    },
    async getEmail() {
      this.accountModel.email = (await axios.get(`/user/${this.guid}`)).data.email 
    },
    async setEmail() {
      try { await axios.put(`/user/email/${this.guid}`, this.accountModel) }
      catch(e) { toast  .error(e.response.data.errors.Email[0]) }
      this.$refs.emailfield.blur()
      toast.success("Success")
    },
    async deleteAccount() {
      try { 
        await axios.delete(`/user/${this.guid}`) 
        this.$router.push("/login")
        toast.success("Success")
      }
      catch(e) { toast  .error(e.response.data) }
    },
    returnToHomepage() {
      this.$router.push("/")
    }
  }
}
</script>

<style scoped>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}
.wrapper {
  height: 100vh;
  max-width: 100vw;
  padding: 3rem;
  flex-direction: column;
}
input, button {
  width: 100%;
  padding: .5rem;
  border: 1px solid transparent;
  border-radius: 5px;
  color: white;
  background: rgba(0, 0, 0, 0.374);
}
input:hover {
  background: rgba(0, 0, 0, 0.56);
}
section {
  margin-top: 1%;
}
h6 {
  margin: 10px 0;
}
button {
  background: red;
}
.icon {
  text-decoration: none;
  cursor: pointer;
  color: white;
  padding: 10px;
  border: 3px solid white;
  border-radius: 50%;
}
.invalid {
  color: red;
}
#flex {
  display: flex;
  align-items: center;
}
#redirect-btn {
  margin-left: auto;
}
.icon:hover {
  background: rgba(23, 24, 26, 0.311);
}
#delete {
  margin: 5rem 0;
}
</style>