<template>
  <div class="picker">
    <button class="word" v-for="word in wordsToChoose" v-bind:key="word">{{ word }}</button>
    <button class="skip">Skip</button>
  </div>
</template>
<script>
import { words } from '~/assets/words.js'
export default {
  data: {
      wordsToChoose: []
  },
  beforeCreate() {
    var n = 3;
    this.wordsToChoose = new Array(n)
    var len = words.length,
    taken = new Array(len);
    while (n--) {
      var x = Math.floor(Math.random() * len);
      this.wordsToChoose[n] = words[x in taken ? taken[x] : x];
      taken[x] = --len in taken ? taken[len] : len;
    }
  },
  methods: {

  }
}
</script>
<style>
.picker {
  width: 650px;
  height: 650px;
  position: absolute;
  display: flex;
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