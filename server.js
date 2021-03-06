var express = require("express");
var bodyParser = require("body-parser");
var compression = require('compression');
var session = require('express-session');
var cookieParser = require('cookie-parser');
var randomstring = require('randomstring');
var fs = require("fs");
var path = require("path");
var convert = require("xml-js");
var MongoClient = require('mongodb').MongoClient;



var collectionUsers;
var collectionGames;
var config = require(path.join(__dirname, '/serverFiles/settings/config.json'));

var port = config.serverSettings.port;
var app = express();

app.set('view engine', 'ejs');
app.set('views', path.join(__dirname, 'serverFiles/views'));
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
    extended: false
}));
app.use(compression());
app.use('/static', express.static(path.join(__dirname, 'serverFiles/public')));
app.use('/games', express.static(__dirname + '/serverFiles/games'));
app.use(cookieParser());



app.get("/", function (req, res) {
    res.sendStatus(200);
});

app.get("/api/games/download/:key", function (req, res) {
    collectionGames.findOne({
        gameKey: req.params.key
    }, function (error, game) {
        if (error == null) {
            res.sendFile(path.join(__dirname, 'serverFiles/games/CS Go 1.6.zip'));
        }
    });
});


//#region API routes

app.get('/api/games/general%key=:key&type=:type', function (req, res) {
    if (req.params.key != null) {
        collectionUsers.findOne({
            key: req.params.key
        }, function (err, response) {
            if (err == null) {
                var gameList = "";
                for (var i = 0; i < response.ownedGames.length; i++) {
                    gameList += response.ownedGames[i];
                    if (i < response.ownedGames.length - 1) {
                        gameList += " ; ";
                    }
                }
                res.send(gameList);
            }
        });
    }
});

app.get('/api/games/about=:about%key=:key&type=:type', function (req, res) {
    var games = [];
    fs.readdirSync(__dirname + '/serverFiles/games').forEach(file => {
        games.push(file);
    });
    var game = "";

    collectionUsers.findOne({
        key: req.params.key
    }, function (err, response) {
        if (err == null) {
            for (var i = 0; i < games.length; i++) {
                game = games[i].replace(".zip", '');
                game = game.replace(/ /g, "_");
                if (req.params.about == game) {
                    game = games[i].replace(".zip", '');
                    if (req.params.type == "text") {
                        res.sendFile(__dirname + '/serverFiles/library/' + game + "/info.json");
                    } else if (req.params.type == "image") {
                        res.sendFile(__dirname + '/serverFiles/library/' + game + "/logo.png");
                    }
                }
            }
        }
    });
});

//i need to change all the download process ( by key, not by name)
app.get('/api/buy/key=:key&title=:gameTitle', function (req, res) {
    collectionUsers.findOne({
        key: req.params.key
    }, function (err, user) {
        if (err == null) {
            collectionGames.findOne({
                nmae: req.params.gameTitle
            }, function (error, game) {
                user.ownedGames.push(game.gameKey);
            });
        }
    });
});

app.get('/api/login/user=:user&password=:password', function (req, res) {
    collectionUsers.findOne({
        username: req.params.user,
        password: req.params.password
    }, function (err, response) {
        if (err == null && response != null) {
            res.send(response.key);
        } else res.sendStatus(403);
    });
});

//Signup route
app.get('/api/signup/name=:name&password=:password', function (req, res) {
    var key = randomstring.generate(config.serverSettings.keyLenght);
    var gameList = ["CS Go 1.6", "Half-Life 3", "Zephyr"];
    var newUser = {
        "username": req.params.name,
        "password": req.params.password,
        "key": key,
        "ownedGames": gameList
    }

    collectionUsers.insertOne(newUser, function (err, response) {
        if (err == null) {
            console.log("added a new user");
            res.send(key);
        } else res.sendStatus(403);
    });
});

//#endregion

console.clear();
console.log("--------------------------------------------------------------------------------------");
console.log(" GPlay Server");
console.log("");
console.log(" Grama Nicolae - 2018");
console.log("--------------------------------------------------------------------------------------");
console.log("");
console.time("Server started in ");
console.log("Starting Server ...");


var server = app.listen(port, function () {
    console.log("");
    if (port != 80) {
        console.log('Server is now running. Go to localhost:' + port + ' on your browser');
    } else console.log('Server is now running. Go to localhost on your browser');
    console.timeEnd('Server started in ');
    console.log("");

    connectToAlas();
});

function connectToAlas() {
    var games = [];
    fs.readdirSync(__dirname + '/serverFiles/games').forEach(file => {
        games.push(file);
    });

    MongoClient.connect(config.serverSettings.atlasURL, function (err, client) {
        if (err) {
            console.log('Error occurred while connecting to MongoDB Atlas...\n', err);
        }
        console.log('Connected to the database');
        collectionGames = client.db("GPlay").collection("games");
        collectionUsers = client.db("GPlay").collection("users");
        var gameList = [];
        for (var i = 0; i < games.length; i++) {
            response = games[i].replace(".zip", '');
            var obj = JSON.parse(fs.readFileSync(__dirname + '/serverFiles/library/' + response + '/info.json', 'utf8'));
            gameList.push(obj);
        }

        collectionGames.insertMany(gameList, function (err, res) {
            if (err == null) {
                console.log("Number of documents inserted: " + res.insertedCount);
            }
        });
    });

}