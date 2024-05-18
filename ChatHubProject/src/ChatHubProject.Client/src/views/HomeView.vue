<script setup>
import LoaderComponent from "../components/LoaderComponent.vue";
import ServerComponent from "../components/ServerComponent.vue";
import SignalRList from "../components/SignalRList.vue";
import NavMenu from "../components/NavMenu.vue";
import UserProfile from "../components/UserProfile.vue";
import MessageBox from "../components/MessageBox.vue";
import MessageInput from "../components/MessageInput.vue";
import chatService from "../services/ChatService.js";
import videoService from "../services/VideoService.js";
import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';
</script>

<template>
  <div class="wrapper" v-if="connected">
    <div class="flex">
      <ServerComponent id="server" />
      <SignalRList id="userlist"/>
      <NavMenu id="navmenu" />
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
    toast.success("Success")
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
.video {
  color: white;
  cursor: pointer;
  position: absolute;
  right: 10px; 
  top: 50%;
  transform: translateY(-50%);
}
#navmenu {
  display: none;
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
    display: none;
  }
}
</style>
