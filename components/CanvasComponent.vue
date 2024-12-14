<template>
    <div>
      <canvas ref="canvas" width="600" height="400" style="border:1px solid black;"></canvas>
      <button @click="drawShapes">Change shapes</button>
    </div>
  </template>
  
  <script lang="ts">
  import { ref, onMounted } from 'vue';
  
  export default {
    setup() {
      const canvas = ref<HTMLCanvasElement | null>(null);
      const context = ref<CanvasRenderingContext2D | null>(null);
  
      onMounted(() => {
        if (canvas.value) {
          context.value = canvas.value.getContext('2d');
          if (context.value) {
            drawInitialShapes(context.value);
          }
        }
      });
  
      const drawInitialShapes = (ctx: CanvasRenderingContext2D) => {
        ctx.fillStyle = 'lightblue';
        ctx.fillRect(50, 50, 200, 100);
  
        ctx.fillStyle = 'black';
        ctx.font = '20px Arial';
        ctx.fillText('Hello Canvas!', 80, 120);
      };
  
      const drawShapes = () => {
        if (context.value) {
          context.value.clearRect(0, 0, 600, 400);
  
          context.value.beginPath();
          context.value.arc(300, 200, 50, 0, Math.PI * 2);
          context.value.fillStyle = 'blue';
          context.value.fill();
  
          context.value.fillStyle = 'green';
          context.value.fillRect(350, 150, 100, 100);
  
          context.value.fillStyle = 'white';
          context.value.font = '24px Arial';
          context.value.fillText('Other shapes!', 370, 220);
        }
      };
  
      return {
        canvas,
        drawShapes,
      };
    },
  };
  </script>
  