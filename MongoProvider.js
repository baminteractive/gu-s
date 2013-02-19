var Db = require('mongodb').Db;
var Connection = require('mongodb').Connection;
var Server = require('mongodb').Server;
var BSON = require('mongodb').BSON;
var ObjectID = require('mongodb').ObjectID;

MongoProvider = function(){
	var host = process.env.mongohost || "localhost";
	var port = process.env.mongoport || "27017";
	var user = process.env.mongouser || "";
	var password = process.env.mongopassword || "";
	var database = process.env.mongodatabase || "geoip";

	console.log("Host: " + host + " Port: " + port + " User: " + user + " Database: " + database);

  this.db = new Db(database, new Server(host,port,{auto_reconnect: true}),{w:0});
  this.db.open(function(err,db){
  	if(err) { return console.dir(err); }

  	if(db){
  		if(user != ""){
	  		db.authenticate(user,password,function(err,result){
	  			if(err){ return console.dir(err); }
	  		});
	  	}
  	}
  });
};

MongoProvider.prototype.collection = function(collectionname,callback){
  this.db.collection(collectionname,function(err,collection){
    if(err) { callback(err); }
    else { callback(null,collection); }
  });
}

MongoProvider.prototype.save = function(countries,callback){
	this.collection(function(err, collection){
		if(err){ 
			callback(err)
		}
		else{
			collection.insert(country,function(err){
				callback(null,country);
			});
		}
	})
}

exports.MongoProvider = MongoProvider;