<template>
  <div class="wrapper">
    <div class="video">
      <input type="checkbox" class="videocheckbox" name="videocheck" id="videocheck">
      <label for="videocheck" id="videostartlabel" @click="startCall()">
        <font-awesome-icon class="icon" icon="fa-solid fa-video" />
      </label>
      <label for="videocheck" id="videoendlabel" @click="hangupCall()">
        <font-awesome-icon class="icon" icon="fa-solid fa-person-walking-arrow-right" />
      </label>
    </div>
    <div class="camera-box" v-show="videocheck">
      <div class="video-container">
        <div class="flex">
          <h3> Your Camera </h3>
          <video id="webcam" ref="webcam" autoplay playsinline muted></video>
        </div>
        <div class="flex" id="a">
          <h3> Friend Camera </h3>
          <video id="remote" ref="remote" autoplay playsinline></video>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      localStream: null,
      pc: null,
      signaling: null,
      videocheck: false
    };
  },
  mounted() {
    this.signaling = new BroadcastChannel('webrtc');
    this.signaling.onmessage = this.handleSignalingMessage;
  },
  methods: {
    async startCall() {
      try {
        this.videocheck = true;
        this.localStream = await navigator.mediaDevices.getUserMedia({ audio: true, video: true });
        this.$refs.webcam.srcObject = this.localStream;
        this.signaling.postMessage({ type: 'ready' });
      } catch (error) {
        this.videocheck = false;
        console.error('Error accessing media devices.', error);
      }
    },
    async hangupCall() {
      this.videocheck = false;
      this.hangup();
      this.signaling.postMessage({ type: 'bye' });
    },
    async hangup() {
      if (this.pc) {
        this.pc.close();
        this.pc = null;
      }
      if (this.localStream) {
        this.localStream.getTracks().forEach(track => track.stop());
        this.localStream = null;
      }
    },
    async createPeerConnection() {
      this.pc = new RTCPeerConnection();
      this.pc.onicecandidate = (e) => {
        const message = { type: 'candidate', candidate: null };
        if (e.candidate) {
          message.candidate = e.candidate.candidate;
          message.sdpMid = e.candidate.sdpMid;
          message.sdpMLineIndex = e.candidate.sdpMLineIndex;
        }
        this.signaling.postMessage(message);
      };
      this.pc.ontrack = (e) => this.$refs.remote.srcObject = e.streams[0];
      this.localStream.getTracks().forEach(track => this.pc.addTrack(track, this.localStream));
    },
    async makeCall() {
      await this.createPeerConnection();

      const offer = await this.pc.createOffer();
      this.signaling.postMessage({ type: 'offer', sdp: offer.sdp });
      await this.pc.setLocalDescription(offer);
    },
    async handleOffer(offer) {
      if (this.pc) {
        console.error('existing peerconnection');
        return;
      }
      await this.createPeerConnection();
      await this.pc.setRemoteDescription(offer);

      const answer = await this.pc.createAnswer();
      this.signaling.postMessage({ type: 'answer', sdp: answer.sdp });
      await this.pc.setLocalDescription(answer);
    },
    async handleAnswer(answer) {
      if (!this.pc) {
        console.error('no peerconnection');
        return;
      }
      await this.pc.setRemoteDescription(answer);
    },
    async handleCandidate(candidate) {
      if (!this.pc) {
        console.error('no peerconnection');
        return;
      }
      if (!candidate.candidate) {
        await this.pc.addIceCandidate(null);
      } else {
        await this.pc.addIceCandidate(candidate);
      }
    },
    handleSignalingMessage(e) {
      if (!this.localStream) {
        console.log('not ready yet');
        return;
      }
      switch (e.data.type) {
        case 'offer':
          this.handleOffer(e.data);
          break;
        case 'answer':
          this.handleAnswer(e.data);
          break;
        case 'candidate':
          this.handleCandidate(e.data);
          break;
        case 'ready':
          if (this.pc) {
            console.log('already in call, ignoring');
            return;
          }
          this.makeCall();
          break;
        case 'bye':
          if (this.pc) {
            this.hangup();
          }
          break;
        default:
          console.log('unhandled', e);
          break;
      }
    }
  }
};
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
h3 {
  padding:  20px 0;
  color: white;
}
video {
  width: 100%;
  height: 100%;
  transform: scaleX(-1);
}
.camera-box {
  position: absolute;
  top: 0;
  background: #38343c;
  height: 89.1vh;
  width: 74vw;
  margin-left: 25vw;
}
.video-container {
  display: flex;
}
.flex {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}
.videocheckbox, #videoendlabel, #videocheck:checked ~ #videostartlabel {
  display: none;
}
#videocheck:checked ~ #videoendlabel {
  display: block;
}
@media screen and (max-width: 769px) {
  .video {
    bottom: 38px;
  }
  .video-container {
    flex-direction: column;
  }
  .camera-box {
    height: 85.3vh;
    margin-left: 10vw;
  }
  video {
    width: 50%;
    height: 50%;
  }
}
</style>