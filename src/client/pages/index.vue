<template>
  <div class="container">
    <div class="name-input">
      <input type="text" autocomplete="off" v-model="username" id="username" placeholder="Username">
      <div class="buttons">
        <button class="game-button" v-on:click="onRandomClick">Quick game</button>
        <button class="game-button" v-on:click="onClick">Private game</button>
      </div>
    </div>
  </div>
</template>

<script>
import Logo from '~/components/Logo.vue'
import Chat from '~/components/Chat.vue'
import axios from 'axios'
import Guid from 'guid';

var Vue = require("vue");

export default {
  components: {
    Logo,
    Chat
  },

  data: function() {
    return {
      username: ''
    }
  },
  mounted() {
    this.username = this.getCookie('username')
  },
  methods: {
    onRandomClick: function() {
      this.onClick()
      this.$axios.get(`http://localhost:5000/GetRandomRoom`)
        .then(response => {
          this.$router.push({ path: 'room', query: { id: response.data }})
        })
    },
    onClick: function() {
      if(!this.getCookie('id')) {
        let id = Guid.raw()
        document.cookie = 'id=' + id
        this.$userId = id
      }
      document.cookie = 'username=' + this.username;
      this.$userName = this.username
    },
    getCookie(name) {
      let matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
      ));
      return matches ? decodeURIComponent(matches[1]) : undefined;
    }
  }
}
</script>

<style>
.container {
  margin: 0 auto;
  min-height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  text-align: center;
}

.name-input {
  border-radius: 5px;
  box-shadow: 0 0 3px 0 rgba(0, 0, 0, 0.5);
  background-color: white;
  height: 600px;
  width: 400px;
  display: flex;
  flex-direction: column;
  justify-content:center;
  align-items: center;
}

.buttons {
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 220px;

}

.game-button {
  border-radius: 3px;
  width: 100px;
  height: 30px;
  background-color: rgb(184, 92, 184);
  color: white;
  border: none;
}

.game-button:hover {
  background-color: rgb(121, 58, 121);
  cursor: pointer;
}

.game-button:focus {
  outline: 0;
}

#username {
  margin-bottom: 20px;
  width: 220px;
  height: 30px;
  border-radius: 5px;
  box-shadow: 0 0 3px 0 rgba(0, 0, 0, 0.6);
}


</style>
