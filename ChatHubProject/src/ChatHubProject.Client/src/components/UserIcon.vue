<script setup>
import ProfileAvatar from "vue-profile-avatar";
</script>

<template>
  <div class="wrapper">
    <div class="avatar">
      <ProfileAvatar :username="username" size="m" colorType="pastel"/> 
      <div id="userinfo">
        <span> {{ username }} </span>
        <span> Offline </span>
      </div>
      <router-link id="redirect-btn" to="/custom"> 
        <div class="option"> <font-awesome-icon icon="fa-solid fa-gear" /> </div> 
      </router-link> 
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
  background: grey;
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
.option:hover {
  background: rgba(211, 211, 211, 0.578);
}
#redirect-btn {
  text-decoration: none;
  color: black;
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