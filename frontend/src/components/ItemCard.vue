<template>
  <div class="item-card" @click="openItem">
    <div class="item-name">{{ item.name }}</div>
    <div class="item-price">{{ item.price }} rupees</div>
    <div class="item-quantity">{{ item.quantity }} in stock</div>

  </div>
</template>
<script setup>
import router from '@/router';

const props = defineProps(['item']);

function openItem() {
  console.log('Opening item:', props.item);
  router.push({ name: 'item', params: { id: props.item.id } });
}
</script>
<style scoped>
.item-card {
  border: 1px solid #ccc;
  padding: 10px;
  margin: 10px;
  display: inline-block;
  width: 200px;
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