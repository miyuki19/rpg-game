# RPG Game API using .NET Core MVC

This is an RPG game API built using .NET Core MVC and utilizing .NET 7 SDK. The game uses SQLite for data storage and Swagger for API documentation. Authentication is implemented using JWT.

## Prerequisite

- .NET 7 SDK
- Sqlite

## API Documentation

The API documentation can be accessed by running the application and visiting the following URL:

```
https://localhost:<port>/swagger
```

![swagger_test.png](https://github.com/miyuki19/rpg-game/blob/main/swagger_test.PNG)

## Security

JWT authentication is implemented using the Microsoft.AspNetCore.Authentication.JwtBearer package.
The .env file contains the secret key used for JWT generation and validation.

## Usage

The API contains the following endpoints:

## User Registration

POST /api/User/Register
Allows users to register by providing a username and password in the request body.

Example Request Body:

```
{
    "username": "username",
    "password": "password"
}
```

## User Login

POST /api/User/Login
Allows users to login by providing a username and password in the request body. Returns a JWT token to be used for authentication.

Example Request Body:

```
{
    "username": "username",
    "password": "password"
}
```

## Character Creation, Update, Remove, Add Weapon/ Skills

## Fight (Attack by weapon, skill, auto combat)


