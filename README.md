# zelda-store

Simple CRUD app using dotnet Sqlite3 for a backend. Mostly for practicing how to use dotnet, the front-end is very rushed.

## Backend 
All entities are defined in Entities.cs to reduce number of files. Users are kept track of by Id, and log in with their names (not meant to be a practical secure website, Beedle is a pretty chill guy). Items have an Id, name, price and quantity. In the Program.cs file there are API calls for the frontend to simulate logging in and purchasing items. 

### Logging in
When a user logs in, if they do not have an account, one is created for them and they are given 100 rupees. 

### Purchasing
When a user purchases an item, the Program.cs has a "/purchase" route that ensures they have enough balance and there are enough items. It would have been easier and more eloquant to enforce this on the front end, but again this is about dotnet practice. 

### Modifying Database
In order to get free range at modifiyng the database, login as Beedle. When you try and purchase an item, you can just change it's properties.

## Frontend 
There are three pages a user can be on:

### Login
The input field sends the name to the backend. If the name exists, they are logged in. Else an account is created for them.
### HomeView
This is where the shop is. Users can select items and purchase them, and their account information is updated in the backend and on the screen.
### Update Items
A secret view for Beedle where he can update the data about his store. 

Global state is handled in store.cs. This was used pretty liberally as the size of this app doesn't force me to use SWE best practices.
