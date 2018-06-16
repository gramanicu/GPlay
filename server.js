var express = require("express");
var bodyParser = require("body-parser");
var compression = require('compression');
var session = require('express-session');
var cookieParser = require('cookie-parser');
var randomstring = require('randomstring');
var fs = require("fs");
var path = require("path");
var convert = require("xml-js");

var port = 80;
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
/*
app.use(session({
    key: 'user_sid',
    secret: 'api',
    resave: false,
    saveUninitialized: false,
    cookie: {
        expires: 600000
    }
}));*/

var key = "1234";

app.get("/", function(req, res) {
    // res.render("home");
    res.sendStatus(200);
});

app.get('/api/games/general%key=:key&type=:type', function(req, res) {
    if (req.params.key == key) {
        //it doesnt matter the type, the response will be text
        res.sendFile(__dirname + '/serverFiles/library/gameList.txt');
    }
});

app.get('/api/games/about=:about%key=:key&type=:type', function(req, res) {
    var games = [];
    fs.readdirSync(__dirname + '/serverFiles/games').forEach(file => {
        games.push(file);
    });
    var game = "";

    if (req.params.key == key) {
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

app.get('/api/login/user=:user&password=:password', function(req, res) {
    if (req.params.user == user) {
        if (req.params.password == pass) {
            res.send("1234");
        } else res.sendStatus(403);
    } else res.sendStatus(403);
});

app.get('/api/signup/name=:name&password=:password', function(req, res) {
    user = req.params.name;
    pass = req.params.password;
    res.send("1234");
});

var user = "gramanicu";
var pass = "1234";


console.clear();
console.log("--------------------------------------------------------------------------------------");
console.log(" GPlay Server");
console.log("");
console.log(" Grama Nicolae - 2018");
console.log("--------------------------------------------------------------------------------------");
console.log("");
console.time("Server started in ");
console.log("Starting Server ...");


var server = app.listen(port, function() {
    console.log("");
    if (port != 80) {
        console.log('Server is now running. Go to localhost:' + port + ' on your browser');
    } else console.log('Server is now running. Go to localhost on your browser');
    console.timeEnd('Server started in ');
    console.log("");

    var games = [];
    fs.readdirSync(__dirname + '/serverFiles/games').forEach(file => {
        games.push(file);
    });
    var response = "";
    for (var i = 0; i < games.length; i++) {
        response += games[i].replace(".zip", '');
        if (i < games.length - 1) {
            response += " ; ";
        }
    }
    fs.writeFile("serverFiles/library/gameList.txt", response, function(err) {
        if (err) throw err;
        console.log('Updated the games list');
    });

    var MongoClient = require('mongodb').MongoClient;

    var password = "yourpassword";
    var uri = "yourURL";
    MongoClient.connect(uri, function(err, client) {
        if (err) {
            console.log('Error occurred while connecting to MongoDB Atlas...\n', err);
        }

        console.log('Connected to the database');


        var collection = client.db("GPlay").collection("games");

        var gameList = [];
        for (var i = 0; i < games.length; i++) {
            response = games[i].replace(".zip", '');
            var obj = JSON.parse(fs.readFileSync(__dirname + '/serverFiles/library/' + response + '/info.json', 'utf8'));
            gameList.push(obj);
        }

        collection.insertMany(gameList, function(err, res) {
            if (err == null) {
                console.log("Number of documents inserted: " + res.insertedCount);
            }
        });
        client.close();
    });

});