import { createStore } from "vuex";

export default createStore({
  state() {
    return {
      userdata: [],
      isLoggedIn: false,
    }
  },
  mutations: {
    authenticate(state, userdata) {
      if (!userdata) {
        state.userdata = null;
        return;
      }
      console.log(userdata.role)
      state.userdata = userdata;
      state.isLoggedIn = true;
    },
  },
});

