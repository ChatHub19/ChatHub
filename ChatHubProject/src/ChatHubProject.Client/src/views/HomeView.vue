<script setup>
import LoaderComponent from "../components/LoaderComponent.vue";
import ServerComponent from "../components/ServerComponent.vue";
import SignalRUserList from "../components/SignalRUserList.vue";
import UserProfile from "../components/UserProfile.vue";
import MessageBox from "../components/MessageBox.vue";
import MessageInput from "../components/MessageInput.vue";
import chatService from "../services/ChatService.js";
import videoService from "../services/VideoService.js";
</script>

<template>
  <div class="wrapper" v-if="connected">
    <div class="flex">
      <ServerComponent id="server" />
      <SignalRUserList id="userlist" />
      <MessageBox id="messagebox" />
    </div>
    <div class="flex">
      <UserProfile id="userprofil" />
      <MessageInput id="messageinput" />
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
    videoService.configureConnection();
    await videoService.connect();
    this.connected = chatService.connected, videoService.connected;
  },
  data() {
    return {
      connected: false,
    };
  },
};
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
#messageinput,
#messagebox {
  flex-grow: 1;
  overflow: hidden;
  margin-right: 10px;
}
</style>