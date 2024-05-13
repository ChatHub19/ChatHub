<template>
<div class="wrapper">
    <font-awesome-icon icon="fa-solid fa-check" class="icon" @click="acceptRequest"/>
    <font-awesome-icon icon="fa-solid fa-x" class="icon" @click="denyRequest"/>
</div>
</template>

<script>
import axios from 'axios'
export default {
    methods: {
        acceptRequest(){ 
            const senderuserguid = this.$store.state.userdata.userGuid;
            axios.post("requests/addrequest", Url, senderuserguid, ReceiverUserGuid,)
            alert("bruh")
        },
        denyRequest(){
            alert("bruh2deny")
        },
    }
}
</script>

<style scoped>
.icon {
    background: black;
    margin-right: 5px;
    margin-left: 5px;
    border-radius: 50px;
    padding: 5px;
    color: white;
    height: 20px;
    width: 20px;
    cursor: pointer;
}
.icon:hover{
    background: gray;
}
</style>

