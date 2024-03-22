<script setup>
import axios from "axios";
import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';
</script>


<template>
  <div>
    <button @click="openModal()" @keyup.enter="openModal()"> 
      Change Password
    </button>
    <div v-if="showModal" class="modal">
      <div class="modal-content">
        <span class="close" @click="closeModal()">&times;</span>
        <h6> Password </h6> 
        <input type="password" v-model="accountModel.password">
        <h6> New Password </h6> 
        <input type="password" v-model="accountModel.newpassword">
        <h6> Confirm New Password </h6> 
        <input type="password" v-model="accountModel.confirmnewpassword">
        <button id="save" @click="setPassword()"> Save Change </button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  computed: {
    guid() {
      return this.$store.state.userdata.userGuid;
    }
  },
  data() {
    return {
      showModal: false,
      accountModel: {
        password: "",
        newpassword: "",
        confirmnewpassword: "",
      }
    };
  },
  methods: {
    openModal() {
      this.showModal = true;
    },
    closeModal() {
      this.showModal = false;
    },
    async setPassword() {
      try { 
        await axios.put(`/user/password/${this.guid}`, this.accountModel) 
        this.closeModal()
        toast.success("Success")
      }
      catch(e) { toast.error(e.response.data) }
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
h6 {
  margin: 10px 0;
}
input, button {
  padding: .5rem;
  border: 1px solid transparent;
  border-radius: 5px;
  color: white;
}
button {
  width: 50%;
  background: linear-gradient(
		to bottom right,
		var(--lightblue), 
		var(--lightpurple)
	);
}
input {
  width: 100%;
  background: rgba(0, 0, 0, 0.374);
}
.modal {
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.374);
  overflow: auto;
  z-index: 1;
}
.modal-content {
  margin: 0 30em;
  background: #36393f;
  padding: 20px;
}
.close {
  position: absolute;
  top: 10px;
  right: 10px;
  cursor: pointer;
}
#save {
  margin-top: 30px;
  width: 100%;
}
</style>
