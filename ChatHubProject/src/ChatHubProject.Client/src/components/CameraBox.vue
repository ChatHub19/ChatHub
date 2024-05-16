<template>
  <div class="camera-box">
    <div class="video-container">
      <div class="flex">
        <h3> Your Camera </h3>
        <video id="webcam" autoplay playsinline muted></video>
      </div>
      <div class="flex" id="a">
        <h3> Friend Camera </h3>
        <video id="remote" autoplay playsinline></video>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  async mounted() {
    await this.sendVideoCallOffer();
  },
  methods: {
    async sendVideoCallOffer() {   
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
  },
};
</script>

<style scoped>
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
  height: 90vh;
  max-width: 74vw;
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
@media only screen and (max-width: 769px) {
  .video-container {
    flex-direction: column;
  }
  video {
    width: 50%;
    height: 50%;
  }
}
</style>