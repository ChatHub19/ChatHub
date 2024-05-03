<script setup>
import LoaderComponent from "../components/LoaderComponent.vue"
import SignalRUserList from "../components/SignalRUserList.vue"
import UserProfile from "../components/UserProfile.vue"
import MessageBox from "../components/MessageBox.vue"
import MessageInput from "../components/MessageInput.vue"
import chatService from '../services/ChatService.js';
</script>

<template>
  <div class="wrapper" v-if="connected">
    <div class="flex">
      <SignalRUserList id="userlist"/>
      <!-- Todo: Replace MessageBox with PrivateMessageBox component -->
      <MessageBox id="messagebox"/>
    </div>
    <div class="flex">
      <UserProfile id="userprofil"/>
      <!-- Todo: Replace MessageInput with PrivateMessageInput component -->
      <MessageInput id="messageinput"/>
    </div>
  </div>
  <div v-else>
    <LoaderComponent />
  </div>
</template>

<script>
export default {
  async mounted() {
    chatService.configureConnection(); 
    await chatService.connect();
    this.connected = chatService.connected;
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
