<script setup>
import LoaderComponent from "../components/LoaderComponent.vue"
import SignalRList from "../components/SignalRList.vue"
import ServerComponent from "../components/ServerComponent.vue";
import VideoCallButton from '../components/VideoCallButton.vue';
import UserProfile from "../components/UserProfile.vue"
import MessageBox from "../components/MessageBox.vue"
import MessageInput from "../components/MessageInput.vue"
import chatService from '../services/ChatService.js';
</script>

<template>
  <div class="wrapper" v-if="connected">
    <div class="flex">
      <ServerComponent id="server" />
      <SignalRList id="userlist"/>
      <!-- Todo: Replace MessageBox with PrivateMessageBox component -->
      <MessageBox id="messagebox"/>
    </div>
    <div class="flex">
      <UserProfile id="userprofil"/>
      <!-- Todo: Replace MessageInput with PrivateMessageInput component -->
      <MessageInput id="messageinput"/>
    </div>
    <div class="flex">
      <VideoCallButton id="videocallbtn"/>
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
}
.wrapper {
  background: #38343c;
}
.flex {
  display: flex;
  align-items: center;
}
.video {
  color: white;
  cursor: pointer;
  position: absolute;
  right: 10px; 
  top: 50%;
  transform: translateY(-50%);
}
#messageinput, #messagebox {
  flex-grow: 1;
  overflow: hidden;
  margin-right: 10px;
}
@media screen and (max-width: 769px) {
  #navmenu {
    display: flex;
  }
  #userlist {
    height: 0;
    width: 0;
  }
}
</style>
