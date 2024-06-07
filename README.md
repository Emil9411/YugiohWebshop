# YugiohWebshop Documantation

## Table of contents
- [Introduction](#introduction)
- [Usage](#usage)
- [API](#api)
- [Used external API](#used-external-api)

## Introduction
This is a webshop for Yugioh cards. The webshop is build with React. 
The webshop is connected to a backend server that is build with ASP.NET. 
The backend server is connected to a MS SQL database. 
The webshop has a login and register system, a shopping cart system, a search system, a filter system, a pagination system and an admin system. 
The admin system has a product, user, and order management system.

## Usage
To use the webshop, users need to register an account. After registering an account, users can login. 
After logging in, users can search for products, filter products, see more information about a product by clicking on the product, see their account information. Users can also logout.
Meanwhile, the admin can login. After logging in, the admin can fill up the database with one button, update it with another, or clear it with a third button. The admin can see, and manage all users, see user information, delete users.

## API
The API can be used to get data from the database. The API has the following endpoints:

### Card
- `GET` `api/Card/filldatabase` - Fill the database with cards. Admin only.
- `PATCH` `api/Card/updatedatabase` - Update the database with cards. Admin only.
- `DELETE` `api/Card/cleandatabase` - Clear the database with cards. Admin only.
- `GET` `api/Card/allcards` - Get all cards. Anyone.
- `GET` `api/Card/allmonstercards` - Get all monster cards. User and Admin.
- `GET` `api/Card/allspellcards` - Get all spell cards. User and Admin.
- `GET` `api/Card/cardbyname/{name}` - Get a card by name. User and Admin.
- `GET` `api/Card/cardbyid/{id}` - Get a card by id. User and Admin.
- `GET` `api/Card/randomcard` - Get a random card. Anyone.

### Auth
- `POST` `api/Auth/register` - Register a user. Anyone.
- `POST` `api/Auth/login` - Login a user. Anyone.
- `PATCH` `api/Auth/changepassword` - Change the password of a user. User and Admin.
- `PATCH` `api/Auth/changeemail` - Change the email of a user. User and Admin.
- `GET` `api/Auth/whoami` - Get the user that is logged in. User and Admin.
- `POST` `api/Auth/logout` - Logout a user. User and Admin.

### User
- `GET` `api/User/getusers` - Get all users. Admin only.
- `GET` `api/User/getuserbyemail/{email}` - Get a user by email. User and Admin.
- `POST` `api/User/addadminuser` - Add an admin user. Admin only.
- `DELETE` `api/User/deleteuser/{email}` - Delete a user by email. User and Admin.
- `PATCH` `api/User/updateuser` - Update a user. User and Admin.
- `DELETE` `api/User/deleteuseradmin/{email}` - Delete a user by email. Admin only.}

## Used external API
The webshop uses the [Yugioh API](https://db.ygoprodeck.com/api-guide/) to get the cards.
