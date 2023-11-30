<script setup>
import AccountInfo from "../components/AcountInfo.vue"
</script>

<template>
  <div class="wrapper">
    <aside>
      <span class="info"> User Settings </span>
      <span class="option" @click="getAccountInfo()"> Account </span>
      <span class="option"> Profile </span>
      <span class="option"> Security </span>
      <span class="option"> Languange </span>
      <span class="option"> Advance </span>
    </aside>
    <section>
      <div class="custom-option">
        <AccountInfo/>
      </div>
      <span class="custom-option"> smt</span>
      <span class="custom-option"> smt</span>
      <span class="custom-option"> smt</span>
      <span class="custom-option"> smt</span>
    </section>
  </div>
</template>

<script>
export default {
  async mounted() {
    await this.getAccountInfo();
  },
  methods: {
    async getAccountInfo() {
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
.wrapper {
  height: 100vh;
  max-width: 100vw;
  display: flex;
}
aside {
  width: 30%;
  padding-right: 10px;
  align-items: flex-end; 
  background: #2f3136;
  color: white;
  display: flex;
  flex-direction: column;
  justify-content: right;
}
section {
  width: 70%;
  align-items: flex-start;
  background: #36393f;
  color: white;
}
.info:first-child {
  margin-top: 10%;
}
.info {
  width: 50%;
  margin: 0 5px;
  padding-left: .45rem;
  font-size: .95rem;
  font-weight: bold;
  opacity: .5;
  background: transparent;
}
.option {
  width: 50%;
  padding: .45rem;
  margin: 5px;
  border-radius: 5px;
  background: transparent;
}
.option:hover {
  background: rgba(211, 211, 211, 0.578);
}
.option:nth-child(2) {
  background: rgba(211, 211, 211, 0.578);
}
</style>
