<template>
<h1>Update Items</h1>
<p>{{ state.itemId }}</p>
<AddItem v-if="state.item" :item="state.item"/>
<!-- delete button -->
<button @click="deleteItem">Delete Item</button>
</template>
<script setup>
import { reactive } from 'vue';
import { useRoute } from 'vue-router';
import AddItem from '@/components/AddItem.vue';


const route = useRoute();
const itemId = route.params.id;
const uri = 'http://localhost:5016/items/';

const state = reactive({
  item: null,
  itemId: itemId,
});

fetch(uri + itemId, {
  method: 'GET',
  headers: {
    'Content-Type': 'application/json'
  },
})
  .then(response => response.json())
  .then(data => {
    console.log(data);
    state.item = data;
  })
  .catch(error => console.error('Error:', error));

function deleteItem() {
  fetch(uri + itemId, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json'
    },
  })
    .then(response => response.json())
    .then(data => console.log(data))
    .catch(error => console.error('Error:', error));
}

</script>