<script setup>
import signalRService from '../services/SignalRService.js';
</script>

<template>
  <div class="wrapper">
    <div class="userlist">
        <div v-for="(user, index) in userlists" :key="index" class="message">
          <p> {{ user }} </p> 
        </div>
      </div>
  </div>
</template>

<script>
export default {
  async mounted() {
    try { signalRService.subscribeEvent("ReceiveConnectedUsers", this.onUserReceived); } 
    catch (e) { console.log(e); }    
  },
  unmounted() {
    signalRService.unsubscribeEvent("ReceiveConnectedUsers", this.onUserReceived);
  }, 
  data() {
    return {
      userlists: [],
    }
  }, 
  methods: {
    async onUserReceived(user) {
      this.userlists = user;
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
</style>