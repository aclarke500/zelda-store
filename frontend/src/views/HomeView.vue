<template>
  <div v-if="store.user" class="home">

    <!-- <button @click="login">Login</button> -->
    <h1 @click="router.push({name:'login'})">Beedle's Online Shop</h1>
    <button @click="fetchData">Refresh</button>
    <AddItem v-if="store.user.name == 'Beedle'" @item-added="fetchData" />
    <ItemCardDisplay id="cards" @select="(i)=>state.selectedItem=i"/>

  </div>
  <BeedleChris :item="state.selectedItem" @item-purchased="state.selectedItem = null"/>
</template>

<script setup>
import {reactive} from 'vue';
import AddItem from '@/components/AddItem.vue';
import ItemCardDisplay from '@/components/ItemCardDisplay.vue';
import BeedleChris from '@/components/BeedleChris.vue';
import router from '@/router';
import {store } from '@/store';

const state = reactive({
  selectedItem: null
});

function login(){
  router.push('/login');
}
function passItem(item){
  state.selectedItem = item;
}


</script>

<style scoped>
h1 {
  font-size: 50px;
  font-family: "Hylia Serif", serif;
  color: #2c3e50;
  margin: 2rem;
  padding-top:4rem;
}
#cards{
  display: flex;
  flex-wrap: wrap;
  justify-content: center;

}


@media (max-width: 900px) {
  .card {
    flex: 0 0 50%;
    /* 2 cards per row on smaller screens */
  }
}

@media (max-width: 600px) {
  .card {
    flex: 0 0 100%;
    /* 1 card per row on very small screens */
  }
}
</style>
