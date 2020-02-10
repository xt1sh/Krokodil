<template>
  <div>
    <div>
      <input v-model="room" placeholder="room name" />
    </div>
    <div>
      <button v-on:click="onConnect">Connect</button>
    </div>
    <div>
      <input v-model="message" placeholder="message" />
    </div>
    <div>
      <button v-on:click="onClick">Submit</button>
    </div>
    <div>
      <ul>
				<li v-for="mes in messages" v-bind:key="mes">{{ mes }}</li>
			</ul>
    </div>
		<div>
			<button v-on:click="onDisconnect">Disconnect</button>
		</div>
  </div>
</template>

<script>
var Vue = require("vue");

export default {
  data: function() {
    return {
      room: "",
			message: "",
			messages: this.$messages
    };
  },

  methods: {
    onConnect: function() {
      this.$messageHub.invoke("JoinRoom", this.room);
    },
    onClick: function() {
      this.$messageHub.invoke("SendRoomMessage", this.room, this.message);
		},
		onDisconnect: function() {
			this.$messageHub.invoke("LeaveRoom", this.room)
		}
  }
};
</script>