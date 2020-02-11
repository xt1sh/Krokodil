<template>
  <button v-on:click="disconnect">disconnect</button>
</template>

<script>
import axios from "@nuxtjs/axios";

export default {
  mounted: async function() {
    console.log(this.roomId)
    let isConnected = this.$messageHub.connection.connectionState === 1;
    let tries = 0;
    while (!isConnected && tries < 20) {
      tries++;
      console.log(tries);
      isConnected = this.$messageHub.connection.connectionState === 1;
      if (isConnected) {
        console.log(this.roomId);
        this.$messageHub.invoke("JoinRoom", this.roomId);
        this.$roomId = this.roomId;
        this.connect();
      }
      console.log(isConnected);
      await new Promise(resolve => setTimeout(resolve, 300))
    }
    console.log("aho");
  },
  data: function() {
    console.log(this.$route.query.id)
    return {
      roomId: this.$route.query.id
    };
  },

  methods: {
    connect: function() {
      console.log(this.roomId)
      console.log("zhops");
      this.$axios
        .post(`http://localhost:5000/ConnectToRoom/${this.roomId}`, {
          name: "srak",
          id: "zalup"
        })
        .then();
    },
    disconnect: function() {
      this.$messageHub.invoke("LeaveRoom", this.roomId);
      this.$axios.post(`http://localhost:5000/Disconnect/zalup`);
    }
  },

  beforeDestroy: function() {
    this.disconnect()
  },

  watch:{
    $route (to, from){
        if (to !== from) {
          this.disconnect();
        }
    }
  } 
};
</script>
