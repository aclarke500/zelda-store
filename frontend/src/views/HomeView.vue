<template>
  <div class="home">
    <h1>Hello</h1>
    <button @click="fetchData">Click</button>
    <AddItem @item-added="fetchData" />
    <div v-if="state.items">
      <ItemCard v-for="item in state.items" :key="item.id" :item="item"/>
    </div>
  </div>
</template>

<script setup>
import AddItem from '@/components/AddItem.vue';
import ItemCard from '@/components/ItemCard.vue';

import { onMounted, reactive } from 'vue';
const state = reactive({
  items: null,
});
const uri = 'http://localhost:5016/items/';

function fetchData(){
  console.log('fetching data');
  fetch(uri, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json'
    },

  })
    .then(response => response.json())
    .then(data => {
      console.log(data);
      state.items = data;
    })
    .catch(error => console.error('Error:', error));

}

onMounted(fetchData);

</script>
