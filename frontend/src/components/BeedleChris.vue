<template>
  <div class="container">
    <div class="left"><img id="rupee" src="@/assets/rupee.webp">
      <p>x {{ store.user.balance }}</p>
    </div>
    <div class="middle">
      <div v-if="props.item" class="middle-container">
        <h3>How many {{ props.item.name }} would you like to buy?</h3>
        <input type="number" v-model="state.quantity">
        <h5>{{ (props.item.price*state.quantity) }} rupees</h5>
        <button @click="buyItem">Buy</button>
      </div>
      <div class="item-purchased">
        <h2>Thaaaaaaaaaank you!!!</h2>
        <h3>Feel free to buy anything else!</h3>
      </div>
    </div>
    <div class="right"><img src="@/assets/beedle2.webp" alt=""></div>

  </div>
</template>
<script setup>
import { reactive } from 'vue';
import {store, updateItems, updateUser} from '@/store';

const props = defineProps(['item']);
const emits = defineEmits(['itemPurchased']);
const state = reactive({
  quantity: 0,
  itemPurchased: false
});

function buyItem() {
// function def we're calling
// app.MapPost("/purchase", async(int userId, int itemId, int quantity, UserDb userDb, ItemDb itemDb)=>{
  console.log(store.user.id, props.item.id, state.quantity);
  fetch(store.uri + 'purchase', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      userId: store.user.id,
      itemId: props.item.id,
      quantity: state.quantity
    })
  })
    .then(response => response.json())
    .then(data => {
      
      alert('Purchase successful!');
      updateItems();
      updateUser();
      emits('itemPurchased');
      state.itemPurchased = true;

    })
    .catch(error => console.error('Error:', error));
}

</script>
<style scoped>
img {
  width: 200px;
  height: 200px;
  margin: 10px;
}

.container {
  display: flex;
  margin-top: 10rem;
  flex-direction: row;
  justify-content: bottom;
  align-items: center;
}

.left {
  flex: 1;
  background-color: #42b983;
  display: flex;
  justify-content: center;
  align-items: center;
}

.middle {
  flex: 3;
  background-color: #42b983;
}

.right {
  flex: 1;

}

#rupee {
  width: 50px;
  height: 50px;
}

p {
  font-size: 50px;
}
</style>