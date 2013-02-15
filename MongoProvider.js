var Db = require('mongodb').Db;
var Connection = require('mongodb').Connection;
var Server = require('mongodb').Server;
var BSON = require('mongodb').BSON;
var ObjectID = require('mongodb').ObjectID;

MongoProvider = function(host,port){
  this.db = new Db('geoip', new Server(host,port,{auto_reconnect: true}),{w:1});
  this.db.open(function(){});
};

MongoProvider.prototype.collection = function(collectionname,callback){
  this.db.collection(collectionname,function(err,collection){
    if(err) { callback(err); }
    else { callback(null,collection); }
  });
}

MongoProvider.prototype.save = function(countries,callback){
	this.collection(function(err, collection){
		if(err) callback(err);

		else{
			collection.insert(country,function(){
				callback(null,country);
			});
		}
	})
}

exports.MongoProvider = MongoProvider;