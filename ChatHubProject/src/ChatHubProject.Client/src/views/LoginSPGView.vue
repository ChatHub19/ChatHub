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
				const userdata = (await axios.post('user/loginspg', this.loginModel)).data;
        axios.defaults.headers.common['Authorization'] = `Bearer ${userdata.token}`;
        this.$store.commit('authenticate', userdata);  
				alert("cool")    
        this.$router.push("/");
			} catch (e) {
        if(e.response === undefined) { console.error(e); }
        else if (e.response.status == 401) {
          toast.error("Login failed! Invalid credentials!");
        }
      }
		},
	}
}
</script>

<style scoped>
#login, #img, input, button {
	display: flex;
	justify-content: center;
	align-items: center;
}
#page {
  height: 100vh;
	width: 100vw;
	padding: 1rem;
}
#login {
	width: 100%;
	height: 100%;
}
form {
	width: 500px;
	background: var(--theme-dark-color);
	padding: 3rem;
	border-radius: 15px;
}
input, button {
	width: 100%;
	padding: .5rem 1rem;
	color: white;
	border-radius: 5px;
	margin-bottom: 20px;
	background: #40444b;
}
input::placeholder {
	color: white;
}
button{
	font-weight: 900;
	color: black;
	background: linear-gradient(to bottom right, #82B4F6, #670DEB);
}
*, *::before, *::after {
  box-sizing: border-box;
}
</style>