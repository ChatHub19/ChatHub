<script setup>

</script>

<template>
  <div class="wrapper">
    <aside>
      <span class="option"> Account </span>
      <span class="option"> Profile </span>
      <span class="option"> Security </span>
      <span class="option"> Languange </span>
      <span class="option"> Advance </span>
    </aside>
    <section>
      <span class="custom-option"> smt</span>
      <span class="custom-option"> smt</span>
      <span class="custom-option"> smt</span>
      <span class="custom-option"> smt</span>
      <span class="custom-option"> smt</span>
    </section>
  </div>
</template>

<script>
export default {
  setup() {
    
  },
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
  align-items: flex-end;
  background: grey;
}
section {
  width: 70%;
  align-items: flex-start;
  background: white;
}
aside, section {
  display: flex;
  justify-content: right;
  flex-direction: column;
}
.option {
  width: 50%;
  padding: .5rem;
  margin: 5px;
  border-radius: 5px;
  background: transparent;
}
.option:hover {
  background: rgba(211, 211, 211, 0.578);
}
.option:active {
  background: lightgrey;
}
</style>
