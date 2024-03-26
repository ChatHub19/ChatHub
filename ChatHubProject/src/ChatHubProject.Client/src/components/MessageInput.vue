<script setup>
import axios from "axios";
import store from '../store.js'
import signalRService from '../services/SignalRService.js';
</script>

<template>
  <div class="wrapper">
    <div class="flex">
      <div class="message-container">
        <input type="text" v-model="message" placeholder="Send Message" @keypress.enter="SendMessage()">
      </div>
    </div>
  </div>
</template>

<script>
export default {
  computed: {
    guid() {
      return this.$store.state.userdata.userGuid
    }
  }, 
  data() {
    return {
      message: "",
    }
  }, 
  methods: {
    async SendMessage() {
      if (this.message !== "") { 
        const text = this.message;
        const displayname = (await axios.get(`/user/${this.guid}`)).data.displayname
        const time = new Date().toLocaleTimeString();
        const userguid = this.guid; 
        (await axios.post("message/send", {text, userguid, time}));
        signalRService.sendMessageToAll(text, displayname, time);
      }
      this.message = "";
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