<template>
  <div class="chat-wrapper">
    <div class="chat bordered">
      <div class="messages-wrapper">
        <ul>
          <li v-for="mes in finalMessages" v-bind:key="mes.message">
            <div class="system-message-wrapper" v-if="mes.isSystem">
              {{mes.message}}
            </div>
            <div v-else>
              {{mes.message}}
            </div>
          </li>
        </ul>
      </div>
    </div>
    <div class="form-wrapper bordered">
      <form v-on:submit.prevent="onSubmit">
        <div class="input-wrapper">
          <input class="message-input" v-model="message" placeholder="Enter word" />
          <button type="submit">Submit</button>
        </div>
      </form>
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
      messages: this.$messages,
      systemMessages: this.$systemMessages
    };
  },

  beforeMount: function() {
    this.userName = this.getCookie('username')
    this.userId = this.getCookie('id')
  },

  methods: {
    onSubmit: function() {
      if (this.checkInput()) {
        this.$messageHub.invoke("SendRoomMessage", this.room, this.userId, this.userName, this.message)
      }
      this.message = ''
    },

    getCookie(name) {
      let matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
      ));
      return matches ? decodeURIComponent(matches[1]) : undefined;
    },

    checkInput: function() {
      for (let i = 0; i < this.message.length; i++) {
        if (this.message.charAt(i)) {
          return true
        }
      }
      return false
    }
  },

  computed: {
    finalMessages: function() {
      return this.messages.map((element) => {
        if (element.userName) {
          return { message: `${element.userName}: ${element.message}`, isSystem: false }
        } else {
          return { message: element.message, isSystem: true }
        }
      }
      )
    }
  }
};
</script>

<style>
  ul {
    list-style-type: none;
    word-wrap: break-word;
    max-width: 285px;
    text-align: left;
    padding: 0;
  }

  .chat-wrapper {
    position: relative;
    display: flex;
    justify-content: left;
    width: 325px;
    height: 650px;
    margin-left: 5px;
  }

  .chat {
    position: relative;
    width: inherit;
    height: 575px;
    background-color: white;
  }

  .bordered {
    box-sizing: border-box;
    border-radius: 5px;
    box-shadow: 0 0 3px 0 rgba(0, 0, 0, 0.5);
  }

  .form-wrapper {
    position: absolute;
    bottom: 0;
    width: 100%;
    background-color: white;
  }

  .message-input {
    display: inline-block;
    padding: 12px 20px;
    margin: 15px 0;
    border: 1px solid #ccc;
    border-radius: 4px;
    box-sizing: border-box;
  }

  .messages-wrapper {
    max-height: -webkit-fill-available;
    overflow-y: auto;
    padding: 15px 25px 15px 25px;
    position: absolute;
    bottom: 0;
    width: 325px;
  }

  .system-message-wrapper {
    width: 100%;
    text-align: center;
    color: #969696;
  }

  button[type=submit] {
    padding: 12px;
    border: 1px solid #ccc;
    border-radius: 4px;
  }
</style>
