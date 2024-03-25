<script setup>
import axios from "axios";
import store from '../store.js'
import signalRService from '../services/SignalRService.js';
</script>

<template>
  <div class="wrapper">
    <div class="flex">
      <div class="message-container">
        <input type="text" v-model="accountModel.message" placeholder="Send Message" @keypress.enter="SendMessage()">
      </div>
    </div>
  </div>
</template>

<script>
export default {
  async mounted() {
    signalRService.configureConnection(); 
		signalRService.connect();
    await this.getUserdata();
    await this.getDisplayname();
    try { 
      signalRService.sendJoinedMessageToAll();
      signalRService.subscribeEvent("ReceiveMessage", this.onMessageReceived); 
      signalRService.subscribeEvent("ReceiveJoinedMessage", this.onMessageReceived); 
    } 
    catch (e) { console.log(e); }    
  },
  unmounted() {
    signalRService.unsubscribeEvent("ReceiveMessage", this.onMessageReceived);
  },
  computed: {
    guid() {
      return this.$store.state.userdata.userGuid
    },
    displayname() {
      return this.$store.state.userdata.displayname
    },
  }, 
  data() {
    return {
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
      } 
      catch (e) { e.response.data }
    },
    async getDisplayname() {
      this.accountModel.displayname = (await axios.get(`/user/${this.guid}`)).data.displayname
    },
    async SendMessage() {
      if (this.accountModel.message !== "") { 
        const text = this.accountModel.message;
        const displayname = this.accountModel.displayname;
        const time = new Date().toLocaleTimeString();
        const userguid = this.guid; 
        (await axios.post("message/send", {text, userguid, time}));
        signalRService.sendMessageToAll(text, displayname, time);
      }
      this.accountModel.message = "";
    }
  },
}
</script>

<style scoped>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
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
.flex {
  display: flex;
  align-items: center;
}
.message-container {
  width: 100%;
}
</style>