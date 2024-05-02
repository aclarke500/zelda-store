# zelda-store

Simple CRUD app using dotnet Sqlite3 for a backend. Mostly for practicing how to use dotnet, the front-end is very rushed.

## Backend Archiecture
All entities are defined in Entities.cs to reduce number of files. Users are kept track of by Id, and log in with their names (not meant to be a practical secure website, Beedle is a pretty chill guy). Items have an Id, name, price and quantity. In the Program.cs file there are API calls for the frontend to simulate logging in and purchasing items. 

### Logging in
When a user logs in, if they do not have an account, one is created for them and they are given 100 rupees. 

### Purchasing
When a user purchases an item, the Program.cs has a "/purchase" route that ensures they have enough balance and there are enough items. It would have been easier and more eloquant to enforce this on the front end, but again this is about dotnet practice. 

