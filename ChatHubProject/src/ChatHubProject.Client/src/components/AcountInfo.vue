<script setup>

</script>

<template>
  <div class="wrapper">
    <h1> Account </h1>
    <section>
      <div>
        <h6> Username </h6> 
        <input type="text" :placeholder="username" v-model="accountModel.username">
      </div>
      <div v-if="!accountModel.checkedPassword">
        <h6> Password </h6> 
        <input type="password" :placeholder="password" @keyup.enter="checkPassword()">
      </div>
      <div v-else> 
        <h6> New Password </h6> 
        <input type="password" v-model="accountModel.newpassword">
        <h6> Confirm Password </h6> 
        <input type="password" v-model="accountModel.confirmpassword" @keyup.enter="confirmPassword()">
      </div>
      <div>
        <h6> Email </h6> 
        <input type="email" :placeholder="email" v-model="accountModel.email">
      </div>
      <div id="delete">
        <button class="button" @click="deleteAccount()"> Delete Account </button>
      </div>
    </section>
  </div>
</template>

<script>
export default {
  computed: {
    username() {
      return this.$store.state.user.username;
    },
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