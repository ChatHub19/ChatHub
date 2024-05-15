<script setup>
import axios from "axios";
import chatService from "../services/ChatService.js";

</script>

<template>
  <div class="wrapper">
    <div class="flex">
      <div class="message-container">
        <input
          type="text"
          v-model="message"
          placeholder="Enter Message"
          @keypress.enter="SendMessage()"
        />
      </div>
    </div>
  </div>
</template>

<script>
export default {
  computed: {
    guid() {
      return this.$store.state.userdata.userGuid;
    },
  },
  data() {
    return {
      message: "",
    };
  },
  methods: {
    async SendMessage() {
      if (this.message !== "") {
        const text = this.message;
        const displayname = (await axios.get(`/user/${this.guid}`)).data
          .displayname;
        const time = new Date().toLocaleTimeString();
        const userguid = this.guid;
        await axios.post("message/send", { text, userguid, time });
        chatService.sendMessageToAll(text, displayname, time);
      }
      this.message = "";
    },
  },
};
</script>

<style scoped>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}
input {
  width: 99%;
  margin: 0 10px;
  color: black;
  padding: 10px;
  border-radius: 10px;
  border: 0;
  background: #403c44;
}

input:focus {
  color: white;
  outline: none;
}

input:focus::placeholder {
  outline: none;
  color: gray;
  color: transparent;
}
.flex {
  display: flex;
  align-items: center;
}
.message-container {
  width: 100%;
  position: relative;
}
</style>