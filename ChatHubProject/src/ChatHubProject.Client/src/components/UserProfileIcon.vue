<script setup>
import axios from "axios";
import store from '../store.js'
import ProfileAvatar from "vue-profile-avatar";
</script>

<template>
  <div class="wrapper">
    <div class="avatar">
      <ProfileAvatar :username="displayname" size="m" colorType="pastel"/> 
      <div id="userinfo">
        <span> {{ displayname }} </span>
        <span> Offline </span>
      </div>
      <router-link class="redirect-btn" to="/login" @click="logout()"> 
        <div class="option"> <font-awesome-icon icon="fa-solid fa-caret-left" /> </div> 
      </router-link> 
      <router-link class="redirect-btn" to="/custom"> 
        <div class="option"> <font-awesome-icon icon="fa-solid fa-gear" /> </div> 
      </router-link> 
    </div>
  </div>
</template>

<script>
export default {
  async mounted() {
    await this.getUserdata();
    await this.getDisplayname();
  },
  computed: {
    guid() {
      return this.$store.state.user.guid;
    },
    displayname() {
      return this.$store.state.user.displayname;
    },
  }, 
  data() {
    return {
      showOption: false,
      accountModel: {
        displayname: "",
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
    async logout() {
      (await axios.get("user/logout")).data
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
.redirect-btn {
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