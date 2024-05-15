<script setup>
import axios from 'axios';
import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';
</script>

<template>
  <div id="page" class="container-fluid">
		<div id="login" class="container-fluid">
			<form @submit.prevent="login" class="container-fluid">
				<div id="img" >
					<img src="@/assets/logo-vertical.png" alt="logo" class="img-fluid">
				</div>
				<div class="form-group">
					<input type="text" required placeholder="Username" v-model="loginModel.username"/>
				</div>
				<div class="form-group">
					<input type="password" required placeholder="Password" v-model="loginModel.password"/>
				</div>
				<button type="submit" class="button">Login</button>
				<span> Don't have an account? </span> 
				<router-link id="register-btn" to="/register"> Register </router-link> 
			</form>			
		</div>
	</div>
</template>

<script>
export default {
	data() {
		return {
			loginModel: {
				username: "",
				password: "",
			}
		}
	},
	methods: {
		async login() {
			try {
				const userdata = (await axios.post('user/login', this.loginModel)).data;
        this.$store.commit('authenticate', userdata);  
        this.$router.push("/");
			} 
			catch (e) {
				if (e.response.status == 500) {
          toast.error("Login failed! User does not exist!");
        }
        else {
          toast.error(e.response.data);
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
#login, #img, input, button {
	display: flex;
	justify-content: center;
	align-items: center;
}
#page {
  height: 100vh;
	width: 100vw;
	padding: 1rem;
	background: linear-gradient(
		to bottom right,
		var(--lightblue), 
		var(--lightpurple)
	);
}
#login {
	width: 100%;
	height: 100%;
}
#register-btn {
	text-decoration: none;
	color: var(--lightblue);
}
form {
	width: 500px;
	background: var(--theme-dark);
	padding: 3rem;
	border-radius: 15px;
}
input, button {
	width: 100%;
	padding: .5rem 1rem;
	color: white;
	border-radius: 5px;
	background: #40444b;
}
input {
	margin-bottom: 20px;
}
button {
	margin-bottom: 10px;
	font-weight: 500;
	color: white;
	background: linear-gradient(to bottom right, var(--lightblue), var(--lightpurple));
}
input::placeholder {
	color: white;
}
span {
	opacity: 0.5;
	color: white;
}
@media (max-width: 480px) { 
  #page, #login {
    padding: 0;
  }
  form {
		height: 100%;
    border-radius: 0;
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
  }
	.form-group {
		width: 100%;
		margin-bottom: 20px;
	}
} 
</style>