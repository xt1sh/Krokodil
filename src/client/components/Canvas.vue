<template>
  <div class="drawing">
    <canvas id="canvas" width="650" height="650" @mouseleave="mouseLeave" @mouseup="mouseup" @mousemove="draw" @mousedown="onClick"></canvas>
    <div id="color-picker-container"></div>

  </div>
</template>

<script>
import iro from "@jaames/iro";

export default {
  data: function() {
    return {
      canvas: null,
      ctx: null,
      canvasx: 0,
      canvasy: 0,
      last_mousex: 0,
      last_mousey: 0,
      mousex: 0,
      mousey: 0,
      mousedown: false,
      tooltype: "draw"
    };
  },
  methods: {
    use_tool: function(tool) {
      this.tooltype = tool;
    },
    draw: function(e) {
      this.mousex = parseInt(e.clientX - this.canvasx);
      this.mousey = parseInt(e.clientY - this.canvasy);
      if (this.mousedown) {
        this.ctx.beginPath();
        this.ctx.globalCompositeOperation = "source-over";
        this.ctx.strokeStyle = this.colorPicker.color.hexString;
        this.ctx.lineWidth = 3;
        this.ctx.moveTo(this.last_mousex, this.last_mousey);
        this.ctx.lineTo(this.mousex, this.mousey);
        this.ctx.lineJoin = this.ctx.lineCap = "round";
        this.ctx.stroke();
        this.ctx.closePath();
      }
      this.last_mousex = this.mousex;
      this.last_mousey = this.mousey;
    },
    onClick: function(e) {
      this.canvasx = canvas.offsetLeft;
      this.canvasy = canvas.offsetTop;
      this.last_mousex = this.mousex = parseInt(e.clientX - this.canvasx);
      this.last_mousey = this.mousey = parseInt(e.clientY - this.canvasy);
      this.ctx.beginPath();
      this.ctx.arc(this.mousex, this.mousey, 1, 0, 2 * Math.PI, true);
      this.ctx.fill();
      this.mousedown = true;
    },
    mouseup: function(e) {
      this.mousedown = false;
    },
    mouseLeave: function(e) {
      this.mousedown = false;
    }
  },
  mounted: function() {
    this.canvas = document.getElementById("canvas");
    this.ctx = canvas.getContext("2d");
    this.canvasx = canvas.offsetLeft;
    this.canvasy = canvas.offsetTop;
    this.colorPicker = new iro.ColorPicker("#color-picker-container", {
      // Set the size of the color picker
      width: 100,
      // Set the initial color to pure red
      color: "#f00"
    });
  }
};
</script>

<style >
canvas {
  border-radius: 5px;
  box-shadow: 0 0 3px 0 rgba(0, 0, 0, 0.5);
  cursor: crosshair;
  background-color: white;
  float: left;
}

#color-picker-container {
  position: fixed;
  bottom: 150px;
  margin-left: 10px;
}
</style>