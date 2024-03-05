<script setup>
import axios from "axios";
import store from '../store.js'
import signalRService from '../services/SignalRService.js';
import ProfileAvatar from "vue-profile-avatar";
</script>

<template>
  <div class="wrapper">
    <div class="flex flexbox">
      <div class="message-box">
        <div v-for="(message, index) in messages" :key="index" class="message">
          {{ message.text }} 
          <span class="time"> {{ formatTime(message.time) }} </span>
        </div>
      </div>
    </div>
    <div class="flex">
      <div class="avatar">
        <ProfileAvatar v-if="authenticated" :username="displayname" size="m" colorType="pastel"/> 
        <div id="userinfo">
          <span> {{ displayname }} </span>
          <span> {{ status }} </span>
        </div>
        <router-link class="redirect-btn" to="/login" @click="logout()"> 
          <div class="option"> <font-awesome-icon icon="fa-solid fa-caret-left" /> </div> 
        </router-link> 
        <router-link class="redirect-btn" to="/custom"> 
          <div class="option"> <font-awesome-icon icon="fa-solid fa-gear" /> </div> 
        </router-link> 
      </div>
      <div class="message-container">
        <input type="text" v-model="accountModel.message" placeholder="Send Message" @keypress.enter="SendMessage()">
      </div>
    </div>
  </div>
</template>

<script>
export default {
  async mounted() {
    await this.getUserdata();
    await this.getDisplayname();
    await this.getPrevMessage();
    await this.getOnlineStatus();
    try { signalRService.subscribeEvent("ReceiveMessage", this.onMessageReceived); } 
    catch (e) { console.log(e); }
  },
  unmounted() {
    signalRService.unsubscribeEvent("ReceiveMessage", this.onMessageReceived);
  },
  computed: {
    guid() {
      return this.$store.state.user.guid
    },
    displayname() {
      return this.$store.state.user.displayname
    },
    authenticated() {
      return this.$store.state.user.isLoggedIn
    }
  }, 
  data() {
    return {
      showOption: false,
      messages: [],
      status: "Offline",
      accountModel: {
        message: "",
        displayname: "",
      }
    }
  }, 
  methods: {
    async getUserdata() {
      try {
        var userdata = (await axios.get("user/userinfo")).data
        store.commit("authenticate", userdata)
        signalRService.configureConnection(); 
				signalRService.connect(); 
        console.log()
      } 
      catch (e) { e.response.data }
    },
    async getDisplayname() {
      this.accountModel.displayname = (await axios.get(`/user/${this.guid}`)).data.displayname
    },
    async getPrevMessage() {
      this.messages = (await axios.get("message")).data
    },
    getOnlineStatus() {
      if(signalRService.connected)
        this.status = "Online"
      else
        this.status = "Offline"
    },
    async logout() {
      (await axios.get("user/logout")).data
    },
    async SendMessage() {
      if (this.accountModel.message !== "") { 
        const text = this.accountModel.message;
        const displayname = this.accountModel.displayname;
        const time = new Date().toLocaleTimeString();
        const userguid = this.guid; 
        (await axios.post("message/send", {text, userguid, time}));
        signalRService.sendMessageToAll(text, displayname, time);
        // try { signalRService.subscribeEvent("MessageReceived", this.onMessageReceived()); } 
        // catch (e) { console.log(e); }
      }
      this.accountModel.message = "";
      // try { signalRService.subscribeEvent("MessageReceived", this.onMessageReceived()); } 
      // catch (e) { console.log(e); }
    },
    async onMessageReceived() {
      this.messages = (await axios.get("message")).data;
    },
    formatTime(time) {
      const date = new Date(time);
      const now = new Date();
      if(date.getDate() < now.getDate()){
        return date.toLocaleDateString();
      }
      return date.toLocaleTimeString();
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
}
input {
  width: 99%;
  margin: 0 10px;
  color: black;
  padding: 10px;
  border-radius: 25px;
  background: lightgrey;
}
input::placeholder {
  color: black;
}
.avatar {
  display: flex;
  align-items: center;
  border: 2px solid black;
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
.flex {
  display: flex;
  align-items: center;
}
#userinfo {
  display: flex;
  flex-direction: column;
  margin: 10px;
}
.message-container {
  width: 100%;
}
.message-box {
  width: 100%;
  height: 80vh;
  overflow-y: auto;
  border: 3px solid black;
  padding: 10px;
  background: rgba(61, 62, 63, 0.681);
}
.message {
  margin-bottom: 10px;
  padding: 5px;
  border-radius: 5px;

}
.time {
  float: right;
  font-size: 12px;
  color: #868585;
}
</style>