# Dungeons and Dragons

## Description ##
Final project for Makers Academy.

An application to assist Dungeon Masters in the set up and running of a Dungeons and Dragons campaign.

## Technologies Used ##
* ASP.NET Core 2.2
* ASP.NET MVC 5
* .NET Framework 4.7.2
* .NET Standard 2
* NUnit
* PostgreSQL
* Entity Framework
* C#
* HTML
* CSS
* Bootstrap

## Process ##
Our [wiki](https://github.com/aimeecraig/dungeons-and-dragons/wiki) discusses our production process for this challenge.

## How to Install and Use ##
1. Clone the repository

```
git clone https://github.com/aimeecraig/dungeons-and-dragons.git
```

2. Navigate into the project directory
```
cd dungeons-and-dragons
```

## Databases Setup
Create a database called `dungeons_and_dragons` using PostgreSQL.

This can be done using the following command:

```bash
DATABASE CREATE dungeons_and_dragons
```

### Set up Connection String
Create an `appsettings.json` file in the `/dungeons-and-dragons/DungeonsAndDragons/DungeonsAndDragons` directory with the below code snippet. Ensure that you update the Username and Password key value pairs with the username and password you will be using to access your database.

```csharp
{
    "Logging": {
        "LogLevel": {
            "Default": "Warning"
        }
    },
    "AllowedHosts": "*",
    "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Port=5432;Username=username;Password=password;Database=dungeons_and_dragons;"
    }
}
```

### Migrate Table schemas
To import the tables and table schemas for the project, while in the main project directory (`/dungeons-and-dragons/DungeonsAndDragons/DungeonsAndDragons`) run the following command:

```bash
dotnet ef database update
```

### Setup Foreign Keys and Nullable Values
In each table manually set the following foreign keys and null requirements. Unless explicitly stated nullable should be set to false.

* ganes
  * dm => users.id
* gamesusers
  * gameid => games.id
  * userid => users.id
  * playablecharacterid => playablecharacters.id, nullable => true
* nonplayablecharacters
  * species_id => species.id
  * game_id => games.id
* playablecharacters
  * userid = users.id
  * species_id => species.id


### Import Games Base Data
In Table Plus, on the table that you are importing data to:
1. right click (two finger click)
2. From CSV..
3. select corresponding csv file in sql_tables directory
4. open
5. ensure first line is header is checked
6. import
7. refresh the table


## Contributors ##
* [Aimee Craig](https://github.com/aimeecraig)
* [John Littler](https://github.com/JSLittler)
* [Melissa Sedgwick](https://github.com/melissasedgwick)
* [Terry Mace](https://github.com/Tolvic)
