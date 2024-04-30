<template>
  <div class="home">
    <h1>Hello</h1>
    <button @click="fetchData">Click</button>
  </div>
</template>

<script setup>
const uri = 'http://localhost:5016/todoitems/';

function fetchData(){
  console.log('fetching data');
  fetch(uri, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json'
    },
    // mode: 'no-cors',
  })
    .then(response => response.json())
    .then(data => console.log(data))
    .catch(error => console.error('Error:', error));

}

</script>
