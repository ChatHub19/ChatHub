import { createStore } from 'vuex'   

export default createStore({
  state() {
    return {
      user: {
        displayname: "",
        guid: "",
        role: "",
        isLoggedIn: false,
      }
    }
  },
  mutations: {
    authenticate(state, userdata) {
      if (!userdata) {
        state.user = { displayname: "", guid: "", role: "", isLoggedIn: false };
        return;
      }
      state.user.displayname = userdata.displayname;
      state.user.guid = userdata.userGuid;
      state.user.role = userdata.role;
      state.user.isLoggedIn = true;
    },
  }
});