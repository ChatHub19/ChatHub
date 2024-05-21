<script setup>
import chatService from '../services/ChatService.js';
import videoService from '../services/VideoService.js';
import axios from "axios";
import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';
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
          <div class="flex">
            <input type="checkbox" :name="'editbox' + index" :id="'editbox' + index">
            <p>{{ message.text }}</p>
            <input type="text" @keyup.enter=editMessage(message,index) :id="'editmessage' + index" v-model="editmessage">  
            <button class="edit" :id="'edit' + index" @click="showInput(index)">edit</button>
            <button :id="'delete' + index" @click=deleteMessage(message)>delete</button>
          </div>
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
      editmessage: ""
      
    }
  }, 
   
  computed: {
    guid() {
      return this.$store.state.userdata.userGuid;
    },

  },
  methods: {
    async onMessageReceived(text, displayname, time) {
      if(displayname === undefined) { displayname = "System"; }
      if(time === undefined) { time = new Date().toLocaleDateString(); }
      var messagedata = (await axios.get("message")).data
      this.messages=messagedata 
    },
     showInput(index){
      document.getElementById(`editbox${index}`).checked = !document.getElementById(`editbox${index}`).checked;
    },
    async editMessage(message,index){
      message.text=this.editmessage
      try {await axios.put(`/message/${message.guid}`, message) }
      catch(e) { toast.error(e.response.data) }
      var messagedata = (await axios.get("message")).data
      this.messages=messagedata 
      document.getElementById(`editbox${index}`).checked = false
      this.editmessage=""

    },
    async deleteMessage(message) {
      try { 
        await axios.delete(`/message/${message.guid}`) 
      }
      catch(e) { toast  .error(e.response.data) }
      var messagedata = (await axios.get("message")).data
      this.messages=messagedata 
    },
  
     showInput(index){
      document.getElementById(`editbox${index}`).checked = !document.getElementById(`editbox${index}`).checked;
    },
    async editMessage(message,index){
      message.text=this.editmessage
      try {await axios.put(`/message/${message.guid}`, message) }
      catch(e) { toast.error(e.response.data) }
      var messagedata = (await axios.get("message")).data
      this.messages=messagedata 
      document.getElementById(`editbox${index}`).checked = false
      this.editmessage=""

    },
    async deleteMessage(message) {
      try { 
        await axios.delete(`/message/${message.guid}`) 
      }
      catch(e) { toast  .error(e.response.data) }
      var messagedata = (await axios.get("message")).data
      this.messages=messagedata 
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
button
{
    background: rgba(61, 62, 63, 0.681);
    color: white;
    border-radius: 20px;
    padding: 5px;
}
.edit{
 margin-left: auto;
}
input{
background: transparent;
color: white;

}
#editmessage,input{
  display: none;
}
input[type="checkbox"]:checked ~ p {
  display: none;
}
input[type="checkbox"]:checked ~ input {
  display: block;
}
button
{
    background: rgba(61, 62, 63, 0.681);
    color: white;
    border-radius: 20px;
    padding: 5px;
}
.edit{
 margin-left: auto;
}
input{
background: transparent;
color: white;

}
#editmessage,input{
  display: none;
}
input[type="checkbox"]:checked ~ p {
  display: none;
}
input[type="checkbox"]:checked ~ input {
  display: block;
}
</style>