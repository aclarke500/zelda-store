<template>
  <div v-if="store.user" class="home">

    <h1 @click="router.push({ name: 'login' })">Beedle's Online Shop</h1>
    <button @click="fetchData">Refresh</button>
    <AddItem v-if="store.user.name == 'Beedle'" @item-added="fetchData" />

    <div class="frame">
    <div class="left">
      <img id="rupee" src="@/assets/rupee.webp">
      <p>x {{ store.user.balance }}</p>
    </div>

    <div class="card-container" v-if="state.displayItems">
      <DisplayCategoryCards v-for="word in state.wordClasses" :key="word" :category="word"
        @select="(i) => { state.selectedItem = i }" />
    </div>
    <div class="right">
      
      <div v-if="state.selectedItem">
        <h3>How many {{ state.selectedItem?.name }} would you like to buy?</h3>
        <input type="number" v-model="state.quantity">
        <h5>{{ (state.selectedItem?.price * state.quantity) }} rupees</h5>
        <button @click="purchase">Buy</button>
      </div>
      <img id="beedle" src="@/assets/beedle2.webp" alt="">
      </div>

  </div>




  </div>
  <BeedleChris :item="state.selectedItem" @item-purchased="state.selectedItem = null" />

</template>

<script setup>
import { reactive } from 'vue';
import AddItem from '@/components/AddItem.vue';
import DisplayCategoryCards from '@/components/DisplayCategoryCards.vue';
import router from '@/router';
import { store, updateUser } from '@/store';

const state = reactive({
  selectedItem: null,
  wordClasses: ['Food', 'Weapons', 'Consumables', 'Miscellaneous'],
  quantity: 0,
  displayItems: true,

});

/**
 * Fetches the items from the server
 */
function getWordClass() {
  displayItems = false;
  fetch(store.aiUrl, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({ word: state.tempWord })
  }).then(response => response.json())
    .then(data => {
      console.log(data);
      state.class = data.class;
    })
    .catch(error => console.error('Error:', error));
}

/**
 * Updates the purchase in db, and updates the user's balance on vue
 */
async function purchase(){
  fetch(store.uri + 'purchase', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      userId: store.user.id,
      itemId: state.selectedItem.id,
      quantity: state.quantity
    })
  })
    .then(response => response.json())
    .then(data => {
      debugger
      if (!data.ok) {
        throw data;

      }
      alert('Purchase successful!');
      state.displayItems = true;
      updateUser();
      emits('itemPurchased');
      state.itemPurchased = true;

    })
    .catch(error =>{
      console.error('Error:', error);
      alert(error.message);
    });
}
</script>

<style scoped>
h1 {
  font-size: 50px;
  font-family: "Hylia Serif", serif;
  color: #2c3e50;
  margin: 2rem;
  padding-top: 4rem;
  margin-bottom: 0;
}
.frame{
  display: flex;
  flex-direction: row;
}

.left {
  flex: 1;
  background-color: #42b983;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}
.right {
  background-color: #42b983;
  flex: 1;
  display: flex;
  flex-direction: column;

}

img {
  width: 200px;
  height: 200px;
  margin: 10px;
}
#rupee {
  width: 50px;
  height: 50px;
  display: inline;
}

p {
  font-size: 20px;
  display: inline;
}
</style>
