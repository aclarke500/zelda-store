<template>

  <!-- <nav>
    <router-link to="/"></router-link>
  </nav> -->
  <router-view class="router-view"></router-view>
</template>
<script setup>
import router from './router';
router.push('/');
</script>
<style>
.router-view{
  background-color: #42b983;
}
header{
  background-color: #42b983;
}

body{
  margin: 0;
  /* box-sizing: border-box; */
  /* box-sizing: content-box; */
}
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  height: 1000px;
  width: 100%;
  background-color: #42b983;
  margin: 0;
}
@font-face {
    font-family: "Hylia Serif";
    src: url("assets/HyliaSerif.otf") format("truetype");
    font-weight: normal;
    font-style: normal;
}
h1, h2, h3, h4, h5, h6 {
  font-family: "Hylia Serif", serif;
}

nav {
  padding: 30px;
}

nav a {
  font-weight: bold;
  color: #2c3e50;
}

nav a.router-link-exact-active {
  color: #42b983;
}
</style>
