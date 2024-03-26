<script setup>
import UserProfileMessage from "../components/UserProfileMessage.vue"
import LoaderComponent from "../components/LoaderComponent.vue"
import UserProfile from "../components/UserProfile.vue"
import MessageBox from "../components/MessageBox.vue"
import MessageInput from "../components/MessageInput.vue"
import signalRService from '../services/SignalRService.js';
import axios from "axios";
import store from '../store.js'
</script>

<template>
  <div class="wrapper" v-if="connected">
    <div class="flex">
      <MessageBox id="messagebox"/>
    </div>
    <div class="flex">
      <!-- <UserProfileMessage /> -->
      <UserProfile id="userprofil"/>
      <MessageInput id="messageinput"/>
    </div>
  </div>
  <div v-else>
    <LoaderComponent />
    {{ connected }}
  </div>
</template>

<script>
export default {
  async mounted() {
    signalRService.configureConnection(); 
    await signalRService.connect();
    this.connected = signalRService.connected;
  },
  data() {
    return {
      connected: false,
    }
  },
}
</script>

<style scoped>
* {
	margin: 0;
	padding: 0;
  box-sizing: border-box;
  overflow: hidden;
}
.wrapper {
  background: rgb(148, 147, 147);
}
.flex {
  display: flex;
  align-items: center;
}
#messageinput, #messagebox {
  flex-grow: 1;
}
</style>
