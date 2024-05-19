<script setup>
import CameraBox from "./CameraBox.vue";
</script>

<template>
  <div class="wrapper">
    <div class="video" @click="toogleVideoCall()">
      <font-awesome-icon class="icon" icon="fa-solid fa-video" />
    </div>
    <CameraBox v-if="isVideoCallActive"/>
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
    async toogleVideoCall() {
      this.isVideoCallActive = !this.isVideoCallActive;
    },
  }
}
</script>

<style scoped>
.video {
  position: absolute;
  right: 30px;
  bottom: 25px;
}
.icon {
  color: white;
  cursor: pointer;
}
@media screen and (max-width: 769px) {
  .video {
    bottom: 38px;
  }
}
</style>