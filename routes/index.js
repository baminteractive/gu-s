var MongoProvider = require('../MongoProvider').MongoProvider
,url = require('url');

var mongo = new MongoProvider("localhost",27017);

exports.index = function(req, res){
	var country = "";
	var countryabbr = "";
	var ip = "";

	var urlParts = url.parse(req.url,true);
	var query = urlParts.query;

	if(query.ip){
		mongo.collection('countries',function(err,collection){
			// Validate ip address
			var regex = /^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$/;
			var result = query.ip.match(regex);
			if(result){
				ip = query.ip;
				ipParts = ip.split('.');

				collection.find({
					startFirstOctet : parseInt(ipParts[0]), 
					startSecondOctet : { $lte : parseInt(ipParts[1]) }, 
					endSecondOctet : { $gte : parseInt(ipParts[1]) }, 
					startThirdOctet : { $lte : parseInt(ipParts[2])}, 
					endThirdOctet : { $gte : parseInt(ipParts[2])} })
					.toArray(function(err,items){
						if(items != null && items[0]){
							if(process.env.ENV_VARIABLE != "production"){
								console.log(items[0]);
							}

							country = items[0].country;
							countryabbr = items[0].countryAbbr;					
							res.json({country:country,abbr:countryabbr,ip:ip});
						}else{
							console.log("No items could be found for that ip.  [IP:"+ip+"]");
							sendResponse(res,country,countryabbr,ip);
						}
				}); 
			}
		});
	}
	else{
		sendResponse(res,country,countryabbr,"");
	}
};	


function sendResponse(res,country,abbr,ip){
	res.json({country:country,abbr:abbr,ip:ip});
}