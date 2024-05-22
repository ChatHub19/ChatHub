<script setup>
import LoaderComponent from "../components/LoaderComponent.vue";
import ServerComponent from "../components/ServerComponent.vue";
import SignalRList from "../components/SignalRList.vue";
import UserProfile from "../components/UserProfile.vue";
import MessageBox from "../components/MessageBox.vue";
import MessageInput from "../components/MessageInput.vue";
import signalRService from "../services/SignalRService.js";
import ChannelComponent from "../components/ChannelComponent.vue";
import chatService from "../services/ChatService.js";
</script>

<template>
  <div class="wrapper" v-if="connected">
    <div class="flex">
      <ServerComponent id="server" />
      <ChannelComponent id="channel" />
      <!-- <SignalRUserList id="userlist" /> -->
      <SignalRList id="userlist" />
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
    this.connected = chatService.connected;
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
}
</style>
