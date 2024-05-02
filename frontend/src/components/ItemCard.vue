<template>
  <div class="item-card" @click="openItem">
    <div class="item-name">{{ item.name }}</div>
    <div class="item-price">{{ item.price }} rupees</div>
    <div class="item-quantity">{{ item.quantity }} in stock</div>

  </div>
  
</template>
<script setup>
import router from '@/router';
import {store }from '@/store';
const emits = defineEmits(['select']);
const props = defineProps(['item']);

function openItem() {
  if (store.user.name !== 'Beedle') {
    emits('select', props.item);
    return;
  }
  console.log('Opening item:', props.item);
  router.push({ name: 'item', params: { id: props.item.id } });
}
</script>
<style scoped>
.item-card {
  border: 3px solid #864d03;
  padding: 10px;
  margin: 8px;
  display: inline-block;
  background-color: #bdbb0c;
  /* width: 200px; */
}
/* hover styles */
.item-card:hover {
  background-color: #7bdafa;
  border: 3px solid black;
  margin: 8px;
}
.danger {
  background-color: red;
  color: white;
  border: none;
  padding: 5px;
  margin: 5px;
}
</style>