<script setup>

</script>

<template>
  <div class="wrapper">
    <h1> Account </h1>
    <section>
      <h6> Username </h6> 
      <input type="text" :placeholder="username" v-model="accountModel.username">
      <h6> Password </h6> 
      <input type="password" :placeholder="password" @keyup.enter="checkPassword()" v-if="!checkPassword">
      <div v-else>
        <label for="New Password"> New Password </label>
        <input name="New Password" type="password" v-model="accountModel.password">
        <input name="Confirm Password" type="password" v-model="accountModel.password" @keyup.enter="confirmPassword">
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
        checkPassword: false,
      }
    }
  },
  methods: {
    checkPassword() {
      this.checkPassword = true;
      alert(this.checkPassword)
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
input, select {
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

</style>