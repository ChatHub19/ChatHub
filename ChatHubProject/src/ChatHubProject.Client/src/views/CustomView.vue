<script setup>
import AccountInfo from "../components/AcountInfo.vue"
</script>

<template>
  <div class="wrapper">
    <section>
      <div class="custom-option">
        <AccountInfo/>
      </div>
    </section>
  </div>
</template>

<script>
export default {
  
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
section {
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100vw;
  height: 100vh;
  align-items: flex-start;
  background: #36393f;
  color: white;
}
span:hover {
  color: rgba(255, 255, 255, 0.649);
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
