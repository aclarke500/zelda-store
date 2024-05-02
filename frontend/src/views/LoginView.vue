<template>
  <h1>Check in at Beedle's Air Shop</h1>
 <div class="form-item">
      <div class="label">Name</div>
      <div class="input"><input type="text" v-model="state.name"/></div>
  </div>
  <button @click="login">Check In</button>

</template>
<script setup>
import { reactive } from 'vue';
import {store} from '@/store';
import router from '@/router';

const state = reactive({
  name: '',
});


function login(){

  fetch(store.uri+'users/login/?name='+state.name, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(state.name)
  })
    .then(response => response.json())
    .then(data => {
      console.log(data);
      store.user = data;
      router.push('/');
    })
    .catch(error => console.error('Error:', error));
}
</script>

<style scoped>
.form-item {
  margin: 10px 0; /* Separate each form item by adding vertical margin */
  display: block; /* Make each form item a block element */
}

.label {
  display: inline-block; /* To maintain horizontal alignment with the input */
  width: 100px; /* Specify a fixed width for the label to align consistently */
}

.input {
  display: inline-block; /* Keep input inline with the label */
}
</style>