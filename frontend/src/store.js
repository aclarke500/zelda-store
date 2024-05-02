import { reactive } from "vue";
 
export const store = reactive({
  uri:'http://localhost:5016/',
  user:null,
  items:[],
});

export async function updateUser(){
  fetch(store.uri+'users/login/?name='+store.user.name, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(store.user.name)
  })
    .then(response => response.json())
    .then(data => {
      console.log(data);
      store.user = data;
      router.push('/');
    })
    .catch(error => console.error('Error:', error));
}


export async function updateItems(){
  fetch(store.uri+'items/', {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json'
    },

  })
    .then(response => response.json())
    .then(data => {
      console.log(data);
      // reset array without losing pointer
      store.items.length=0
      data.forEach(element => {
        store.items.push(element);
      });
    })
    .catch(error => console.error('Error:', error));
    console.log(store.items, "shalah");
}

