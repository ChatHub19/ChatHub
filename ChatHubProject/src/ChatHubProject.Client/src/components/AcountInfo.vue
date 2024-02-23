<script setup>
import axios from "axios";
import store from '../store.js'
import PasswordButton from "../components/PasswordButton.vue"
</script>

<template>
  <div class="wrapper">
    <h1> Account </h1>
    <section>
      <div>
        <h6> Username </h6> 
        <input 
          type="text" :placeholder="username" v-model="accountModel.username" 
          @focus="getEmptyUsernameValue()" 
          @blur="getUsernameValue()"
          @keyup.enter="setUsername()"
        >
      </div>
      <div>
        <h6> Display name </h6> 
        <input 
          type="text" :placeholder="displayname" v-model="accountModel.displayname" 
          @focus="getEmptyUsernameValue()" 
          @blur="getUsernameValue()"
          @keyup.enter="setUsername()"
        >
      </div>
      <div>
        <h6> Email </h6> 
        <input 
          type="email" :placeholder="email" v-model="accountModel.email"
          @focus="getEmptyEmailValue()" 
          @blur="getEmailValue()"
          @keyup.enter="setEmail()"
        >
      </div>
      <div id="password">
        <h6> Password </h6> 
        <PasswordButton/>
      </div>
      <div id="delete">
        <button class="button" @click="deleteAccount()"> Delete Account </button>
      </div>
    </section>
  </div>
</template>

<script>
export default {
  async mounted() {
    await this.getUserdata();
    await this.getUsername();
    await this.getDisplayname();
    await this.getEmail();
  },
  computed: {
    guid() {
      return this.$store.state.user.guid;
    },
    username() { 
      return "";
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
        username: "",
        displayname: "",
        email: "",
      }
    }
  },
  methods: {
    async getUserdata() {
      var userdata = (await axios.get("user/userinfo")).data
      store.commit("authenticate", userdata);
    },
    async getUsername() {
      this.accountModel.username = (await axios.get(`/user/${this.guid}`)).data.username
    },
    async getDisplayname() {
      this.accountModel.displayname = (await axios.get(`/user/${this.guid}`)).data.displayname
    },
    async getEmail() {
      this.accountModel.email = (await axios.get(`/user/${this.guid}`)).data.email
    },
    async setUsername() {
      console.log(await axios.put(`/user/${this.guid}`, this.accountModel))
      await axios.put(`/user/${this.guid}`, this.accountModel)
    },
    async setPassword() {
      (await axios.put(`/user/${this.guid}`)).data.password
    },
    async setEmail() {
      (await axios.put(`/user/${this.guid}`)).data.email
    },
    checkPassword() {
      this.accountModel.checkedPassword = true;
    },
    confirmPassword() {
      this.accountModel.newpassword = "";
      this.accountModel.confirmpassword = "";
      this.accountModel.checkedPassword = false;
    },
    getEmptyUsernameValue() {
      this.accountModel.username = "";
    },
    async getUsernameValue() {
      this.accountModel.username = (await axios.get(`/user/${this.guid}`)).data.username
    },
    getEmptyDisplaynameValue() {
      this.accountModel.displayname = "";
    },
    async getDisplaynameValue() {
      this.accountModel.displayname = (await axios.get(`/user/${this.guid}`)).data.displayname
    },
    getEmptyPasswordValue() {
      this.accountModel.password = "";
    },
    async getPasswordValue() {
      this.accountModel.password = (await axios.get(`/user/${this.guid}`)).data.password
    },
    getEmptyEmailValue() {
      this.accountModel.email = "";
    },
    async getEmailValue() {
      this.accountModel.email = (await axios.get(`/user/${this.guid}`)).data.email
    },
    deleteAccount() {
      alert("Delete");
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
input, select, button {
  width: 50%;
  padding: .5rem;
  border: 1px solid transparent;
  border-radius: 5px;
  color: white;
  background: rgba(0, 0, 0, 0.374);
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
#password {
  margin: 2rem 0  ;
}
delete {
  margin: 5rem 0;
}
</style>