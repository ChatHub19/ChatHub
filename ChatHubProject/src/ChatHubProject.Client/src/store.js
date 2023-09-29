import { createStore } from 'vuex'   

export default createStore({
  state() {
    return {
      user: {
        name: "",
        guid: "",
        role: "",
        isLoggedIn: false,
      }
    }
  },
  mutations: {
    authenticate(state, userdata) {
      if (!userdata) {
        state.user = { name: "", guid: "", role: "", isLoggedIn: false };
        return;
      }
      state.user.name = userdata.username;
      state.user.guid = userdata.userGuid;
      state.user.role = userdata.role;
      state.user.isLoggedIn = true;
    },
  }
});