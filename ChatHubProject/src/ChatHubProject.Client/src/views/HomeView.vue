<script setup>
import UserProfileMessage from "../components/UserProfileMessage.vue"
import ServerMessageBox from "../components/ServerMessageBox.vue"
</script>

<template>
  <div class="wrapper">
    <!-- <div class="message-box">
      <ServerMessageBox />
    </div> -->
    <div id="userprofilmessage">
      <UserProfileMessage />
    </div>
  </div>
</template>

<script>
export default {

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
  width: 100vw;
  height: 100vh;
  background: rgb(148, 147, 147);
}
#message-box {
  position: relative;
}
#userprofilmessage {
  position: relative;
  top: 9%;
}
</style>
