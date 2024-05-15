<script setup>
import signalRService from '../services/SignalRService.js';
import FriendRequest from "../components/FriendRequest.vue";
</script>

<template>
  <div class="wrapper">
    <div class="container">
      <div class="userlist">
        <span> User </span>
        <div v-for="(value, key) in userlists" :key="key" class="list">
          <p class="displayname" v-if="key !== displayname" @click="selectUser(value, key)"> 
          {{ key }}
          </p> 
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  async mounted() {
    try { signalRService.subscribeEvent("ReceiveConnectedUsers", this.onUsersReceived); } 
    catch (e) { console.log(e); }    
  },
  async unmounted() {
    signalRService.unsubscribeEvent("ReceiveConnectedUsers", this.onUsersReceived);
  }, 
  computed: {
    displayname() { 
      return this.$store.state.userdata.displayname;
    }
  },
  data() {
    return {
      userlists: [],
    }
  }, 
  methods: {
    async onUsersReceived(user) {
      this.userlists = user;
    },
    selectUser(key, value) {
      this.$router.push(`/chatroom/${key}`);
      alert(value[0]);
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
.container {
  width: 20vw;
  height: 89.1vh;
  padding: 10px;
  background: #302c34;
  color: white;
}
.userlist {
  padding: 10px;
  overflow-y: auto;
}
span {
  padding: 5px;
  border-radius: 5px;
  font-weight: bold;
  color: white;
}
p {
  padding: 5px;
}
.displayname {
  font-weight: normal;
  display: flex;
  justify-content: center;
  align-items: center;
}
#friendrequest {
  margin-left: auto;
}
</style>