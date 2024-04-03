<script setup>
import axios from "axios";
import signalRService from '../services/SignalRService.js';
import ProfileAvatar from "vue-profile-avatar";
</script>

<template>
  <div class="wrapper">
    <div class="avatar">
      <ProfileAvatar v-if="authenticated" :username="displayname" size="m" colorType="pastel"/> 
      <div id="userinfo">
        <span> {{ displayname }} </span>
        <span> {{ status }} </span>
      </div>
      <router-link v-if="this.$route.name === 'home'" class="redirect-btn" to="/login" @click="logout()"> 
        <div class="option"> <font-awesome-icon icon="fa-solid fa-caret-left" /> </div> 
      </router-link> 
      <router-link v-else class="redirect-btn" to="/"> 
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
    await this.getOnlineStatus();   
  },
  computed: {
    guid() {
      return this.$store.state.userdata.userGuid
    },
    status() {
      return this.accountModel.status
    },
    displayname() {
      return this.$store.state.userdata.displayname
    },
    authenticated() {
      return this.$store.state.isLoggedIn
    },

  }, 
  data() {
    return {
      accountModel: {
        status: "",
        displayname: "",
      }
    }
  }, 
  methods: {
    async getUserdata() {
      try {
        var userdata = (await axios.get("user/userinfo")).data
        this.$store.commit("authenticate", userdata)
      } 
      catch (e) { e.response.data }
    },
    async getDisplayname() {
      this.accountModel.displayname = (await axios.get(`/user/${this.guid}`)).data.displayname
    },
    async getOnlineStatus() {
      if(signalRService.connected)
        this.accountModel.status = "Online"
      else
        this.accountModel.status = "Offline"
    },
    async logout() {
      (await axios.get("user/logout")).data
    },
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
span {
  margin-left: 10px;
  color: white;
}
.avatar {
  display: flex;
  align-items: center;
  justify-content: center;
  width: calc(20vw + 65px);
  padding: .25rem;
  background: #28242c;
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
  background: lightgrey;;
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
.displayname {
  font-weight: bold;
  margin-right: 10px;
  color: white;
}
</style>