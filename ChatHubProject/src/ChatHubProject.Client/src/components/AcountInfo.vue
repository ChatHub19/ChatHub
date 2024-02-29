<script setup>
import axios from "axios";
import store from '../store.js'
import PasswordButton from "../components/PasswordButton.vue"
import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';
</script>

<template>
  <div class="wrapper">
    <h1> Account </h1>
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
      return this.$store.state.user.guid;
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
      var userdata = (await axios.get("user/userinfo")).data
      store.commit("authenticate", userdata);
    },
    async getDisplayname() {
      this.accountModel.displayname = (await axios.get(`/user/${this.guid}`)).data.displayname
    },
    async setDisplayname() {
      await axios.put(`/user/displayname/${this.guid}`, this.accountModel)
      this.$refs.displaynamefield.blur();
    },
    async getEmail() {
      this.accountModel.email = (await axios.get(`/user/${this.guid}`)).data.email 
    },
    async setEmail() {
      try { await axios.put(`/user/email/${this.guid}`, this.accountModel) }
      catch(e) { toast.error("Invalid Email"); }
      this.$refs.emailfield.blur();
    },
    getEmptyPasswordValue() {
      this.accountModel.password = "";
    },
    async setPassword() {
      (await axios.put(`/user/${this.guid}`)).data.password
    },
    checkPassword() {
      this.accountModel.checkedPassword = true;
    },
    confirmPassword() {
      this.accountModel.newpassword = "";
      this.accountModel.confirmpassword = "";
      this.accountModel.checkedPassword = false;
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
input, button {
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
.invalid {
  color: red;
}
#password {
  margin: 2rem 0  ;
}
delete {
  margin: 5rem 0;
}
</style>