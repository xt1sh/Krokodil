<template>
  <div class="container">
    <div class="game-container">
      <Canvas />
    </div>
  </div>
</template>

<script>
import Canvas from '~/components/Canvas.vue'

export default {
    components: {
        Canvas
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
}

.drawing {
  width: 650px;
  height: 650px;
}
</style>
