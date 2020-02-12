<template>
  <div class="container">
    <div class="game-container">
      <Canvas />
      <WordPicker />
    </div>
  </div>
</template>

<script>
import Canvas from '~/components/Canvas.vue';
import WordPicker from '~/components/WordPicker.vue';

export default {
    components: {
        Canvas,
        WordPicker
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

.game-container {
    width: 900px;
    display: flex;
}

.picker {
  display: none;
}
</style>
