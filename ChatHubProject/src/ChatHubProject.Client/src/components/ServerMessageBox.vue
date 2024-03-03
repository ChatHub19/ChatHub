<script setup>
import axios from "axios";
import store from '../store.js'
import signalRService from '../services/SignalRService.js';
</script>

<template>
  <div class="wrapper">
    <div class="flex">
      <div class="message-box">
        <div v-for="(message, index) in messages" :key="index" class="message">
          {{ message.text }} 
          <span class="time"> {{ formatTime(message.time) }} </span>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  async mounted() {
    try { signalRService.subscribeEvent("ReceiveMessage", this.onReceiveMessage()); } 
    catch (e) { console.log(e); }
    await this.getUserdata();
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
    },
  }, 
  data() {
    return {
      messages: [],
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
    async onReceiveMessage() {
      console.log("Message received");
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
}
</script>

<style scoped>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}
.flex {
  display: flex;
  align-items: center;
}
.message-box {
  width: 100%;
  height: 300px;
  overflow-y: auto;
  border: 1px solid #ccc;
  padding: 10px;
}
.message {
  margin-bottom: 10px;
  padding: 5px;
  border: 1px solid #ccc;
  border-radius: 5px;
}
.time {
  float: right;
  font-size: 12px;
  color: #666;
}
</style>