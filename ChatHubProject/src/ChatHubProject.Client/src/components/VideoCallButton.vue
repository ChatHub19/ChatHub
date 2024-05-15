<script setup>
import videoService from "../services/VideoService.js";
</script>

<template>
  <div class="wrapper">
    <div class="video" @click="sendVideoCallOffer()">
      <font-awesome-icon class="icon" icon="fa-solid fa-video" />
    </div>
    <!-- <div class="camera-box">
      <div class="video-container">
        <div>
          <h3> Your Camera </h3>
          <video id="webcam" autoplay playsinline muted></video>
        </div>
        <div>
          <h3> Other Camera </h3>
          <video id="remote" autoplay playsinline></video>
        </div>
      </div>
    </div> -->
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
    async ToggleVideoCall() {
      if (this.isVideoCallActive) {
        await this.EndVideoCall();
      } else {
        await this.SendVideoCall();
      }
      this.isVideoCallActive = !this.isVideoCallActive;
    },
    async sendVideoCallOffer() {
      this.ToggleVideoCall();
      const webcamVideo = document.getElementById("webcam");
      const remoteVideo = document.getElementById("remote");

      let pc;
      let localStream;

      const signaling = new BroadcastChannel('webrtc'); 
      signaling.onmessage = e => {
        if(!localStream) {
          alert("not ready yet");
          return;
        }
        switch (e.data.type) {
          case 'offer':
            handleOffer(e.data);
            break;
          case 'answer':
            handleAnswer(e.data);
            break;
          case 'candidate':
            handleCandidate(e.data);
            break;
          case 'ready':
            if (pc) {
              alert('already in call, ignoring');
              return;
            }
            makeCall();
            break;
          case 'bye':
            if (pc) {
              hangup();
            }
            break;
          default:
            console.log('unhandled', e);
            break;
        }
      }
      localStream = await navigator.mediaDevices.getUserMedia({audio: true, video: true});
      webcamVideo.srcObject = localStream;
      signaling.postMessage({type: 'ready'});
      // videoService.sendVideoCallToAll(localStream);
      // localStream.getTracks().forEach((track) => { pc.addTrack(track, localStream); });
      // pc.ontrack = (event) => {
      //   event.streams[0].getTracks().forEach((track) => {
      //     remoteStream.addTrack(track);
      //   });
      // };
      async function hangup() {
        if (pc) {
          pc.close();
          pc = null;
        }
        localStream.getTracks().forEach(track => track.stop());
        localStream = null;
      }

      function createPeerConnection() {
        pc = new RTCPeerConnection();
        pc.onicecandidate = e => {
          const message = {
            type: 'candidate',
            candidate: null,
          };
          if (e.candidate) {
            message.candidate = e.candidate.candidate;
            message.sdpMid = e.candidate.sdpMid;
            message.sdpMLineIndex = e.candidate.sdpMLineIndex;
          }
          signaling.postMessage(message);
        };
        pc.ontrack = e => remoteVideo.srcObject = e.streams[0];
        localStream.getTracks().forEach(track => pc.addTrack(track, localStream));
      }

      async function makeCall() {
        createPeerConnection();

        const offer = await pc.createOffer();
        signaling.postMessage({type: 'offer', sdp: offer.sdp});
        await pc.setLocalDescription(offer);
      }

      async function handleOffer(offer) {
        if (pc) {
          console.error('existing peerconnection');
          return;
        }
        createPeerConnection();
        await pc.setRemoteDescription(offer);

        const answer = await pc.createAnswer();
        signaling.postMessage({type: 'answer', sdp: answer.sdp});
        await pc.setLocalDescription(answer);
      }

      async function handleAnswer(answer) {
        if (!pc) {
          console.error('no peerconnection');
          return;
        }
        await pc.setRemoteDescription(answer);
      }

      async function handleCandidate(candidate) {
        if (!pc) {
          console.error('no peerconnection');
          return;
        }
        if (!candidate.candidate) {
          await pc.addIceCandidate(null);
        } else {
          await pc.addIceCandidate(candidate);
        }
      }
    },
  }
}
</script>

<style scoped>
h3 {
  padding: 20px;
  color: white;
}
video {
  margin-left: 20px;
  width: 95%;
}
.video {
  position: absolute;
  right: 30px;
  bottom: 25px;
}
.icon {
  color: white;
  cursor: pointer;
}
.camera-box {
  position: absolute;
  top: 0;
  background: #38343c;
  height: 90vh;
  width: 74vw;
  margin-left: 25vw;
}
.video-container {
  display: flex;
  justify-content: space-between;
}
.video-container > div {
  flex: 1;
}
</style>