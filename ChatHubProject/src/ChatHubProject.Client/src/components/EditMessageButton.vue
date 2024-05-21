<script setup>
import signalRService from '../services/SignalRService.js';
</script>

<template>
    <div class="wrapper">
        
        <input 
          type="text" 
        >
        
    </div>
</template>

<script>
export default {
    data() {
    return {
      message: "",
    }
  }, 
  computed: {
    guid() {
      return this.$store.state.userdata.userGuid;
    },
  },

  methods: {
    async editMessage() {
     try {await axios.put(`/message/${this.guid}`, this.accountModel) }
      catch(e) { toast.error(e.response.data) } }
    }
  }

</script>

<style>

button
{
    background: rgba(61, 62, 63, 0.681);
    color: white;
    border-radius: 20px;
}


</style>