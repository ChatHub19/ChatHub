<script setup>
import axios from "axios"
import LoaderComponent from "../components/LoaderComponent.vue";
import FriendRequest from "../components/FriendRequest.vue";
import ServerComponent from "../components/ServerComponent.vue";
import SignalRUserList from "../components/SignalRUserList.vue";
import UserProfile from "../components/UserProfile.vue";
import MessageBox from "../components/MessageBox.vue";
import MessageInput from "../components/MessageInput.vue";
import signalRService from "../services/SignalRService.js";
import FriendRequest from '../component/FriendRequest.vue'
</script>

<template>
  <div class="wrapper" v-if="connected">
    <div class="flex">
      <!-- <ServerComponent id="server" /> -->
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
    await this.getUserdata();
    signalRService.configureConnection();
    await signalRService.connect();
    this.connected = signalRService.connected;
  },
  data() {
    return {
      connected: false,
    };
  },
  methods: {
    async getUserdata() {
      try {
        var userdata = (await axios.get("user/userinfo")).data
        this.$store.commit("authenticate", userdata)
      } 
      catch (e) { e.response.data }
    },
  }
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
