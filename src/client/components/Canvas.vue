<template>
  <div class="drawing">
    <canvas v-if="isDrawer"
      id="canvas"
      width="650"
      height="650"
      @mouseleave="mouseLeave"
      @mouseup="mouseup"
      @mousemove="onMouseMove"
      @mousedown="onClick"
    ></canvas>
    <canvas v-else
      id="canvas"
      width="650"
      height="650"
    ></canvas>
    <div id="color-picker-container" @mousemove="colorChange"></div>
    <range-slider class="slider" min="1" max="25" step="1" v-model="sliderValue"></range-slider>
    <button class="clear" v-on:click="invokeClear">Clear</button>
  </div>
</template>

<script>
import iro from "@jaames/iro";
import RangeSlider from "vue-range-slider";
import "vue-range-slider/dist/vue-range-slider.css";
import { getCookie } from 'assets/cookie.js';

export default {
  data: function() {
    return {
      isDrawer: false,
      points: [],
      last_mousex: 0,
      last_mousey: 0,
      mousex: 0,
      mousey: 0,
      mousedown: false,
      sliderValue: 5
    };
  },

  mounted: function() {
    this.canvas = document.getElementById("canvas");
    this.ctx = canvas.getContext("2d");
    this.ctx.globalCompositeOperation = "source-over";
    this.ctx.lineJoin = this.ctx.lineCap = "round";
    this.canvasx = canvas.offsetLeft;
    this.canvasy = canvas.offsetTop;
    this.colorPicker = new iro.ColorPicker("#color-picker-container", {
      // Set the size of the color picker
      width: 100,
      // Set the initial color to pure red
      color: "#f00"
    });
    this.slider = document.getElementsByClassName("range-slider-fill")[0];
    this.slider.style.backgroundColor = "#f00";

    this.$messageHub.on('ReceiveDraw', this.onDataReceive)
    this.$messageHub.on('ClearData', this.clear)
    this.$messageHub.on('StartRound', this.startRound)
  },

  components: {
    RangeSlider
  },
  methods: {
    onDataReceive: function(data) {
      if (!this.isDrawer) {
        this.mousex = data.x;
        this.mousey = data.y;
        this.sliderValue = data.stroke;
        this.mousedown = data.mousedown;
        if (!this.slider) {
          this.slider = document.getElementsByClassName("range-slider-fill")[0];
        }
        this.slider.style.backgroundColor = data.color;
        this.colorPicker.color.hexString = data.color
        this.points = data.points
        if (data.isPoint) {
          this.drawPoint()
        } else {
          this.draw()
        }
      }
    },
    onMouseMove: function(e) {
      this.slider.style.backgroundColor = this.colorPicker.color.hexString;
      if (this.mousedown) {
        this.mousex = parseInt(e.clientX - this.canvasx);
        this.mousey = parseInt(e.clientY - this.canvasy);
        this.sendDraw(false)
        this.draw()
      }
    },
    onClick: function(e) {
      this.canvasx = canvas.offsetLeft;
      this.canvasy = canvas.offsetTop;
      this.mousex = parseInt(e.clientX - this.canvasx);
      this.mousey = parseInt(e.clientY - this.canvasy);
      this.sendDraw(true)
      this.drawPoint()
    },
    draw: function() {
      this.points.push({ x: this.mousex, y: this.mousey });

      var p1 = this.points[0];
      var p2 = this.points[1];
      this.ctx.strokeStyle = this.colorPicker.color.hexString;
      this.ctx.lineWidth = this.sliderValue;
      this.ctx.beginPath();
      this.ctx.moveTo(p1.x, p1.y);
      for (var i = 1, len = this.points.length; i < len; i++) {
        var midPoint = this.midPointBtw(p1, p2);
        this.ctx.quadraticCurveTo(p1.x, p1.y, midPoint.x, midPoint.y);
        p1 = this.points[i];
        p2 = this.points[i + 1];
      }

      this.ctx.stroke();
    },
    drawPoint: function() {
      this.points.push({ x: this.mousex, y: this.mousey });
      this.ctx.beginPath();
      this.ctx.fillStyle = this.colorPicker.color.hexString;
      this.ctx.arc(
        this.mousex,
        this.mousey,
        this.sliderValue / 2,
        0,
        2 * Math.PI,
        true
      );
      this.ctx.fill();
      this.mousedown = true;
    },
    sendDraw: function(isPoint) {
      if (this.isDrawer) {
        this.$messageHub.invoke('Draw', {
          x: this.mousex,
          y: this.mousey,
          stroke: this.sliderValue,
          color: this.colorPicker.color.hexString,
          mousedown: this.mousedown,
          points: this.points,
          isPoint: isPoint
        }, this.$route.query.id)
      }
    },
    mouseup: function(e) {
      this.mousedown = false;
      this.points = [];
    },
    mouseLeave: function(e) {
      this.mousedown = false;
      this.points = [];
    },
    colorChange: function(e) {
      this.slider.style.backgroundColor = this.colorPicker.color.hexString;
    },
    midPointBtw: function(p1, p2) {
      return {
        x: p1.x + (p2.x - p1.x) / 2,
        y: p1.y + (p2.y - p1.y) / 2
      };
    },
    clear: function() {
      this.ctx.clearRect(0, 0, canvas.width, canvas.height);
    },
    startRound: function(userId, words) {
      if (userId == getCookie('id')) {
        this.isDrawer = true;
      }
      else {
        this.isDrawer = false;
      }
    },
    invokeClear: function() {
      this.$messageHub.invoke('Clear', this.$route.query.id)
    },
    
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

.drawing {
  width: 650px;
  height: 650px;
}

#color-picker-container {
  position: fixed;
  bottom: 150px;
  margin-left: 10px;
}

.slider {
  position: relative;
  width: 200px;
  bottom: 70px;
  float: right;
}

.range-slider-knob {
  background-color: rgb(105, 105, 105);
}

.clear {
  position: relative;
  bottom: 70px;
  background-color: rgb(92, 82, 150);
  color: white;
  border-radius: 1px;
  border: rgb(92, 82, 150);
  width: 70px;
  height: 30px;
}

.clear:active {
  outline: 0;
  background-color: rgb(65, 56, 116);
}

.clear:focus {
  outline: 0;
}
</style>
