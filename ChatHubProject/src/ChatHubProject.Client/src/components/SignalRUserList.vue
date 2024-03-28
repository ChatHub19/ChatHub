<script setup>
import signalRService from '../services/SignalRService.js';
</script>

<template>
  <div class="wrapper">
    <div class="container">
      <div class="userlist">
        <span> User </span>
        <div v-for="(user, index) in userlists" :key="index" class="list">
          <p 
            class="displayname" 
            v-if="user !== displayname"
            @click="selectUser(user)"
          > 
          {{ user }} 
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
  unmounted() {
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
      if(this.userlists.some(u => u === user)) return;
      this.userlists = user;
    },
    selectUser(user) {
      alert(user);
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
  background: #4a4a4a;
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
}
.list:hover {
  cursor: pointer;
  background: grey;
}
</style>