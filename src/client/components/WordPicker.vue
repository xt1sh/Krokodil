<template>
  <div class="picker" >
    <button class="word" v-for="word in wordsToChoose" v-bind:key="word" v-on:click="pickWord">{{ word }}</button>
    <button class="skip" v-on:click="skip">Skip</button>
  </div>
</template>
<script>
import { getCookie } from '~/assets/cookie.js'
export default {
  data: function() {
    return {
      isDrawer: false,
      pickedWord: null,
      wordsToChoose: []
    }
  },
  beforeCreate: function() {

  },
  mounted: function() {
      this.$messageHub.on('StartRound', this.startRound)
  },
  methods: {
    startRound(userId, words) {
      document.getElementsByClassName('picker')[0].style.display = 'none';
      if (userId == getCookie('id')) {
        this.isDrawer = true;     
        this.wordsToChoose = words;
        document.getElementsByClassName('picker')[0].style.display = 'flex';
      }
    },
    pickWord(e) {
      document.getElementsByClassName('picker')[0].style.display = 'none';
      this.$messageHub.invoke('Play', e.target.textContent, this.$route.query.id);
    },
    skip(e) {
      document.getElementsByClassName('picker')[0].style.display = 'none';
      this.$messageHub.invoke('StartRound', this.$route.query.id)
    }
  }
}
</script>
<style>
.picker {
  display: none;
  width: 650px;
  height: 650px;
  position: absolute;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  background-color: rgba(255,255,255, 0.4)
}

.word {
margin-bottom: 15px;
width: 200px;
height: 35px;
background-color: rgb(19, 139, 209);
color: white;
font-size: 18px;
border-radius: 5px;
}

.skip {
  margin-top: 10px;
  width: 200px;
  height: 35px;
  background-color: rgb(51, 151, 93);
  color: white;
  font-size: 18px;
  border-radius: 5px;
}
</style>