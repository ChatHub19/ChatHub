<script setup>
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
    </div>
  </div>
</template>

<script>
</script>
<style>
input{
width: 50%;
background: var(--theme-middle);
color: white;
}


</style>