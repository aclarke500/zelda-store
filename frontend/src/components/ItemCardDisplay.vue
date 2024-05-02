<template>
  <div class="container">
   <div class="item-cards" v-if="state.items" :key="state.items">
      <ItemCard class="card" v-for="item in state.items" :key="item" 
      :item="item" @select="(i)=>{onItemSelect(i)}"/>
    </div>
  </div>
</template>
<script setup>
import { onMounted, reactive } from 'vue';
import ItemCard from '@/components/ItemCard.vue';
import { store, updateItems } from '@/store';
const state = reactive({
  items: store.items,
});
const uri = 'http://localhost:5016/items/';
const emits = defineEmits(['select']);


function fetchData() {
  state.items = null;
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
    console.log(state.items, "shalah");
}

function onItemSelect(item) {
  console.log(item);
  emits('select', item);
}

onMounted(updateItems);
</script>

<style scoped>
.item-cards {
    display: flex;
    flex-direction: row; /* default, can be omitted */
    flex-wrap: wrap; /* enables wrapping */
    justify-content: centre;
    width: 1000px; /* or set it to a larger value, like 1000px */
    align-items: center;
    border: 1px solid black;
}

.card {
flex:3;
}
</style>