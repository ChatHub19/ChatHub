<script setup>
import axios from "axios";
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
      <div v-if="!accountModel.checkedPassword">
        <h6> Password </h6> 
        <input 
          type="password" :value="password"
          @focus="getEmptyPasswordValue()" 
          @blur="getPasswordValue()"
          @keyup.enter="checkPassword()"
        >
      </div>
      <div v-else> 
        <h6> New Password </h6> 
        <input type="password" v-model="accountModel.newpassword">
        <h6> Confirm Password </h6> 
        <input type="password" v-model="accountModel.confirmpassword" @keyup.enter="confirmPassword()">
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
      <div id="delete">
        <button class="button" @click="deleteAccount()"> Delete Account </button>
      </div>
    </section>
  </div>
</template>

<script>
export default {
  async mounted() {
    await this.getUsername();
    await this.getPassword();
    await this.getEmail();
  },
  computed: {
    guid() {
      return this.$store.state.user.guid;
    },
    username() { 
      return "";
    },
    password() {
      return "123456789";
    },
    email() {
      return "";
    }
  },
  data() {
    return {
      accountModel: {
        username: "",
        password: "",
        newpassword: "",
        confirmpassword: "",
        checkedPassword: false,
        email: "",
      }
    }
  },
  methods: {
    async getUsername() {
      this.accountModel.username = (await axios.get(`/user/${this.guid}`)).data.username
    },
    async getPassword() {
      this.accountModel.password = (await axios.get(`/user/${this.guid}`)).data.password
    },
    async getEmail() {
      this.accountModel.email = (await axios.get(`/user/${this.guid}`)).data.email
    },
    async setUsername() {
      (await axios.put(`/user/${this.guid}`)).data.username
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
    getEmptyPasswordValue() {
      console.log(this.accountModel.password)
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
#delete {
  margin: 5rem 0;
}
</style>