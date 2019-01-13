# Castles and Kittens #

The site can be reached by visiting [http://bit.do/castlesandkittens](http://bit.do/castlesandkittens) or by scanning the QR code below.

![qr-code](images/qr.png)

## Description ##
An application to assist Dungeon Masters in the set up and running of a Dungeons and Dragons campaign.

## Tech Stack ##
* ASP.NET Core 2.1.5
* Entity Framework 6.2
* SignalR 2.4
* PostgreSQL
* C#
* HTML
* CSS
* Bootstrap
* JavaScript
* jQuery

## Workflow Process ##
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

Follow the next steps to create a local database.

## Database Setup ##
1. Create a database called `dungeons_and_dragons` using PostgreSQL.
```bash
DATABASE CREATE dungeons_and_dragons;
```

### Setup Connection ###
Create an `appsettings.json` file in the `/dungeons-and-dragons/DungeonsAndDragons/DungeonsAndDragons` directory using the below code snippet. Ensure that you update the `Username` and `Password` key value pairs with the username and password you will be using to access your local database.

*This is usually the same username you use to log in to your machine with no password.*

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

### Migrate Table Schemas ###
To import the tables and table schemas for the project, while in the main project directory `/dungeons-and-dragons/DungeonsAndDragons/DungeonsAndDragons` run the following command:
```bash
dotnet ef database update
```

### Setup Foreign Keys and Nullable Values ###
In each table set the following foreign keys and null requirements. Unless explicitly stated nullable should be set to false.

* games
  * dm => users.id
* gamesusers
  * gameid => games.id
  * userid => users.id
  * playablecharacterid => playablecharacters.id, nullable => true
* inventory
  * chracterid => playablecharacters.id
  * inventoryItemId => inventoryitems.id
* nonplayablecharacters
  * species_id => species.id
  * game_id => games.id
* playablecharacters
  * userid => users.id
  * species_id => species.id
  
Foreign Keys can be set by running the following SQL commands:

```SQL
ALTER TABLE games
ADD FOREIGN KEY (dm) REFERENCES users(id);

ALTER TABLE gamesusers
ADD FOREIGN KEY (gameid) REFERENCES games(id),
ADD FOREIGN KEY (userid) REFERENCES users(id),
ADD FOREIGN KEY (playablecharacterid) REFERENCES playablecharacters(id);

ALTER TABLE inventory
ADD FOREIGN KEY (chracterid) REFERENCES playablecharacters(id),
ADD FOREIGN KEY (inventoryItemId) REFERENCES inventoryitems(id);

ALTER TABLE nonplayablecharacters
ADD FOREIGN KEY (species_id) REFERENCES species(id),
ADD FOREIGN KEY (game_id) REFERENCES games(id);

ALTER TABLE playablecharacters
ADD FOREIGN KEY (userid) REFERENCES users(id),
ADD FOREIGN KEY (species_id) REFERENCES species(id);
```

### Import Pre-existing Data ###
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
