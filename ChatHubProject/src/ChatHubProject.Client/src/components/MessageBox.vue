<script setup>
import chatService from '../services/ChatService.js';
import videoService from '../services/VideoService.js';
</script>

<template>
  <div class="wrapper">
    <div class="flex flexbox">
      <div class="message-box">
        <div v-for="(message, index) in messages" :key="index" class="message">
          <div class="display-content">
            <p class="displayname"> {{ message.displayname }} </p>
            <p class="time"> {{ message.time }} </p>
          </div>
          <div class="flex"> <p> {{ message.text }}</p><EditMessageButton id="editMessageButton"/> </div>
         
           
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  async mounted() {
    try { 
      chatService.sendJoinedMessageToAll();
      chatService.subscribeEvent("ReceiveMessage", this.onMessageReceived); 
      chatService.subscribeEvent("ReceiveJoinedMessage", this.onMessageReceived); 
    } 
    catch (e) { console.log(e); }    
  },
  async unmounted() {
    chatService.unsubscribeEvent("ReceiveMessage", this.onMessageReceived);
    chatService.unsubscribeEvent("ReceiveJoinedMessage", this.onMessageReceived);
  }, 
  data() {
    return {
      messages: [],
    }
  }, 
  methods: {
    async onMessageReceived(text, displayname, time) {
      if(displayname === undefined) { displayname = "System"; }
      if(time === undefined) { time = new Date().toLocaleDateString(); }
      this.messages.push({text, displayname, time}); 
    },
  }
}
</script>

<style scoped>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}
h3 {
  color: white;
}
#webcam, #remote {
  transform: scaleX(-1);
}
.flex {
  display: flex;
  align-items: center;
}
.message-box {
  width: 100%;
  height: 89.1vh;
  overflow-y: auto;
  padding: 10px;
  background: #38343c;
}
.display-content {
  display: flex;
  align-items: center;
}
.displayname {
  font-weight: bold;
  margin-right: 10px;
  color: white;
}
.message {
  margin-bottom: 10px;
  padding: 5px;
  border-radius: 5px;
  color: white;
}
.time {
  float: right;
  font-size: 12px;
  color: white;
}
@media screen and (max-width: 769px) { 
  .message-box {
    height: 85.3vh;
  }
}
</style>