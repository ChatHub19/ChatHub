<script setup>
import axios from "axios";
</script>

<template>
  <div>
    <div>
      <!-- Eingabefeld für die Nachricht -->
      <input
        v-model="message"
        @keyup.enter="SendMessage"
        placeholder="Send Message to"
      />

      <!-- Nachrichten-Liste -->
      <ul>
        <li v-for="nachricht in nachrichten" :key="nachricht.id">
          {{ nachricht.text }}
        </li>
      </ul>
      <button @click="SendMessage()">Send Message</button>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      message: "",
    };
  },
  async mounted() {
    try {
      this.categories = (await axios.get("message")).data;
    } catch (e) {
      alert("Fehler beim Laden der Messages.");
    }
  },
  methods: {
    async SendMessage() {
      try {
        const userdata = (await axios.post("message/send", message)).data;
      } catch (e) {
        if (e.response.status == 500) {
          toast.error("Error");
        }
      }
    },
  },
  


};
</script>

<style>
input {
  width: 50%;
  background: var(--theme-middle);
  color: white;
}
</style>