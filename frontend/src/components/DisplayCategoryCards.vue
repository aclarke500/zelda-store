<template>
<h2>{{ props.category }}</h2>
<div v-if="state.items" class="item-container">
  <div class="item-cards" v-if="state.items" :key="state.items">
    <ItemCard class="card" v-for="item in state.items" :key="item" :item="item" @select="(i)=>{onItemSelect(i)}"/>
  </div>
</div>
</template>
<script setup>
import { onMounted, reactive } from 'vue';
import ItemCard from '@/components/ItemCard.vue';

const props = defineProps(['category']);
const emits = defineEmits(['select']);
const state = reactive({
  items: null,
});

function onItemSelect(item) {
  emits('select', item);
}

onMounted(async () => {
  const response = await fetch(`http://localhost:5016/items/category/${props.category}`);
  const data = await response.json();
  state.items = data;
});
</script>

<style scoped>
.item-container{
  /* display: flex;
  flex-direction: column;
  flex-wrap: wrap;
  justify-content: left;
  width: 1000px;
  align-items: center; */
}
h2{
  margin: 0;
}
</style>