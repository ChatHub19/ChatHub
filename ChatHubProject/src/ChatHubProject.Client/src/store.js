import { createStore } from 'vuex'   

export default createStore({
  state() {
    return {
      user: {
        username: "",
        guid: "",
        role: "",
        isLoggedIn: false,
      }
    }
  },
  mutations: {
    authenticate(state, userdata) {
      if (!userdata) {
        state.user = { username: "", guid: "", role: "", isLoggedIn: false };
        return;
      }
      state.user.username = userdata.username;
      state.user.guid = userdata.userGuid;
      state.user.role = userdata.role;
      state.user.isLoggedIn = true;
    },
  }
});