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
DATABASE CREATE dungeons_and_dragons;
```

### Setup Connection
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

### Migrate Table Schemas
To import the tables and table schemas for the project, while in the main project directory (`/dungeons-and-dragons/DungeonsAndDragons/DungeonsAndDragons`) run the following command:

```bash
dotnet ef database update
```

### Setup Foreign Keys and Nullable Values
In each table manually set the following foreign keys and null requirements. Unless explicitly stated nullable should be set to false.

* games
  * dm => users.id
* gamesusers
  * gameid => games.id
  * userid => users.id
  * playablecharacterid => playablecharacters.id, nullable => true
* nonplayablecharacters
  * species_id => species.id
  * game_id => games.id
* playablecharacters
  * userid => users.id
  * species_id => species.id

### Import Games Base Data
There are files in the `sql_tables` directory that need to be imported to their respective tables.

In TablePlus, on the table that you are importing data to:
1. Truncate the table you are importing into (ensuring that Restart Identity is checked).
2. Right click (two finger click) the table in the left panel.
3. Import > From CSV...
4. Select the corresponding .csv file in `sql_tables` directory.
5. Click Open.
6. Ensure "First line is header" is checked.
7. Click Import.
8. Refresh the table.

## Contributors ##
* [Aimée Craig](https://github.com/aimeecraig)
* [John Littler](https://github.com/JSLittler)
* [Melissa Sedgwick](https://github.com/melissasedgwick)
* [Terry Mace](https://github.com/Tolvic)
