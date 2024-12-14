import DefaultTheme from 'vitepress/theme';
import CanvasComponent from '../../../components/CanvasComponent.vue';
import { Theme } from 'vitepress';

const theme: Theme = {
  ...DefaultTheme,
  enhanceApp({ app }) {
    app.component('CanvasComponent', CanvasComponent);
  },
};

export default theme;
