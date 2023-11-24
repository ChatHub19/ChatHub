<script setup>
import ProfileAvatar from "vue-profile-avatar";
</script>

<template>
  <div class="wrapper">
    <div class="avatar">
      <ProfileAvatar :username = "username" size = "m" @click="showOption"/> 
      <div id="userinfo">
        <span> {{ username }} </span>
        <span> Offline </span>
      </div>
      <div class="option"> <font-awesome-icon icon="fa-solid fa-gear" /> </div>
      <div class="option"> icon </div>
      <div class="option"> icon </div>
      <div class="option"> icon </div>
    </div>
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
      showOption: false,
    }
  },
  components: {
    ProfileAvatar,
  },  
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
}
.avatar {
  display: flex;
  align-items: center;
  border: 2px solid black;
  margin-top: 10px;
  width: fit-content;
  padding: .25rem;
  background: lightslategrey;
}
.option {
  display: flex;
  justify-content: center;
  align-items: center;
  border: 2px solid black;
  border-radius: 20px;
  width: 3rem;
  height: 3rem;
  margin: 5px;
  background: lightgrey;
}
#userinfo {
  display: flex;
  flex-direction: column;
  margin: 10px;
}
span {
  margin-left: 10px;
}
</style>