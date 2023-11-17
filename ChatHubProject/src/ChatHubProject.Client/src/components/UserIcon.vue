<script setup>
import ProfileAvatar from "vue-profile-avatar";
</script>

<template>
  <div class="wrapper">
    <div class="avatar">
      <ProfileAvatar :username = "username" size = "m" @click="showOption"/> 
      <div class="option"> Profil </div>
      <div class="option"> Profil </div>
      <div class="option"> Profil </div>
      <div class="option"> Profil </div>
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
  width: 100vw;
}
.avatar {
  display: flex;
  justify-content: center;
  flex-direction: column;
  height: 100vh;
}
.option {
  display: flex;
  justify-content: center;
  align-items: center;
  border: 2px solid black;
  border-radius: 20px;
  width: 200px;
  height: 3rem;
  background: lightgrey;
  margin-left: 100px;
}
</style>