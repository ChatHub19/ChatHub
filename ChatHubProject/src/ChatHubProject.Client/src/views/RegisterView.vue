<script setup>
import axios from 'axios';
import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';
</script>

<template>
  <div id="page" class="container-fluid">
		<div id="register" class="container-fluid">
			<form @submit.prevent="register" class="container-fluid">
				<div id="img" >
					<img src="@/assets/logo-vertical.png" alt="logo" class="img-fluid">
				</div>
				<div class="form-group">
					<input type="text" required placeholder="Username" v-model="registerModel.username"/>
				</div>
        <div class="form-group">
					<input type="email" required placeholder="Email" v-model="registerModel.email"/>
				</div>
				<div class="form-group">
					<input type="password" required placeholder="Password" v-model="registerModel.password"/>
				</div>
				<button type="submit" class="button"> Register </button>
				<span> Have an account? </span> 
				<router-link id="register-btn" to="/"> Login </router-link> 
			</form>			
		</div>
	</div>
</template>

<script>
export default {
	data() {
		return {
			registerModel: {
				username: "",
        email: "",
				password: "",
			}
		}
	},
	methods: {
		async register() {
			try {
				const userdata = (await axios.post('user/register', this.registerModel)).data;
        axios.defaults.headers.common['Authorization'] = `Bearer ${userdata.token}`;
        this.$store.commit('authenticate', userdata);   
				alert("vely gud")  
        this.$router.push("/");
			} catch (e) {
				if (e.response.status == 401) {
          toast.error("Login failed! Invalid credentials!");
        }
        if (e.response.status == 400) {
          toast.error("User is already in the database!");
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
#register, #img, input, button {
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
#register {
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
/* Media Query for Mobile Devices */ 
@media (max-width: 480px) { 
  #page, #register {
    padding: 0;
  }
  #register {
    align-items: initial  ;
  }
  form {
    border-radius: 0;
  }
} 
</style>