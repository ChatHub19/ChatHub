<script setup>
import chatService from '../services/ChatService.js';
</script>

<template>
  <div class="wrapper">
    <div class="container">
      <nav>
        <input type="checkbox" id="active">
        <label for="active" class="menu-btn"> 
          <font-awesome-icon icon="fa-solid fa-bars" id="icon"/> 
        </label>
        <div class="listwrapper">
          <label class="listname"> User </label>
          <ul v-for="(value, key) in userlists" :key="key">
            <li class="displayname" v-if="key !== displayname" @click="selectUser(key, value)"> 
              {{ key }}
            </li> 
          </ul>
          <ul v-for="(value, key) in userlists" :key="key">
            <li class="displayname" v-if="key !== displayname" @click="selectUser(key, value)"> 
              {{ key }}
            </li> 
          </ul>
          <ul v-for="(value, key) in userlists" :key="key">
            <li class="displayname" v-if="key !== displayname" @click="selectUser(key, value)"> 
              {{ key }}
            </li> 
          </ul>
          <ul v-for="(value, key) in userlists" :key="key">
            <li class="displayname" v-if="key !== displayname" @click="selectUser(key, value)"> 
              {{ key }}
            </li> 
          </ul>
          <ul v-for="(value, key) in userlists" :key="key">
            <li class="displayname" v-if="key !== displayname" @click="selectUser(key, value)"> 
              {{ key }}
            </li> 
          </ul>
          <ul v-for="(value, key) in userlists" :key="key">
            <li class="displayname" v-if="key !== displayname" @click="selectUser(key, value)"> 
              {{ key }}
            </li> 
          </ul>
          <ul v-for="(value, key) in userlists" :key="key">
            <li class="displayname" v-if="key !== displayname" @click="selectUser(key, value)"> 
              {{ key }}
            </li> 
          </ul>
        </div>
      </nav>
    </div>
  </div>
</template>

<script>
export default {
  async mounted() {
    try { chatService.subscribeEvent("ReceiveConnectedUsers", this.onUsersReceived); } 
    catch (e) { console.log(e); }    
  },
  async unmounted() {
    chatService.unsubscribeEvent("ReceiveConnectedUsers", this.onUsersReceived);
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
    async onUsersReceived(value) {
      // console.log(value.admin[0]); get connectionid
      // console.log(value.admin[1]); get userguid
      this.userlists = value;
    },
    selectUser(key, value) {
      this.$router.push(`/chatroom/${key}`);
      // alert(value[0]);
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
/* .userlist {
  padding: 10px;
  overflow-y: auto;
} */
label {
  padding: 5px;
  border-radius: 5px;
  font-weight: bold;
  color: white;
}
li {
  padding: 5px;
}
.displayname {
  font-weight: normal;
}
/* responsive */
.listwrapper {
  position: fixed;
  top: 0;
  left: 0;
  height: 100%;
  width: 100%;
  background: #302c34;
  clip-path: circle(25px at calc(45px) 45px);
  transition: all 0.3s ease-in-out;
  z-index: 1;
  overflow: auto;
}
#active:checked ~ .listwrapper{
  clip-path: circle(75%);
}
.menu-btn{
  position: absolute;
  z-index: 2;
  left: 20px;
  top: 20px;
  height: 50px;
  width: 50px;
  text-align: center;
  line-height: 40px;
  border-radius: 50%;
  font-size: 20px;
  color: white;
  cursor: pointer;
  background: #302c34;
  transition: all 0.3s ease-in-out;
}
#active:checked ~ .menu-btn{
  background: white;
  color: black;
}
#active:checked ~ .menu-btn #icon:before{
  content: "\f00d";
}
.listwrapper ul{
  display: flex;
}
.listwrapper ul li {
  color: none;
  text-decoration: none;
  font-size: 26px;
  font-weight: 500;
  margin: 15px 0;
  padding: 5px 30px;
  color: white;
  position: relative;
  line-height: 50px;
  transition: all 0.3s ease;
}
.listwrapper ul li:hover {
  color: blueviolet;
  cursor: pointer;
}
.listname {
  text-decoration: underline;
  margin: 100px 0 20px 0;
  padding: 5px 30px;
  font-size: 30px;
  font-weight: bolder;
  color: lightgrey;
}
#active {
  display: none;
}
</style>