<script setup>
import signalRService from '../services/SignalRService.js';
import EditMessageButton from './EditMessageButton.vue';
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
      signalRService.sendJoinedMessageToAll();
      signalRService.subscribeEvent("ReceiveMessage", this.onMessageReceived); 
      signalRService.subscribeEvent("ReceiveJoinedMessage", this.onMessageReceived); 
    } 
    catch (e) { console.log(e); }    
  },
  async unmounted() {
    signalRService.unsubscribeEvent("ReceiveMessage", this.onMessageReceived);
    signalRService.unsubscribeEvent("ReceiveJoinedMessage", this.onMessageReceived);
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
    }
  }
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
  height: 89.1vh;
  overflow-y: auto;
  padding: 10px;
  background: rgba(61, 62, 63, 0.681);
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
#editMessageButton{
 font-weight: bold;
 margin-left: auto;

}
</style>