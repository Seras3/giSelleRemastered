# GiSelle 🃏

An marketplace for tattoo artists.


[**Overviews**](https://github.com/Seras3/giSelleRemastered#overview-from-user-account)
  - [User](https://github.com/Seras3/giSelleRemastered#overview-from-user-account)
  - [Provider](https://github.com/Seras3/giSelleRemastered#overview-from-provider-account)
  - [Admin](https://github.com/Seras3/giSelleRemastered#overview-from-admin-account)

[**Features**](https://github.com/Seras3/giSelleRemastered#features)



## Overview (from user account)

### Product search
![user_main](https://github.com/Seras3/giSelleRemastered/blob/master/images/user-main.JPG)

### Product view
![user_product](https://github.com/Seras3/giSelleRemastered/blob/master/images/user-product.JPG)

### Cart view
![cart_view](https://github.com/Seras3/giSelleRemastered/blob/master/images/user-cart.JPG)

## Overview (from provider account)

### Product search
![provider_main](https://github.com/Seras3/giSelleRemastered/blob/master/images/provider-main.JPG)

### Edit post
   scroll-up            |   scroll-down
:-------------------------:|:-------------------------:
![edit_product_1](https://github.com/Seras3/giSelleRemastered/blob/master/images/provider-edit-1.JPG)  |  ![edit_product_2](https://github.com/Seras3/giSelleRemastered/blob/master/images/provider-edit-2.JPG)


## Overview (from admin account)

### Admin links

![admin_links](https://github.com/Seras3/giSelleRemastered/blob/master/images/admin-links.JPG)

### Request list

![request_list](https://github.com/Seras3/giSelleRemastered/blob/master/images/admin-request-list.JPG)

### Request view

![request_view](https://github.com/Seras3/giSelleRemastered/blob/master/images/admin-request-product.JPG)

### Change user role

![admin_user](https://github.com/Seras3/giSelleRemastered/blob/master/images/admin-user.JPG)

### Product view

![admin_product_view](https://github.com/Seras3/giSelleRemastered/blob/master/images/admin-product.JPG)



## Features
There are 4 main entities involved in this hierarchy. (Guest, User, Provider, Admin)

**Guest :**
  - search for products (filters included)
  - view product and comments
  - create an account
 
**User :** (inherit from Guest)
  - buy product
  - add/update/delete comment on post
  - add/clear rating for a product
  - add/delete product in/from shopping cart
  - place an order
  
**Provider :** (inherit from User)
  - request to add product post
  - update/delete product post

**Admin :** (inherit from Provider)
  - delete any post
  - delete any comment
  - change user role
  - approve add product post request
  - add/update/delete product categories

