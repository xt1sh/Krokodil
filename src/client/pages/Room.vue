<template>
  <div class="container">
    <div class="game-container">
      <Canvas />
      <WordPicker />
      <Chat />
    </div>
  </div>
</template>

<script>
import axios from "@nuxtjs/axios";
import Chat from "~/components/Chat.vue";
import Canvas from '~/components/Canvas.vue';
import WordPicker from '~/components/WordPicker.vue';

export default {
  components: {
    Chat,
    Canvas,
    WordPicker
  },

  mounted: async function() {
    let tries = 0;
    this.userName = this.getCookie('username')
    this.userId = this.getCookie('id')
    if(!this.userName || !this.userId) {
      this.deleteAllCookies()
      window.location = '/'
    }
    while (!isConnected && tries < 20) {
      tries++;
      var isConnected = this.$messageHub.connection.connectionState === 1;
      if (isConnected) {
        this.$messageHub.invoke(
          "JoinRoom",
          this.roomId,
          this.userId,
          this.userName
        );
        this.$roomId = this.roomId;
      }
      await new Promise(resolve => setTimeout(resolve, 300));
    }
  },
  data: function() {
    return {
      roomId: this.$route.query.id
    };
  },

  methods: {
    disconnect: function() {
      let userId = this.getCookie("id");
      this.$messageHub.invoke("LeaveRoom", this.roomId);
      this.$axios.post(`http://localhost:5000/Disconnect/${userId}`);
    },
    getCookie: function(name) {
      var value = "; " + document.cookie;
      var parts = value.split("; " + name + "=");
      if (parts.length == 2)
        return parts
          .pop()
          .split(";")
          .shift();
    },
    deleteAllCookies: function() {
    var cookies = document.cookie.split(";");
    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i];
        var eqPos = cookie.indexOf("=");
        var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
        document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
    }
}
  },

  beforeDestroy: function() {
    this.disconnect();
  },

  watch: {
    $route(to, from) {
      if (to !== from) {
        this.disconnect();
      }
    }
  }
};
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

.game-container {
  display: flex;
}

.picker {
  display: none;
}
</style>
