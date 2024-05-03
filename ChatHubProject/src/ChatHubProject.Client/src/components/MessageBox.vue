<script setup>
import chatService from '../services/ChatService.js';
import videoService from '../services/VideoService.js';
</script>

<template>
  <div class="wrapper">
    <div class="flex flexbox">
      <div class="message-box">
        <div v-for="(message, index) in messages" :key="index" class="message">
          <div class="display-content">
            <p class="displayname"> {{ message.displayname }} </p>
            <p class="time"> {{ message.time }} </p>
          </div>
          <p> {{ message.text }} </p> 
        </div>
      </div>
      <div class="camera-box">
        <font-awesome-icon id="webcam-btn" icon="fa-solid fa-video" @click="sendVideoCallOffer()" style="color: white; cursor: pointer;"/>
        <h3> Your Camera </h3>
        <video id="webcam" autoplay playsinline muted></video>
        <font-awesome-icon id="remote-btn" icon="fa-solid fa-video" @click="sendVideoCallOffer()" style="color: white; cursor: pointer;"/>
        <h3> Friend Camera </h3>
        <video id="remote" autoplay playsinline></video>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  async mounted() {
    try { 
      chatService.sendJoinedMessageToAll();
      chatService.subscribeEvent("ReceiveMessage", this.onMessageReceived); 
      chatService.subscribeEvent("ReceiveJoinedMessage", this.onMessageReceived); 
    } 
    catch (e) { console.log(e); }    
  },
  async unmounted() {
    chatService.unsubscribeEvent("ReceiveMessage", this.onMessageReceived);
    chatService.unsubscribeEvent("ReceiveJoinedMessage", this.onMessageReceived);
  }, 
  data() {
    return {
      messages: [],
    }
  }, 
  methods: {
    async onMessageReceived(text, displayname, time) {
      if(displayname === undefined) { displayname = "System"; }
      if(time === undefined) { time = new Date().toLocaleDateString(); }
      this.messages.push({text, displayname, time}); 
    },
    async sendVideoCallOffer() {
      const webcamVideo = document.getElementById("webcam");
      const remoteVideo = document.getElementById("remote");

      const webcamButton = document.getElementById("webcam-btn");
      const remoteButton = document.getElementById("remote-btn");

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
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}
h3 {
  color: white;
}
#webcam, #remote {
  transform: scaleX(-1);
}
.flex {
  display: flex;
  align-items: center;
}
.message-box {
  width: 100%;
  height: 89.1vh;
  overflow-y: auto;
  padding: 10px;
  background: #38343c;
}
.display-content {
  display: flex;
  align-items: center;
}
.displayname {
  font-weight: bold;
  margin-right: 10px;
  color: white;
}
.message {
  margin-bottom: 10px;
  padding: 5px;
  border-radius: 5px;
  color: white;
}
.time {
  float: right;
  font-size: 12px;
  color: white;
}
</style>