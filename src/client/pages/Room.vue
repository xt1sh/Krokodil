<template>
  <div class="container">
    <div class="game-container">
      <Canvas />
      <WordPicker />
      <button v-on:click="disconnect">disconnect</button>
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
    while (!isConnected && tries < 20) {
      tries++;
      console.log(tries)
      var isConnected = this.$messageHub.connection.connectionState === 1;
      if (isConnected) {
        console.log(this.roomId, this.getCookie("id"), this.getCookie("username"))
        this.$messageHub.invoke(
          "JoinRoom",
          this.roomId,
          this.getCookie("id"),
          this.getCookie("username")
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
      console.log(userId);
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
    width: 900px;
    display: flex;
}

.picker {
  /* display: none; */
}
</style>
