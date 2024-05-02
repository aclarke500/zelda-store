<template>
  <!-- <div class="container"> -->
    <div class="form-item">
      <div class="label">Name</div>
      <div class="input"><input type="text" v-model="state.item.name" /></div>
    </div>

    <div class="form-item">
      <div class="label">Price</div>
      <div class="input"><input type="number" v-model="state.item.price" /></div>
    </div>

    <div class="form-item">
      <div class="label">Quantity</div>
      <div class="input"><input type="number" v-model="state.item.quantity" /></div>
  </div>
  <!-- make id form item -->
  <!-- <div class="form-item">
    <div class="label">Id</div>
    <div class="input"><input type="number" v-model="state.item.id" /></div>
  </div> -->

  <button v-if="!props.item" @click="addItem">Add Item</button>
  <button v-else @click="updateItem">Update Item</button>
  <!-- </div> -->

</template>
<script setup>
import router from '@/router';
import { reactive } from 'vue';
const props = defineProps(['item']);
const emits = defineEmits(['itemAdded']);
const uri = 'http://localhost:5016/items/';
const state = reactive({
  item: {
    name: '',
    price: 0,
    quantity: 0,
    // id: 0,

  }
});

if (props.item) { // we re-use this component for creating and updating items
  state.item = props.item;
}

/**
 * API call to add item to sql database
 */
function addItem() {
  fetch(uri, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(state.item)
  })
    .then(response => response.json())
    .then(data => console.log(data))
    .catch(error => console.error('Error:', error));
  emits('itemAdded');
}

function updateItem() {
  fetch(uri + state.item.id, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(state.item)
  })
    .then(response => response.json())
    .then(data => console.log(data))
    .catch(error => console.error('Error:', error));
  router.push('/');
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