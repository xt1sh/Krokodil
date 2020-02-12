<template>
  <div>
    <div>
      <input v-model="message" placeholder="message" />
    </div>
    <div>
      <button v-on:click="onClick">Submit</button>
    </div>
    <div>
      <ul>
				<li v-for="mes in messages" v-bind:key="mes.message">{{ mes.userName }}: {{ mes.message }}</li>
			</ul>
    </div>
  </div>
</template>

<script>
var Vue = require("vue");

export default {
  data: function() {
    return {
      room: this.$route.query.id,
			message: "",
			messages: this.$messages
    };
  },

  methods: {
    onClick: function() {
      this.$messageHub.invoke("SendRoomMessage", this.room, this.$userId, this.$userName, this.message);
		}
  }
};
</script>