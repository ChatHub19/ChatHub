<script setup>
import signalRService from '../services/SignalRService.js';
</script>

<template>
    <div class="wrapper">
        <button @click="editMessage()">edit</button>
    </div>
</template>

<script>
export default {
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

<style>

button
{
    background: rgba(61, 62, 63, 0.681);
    color: white;
    border-radius: 20px;
}
</style>