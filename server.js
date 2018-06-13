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

app.get("/", function (req, res) {
    res.render("home");
});

app.get('/api/key=:key&type=:type', function (req, res) {
    var response = {
        blue: Math.floor(Math.random() * 256),
        red: Math.floor(Math.random() * 256),
        green: Math.floor(Math.random() * 256)
    };
    console.log(response);
    if (req.params.key == key) {
        if (req.params.type == "json") {
            res.send(response).status(200);
        } else if (req.params.type == "xml") {
            res.send('<?xml version="1.0" encoding="utf-8"?>' +
            '<color>' +
            '    <blue value="' +response.blue +'"/>' +
            '    <red value="' + response.red +'"/>' +
            '    <green value="' +response.green+ '"/>' +
            '</color>');
        }
        else if (req.params.type == "text") {
            res.send("red="+  response.red + ";green=" + response.green + ";blue=" + response.blue);
        }
    } else {
        res.sendStatus(403);
    }
});

app.get('/api/login/user=:user&password=:password', function (req, res) {
    if(req.params.user==user)
    {
        if(req.params.password==pass)
        {
            res.send("1234");
        }
        else  res.sendStatus(403);
    }
    else res.sendStatus(403);
});

app.get('/api/signup/name=:name&password=:password', function (req, res) {
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


var server = app.listen(port, function () {
    console.log("");
    if (port != 80) {
        console.log('Server is now running. Go to localhost:' + port + ' on your browser');
    } else console.log('Server is now running. Go to localhost on your browser');
    console.timeEnd('Server started in ');
    console.log("");
});