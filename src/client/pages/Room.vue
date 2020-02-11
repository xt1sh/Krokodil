<template>
  <button v-on:click="connect">connect</button>
</template>

<script>
import axios from '@nuxtjs/axios'

export default {
  mounted: function() {
    setTimeout(() => this.$messageHub.invoke("JoinRoom", this.$route.query.id), 500)
  },
  data: function() {
    // this.$messageHub.invoke("JoinRoom", this.$route.query.id);
    return {
      roomId: this.$route.query.id
    }
  },

  methods: {
    connect: function() {
      this.$messageHub.invoke("JoinRoom", this.$route.query.id);
    },
    disconnect: function() {
			this.$messageHub.invoke("LeaveRoom", this.roomId)
		}
  }
}
</script>
