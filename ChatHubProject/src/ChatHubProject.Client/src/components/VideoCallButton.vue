<script setup>
import videoService from "../services/VideoService.js";
</script>

<template>
  <div class="wrapper">
    <div class="video" @click="SendVideoCall()">
      <font-awesome-icon icon="fa-solid fa-video" />
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      isVideoCallActive: false,
    };
  },
  methods: {
    // async ToggleVideoCall() {
    //   if (this.isVideoCallActive) {
    //     await this.EndVideoCall();
    //   } else {
    //     await this.SendVideoCall();
    //   }
    //   this.isVideoCallActive = !this.isVideoCallActive;
    // },
    async SendVideoCall() {    
      const localStream = await navigator.mediaDevices.getUserMedia({ video: true, audio: true });
      const remoteStream = new MediaStream();
      const pc = new RTCPeerConnection();

      videoService.sendVideoCallToAll(localStream);
      
      localStream.getTracks().forEach((track) => {
        pc.addTrack(track, localStream);
      });

      pc.ontrack = (event) => {
        event.streams[0].getTracks().forEach((track) => {
          remoteStream.addTrack(track);
        });
      };
    },
    // async EndVideoCall() {
    //   const localStream = await navigator.mediaDevices.getUserMedia({
    //     video: false,
    //     audio: false,
    //   });
    //   alert(localStream);
    // },
    async onVideoCallReceived(videoData) {
      console.log(videoData);
    },
  },
}
</script>

<style scoped>
canvas {
  width: 300px;
  height: 300px;
  border: solid 1px black;
  background: white;
}
</style>