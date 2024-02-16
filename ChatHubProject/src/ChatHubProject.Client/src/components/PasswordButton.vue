<template>
  <div>
    <button
      @focus="getEmptyUsernameValue()" 
      @blur="getUsernameValue()"
      @keyup.enter="setUsername()"
    > 
      Change Password
    </button>
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
button {
  width: 50%;
  padding: .5rem;
  border: 1px solid transparent;
  border-radius: 5px;
  color: white;
  background: linear-gradient(
		to bottom right,
		var(--lightblue), 
		var(--lightpurple)
	);
}
</style>
