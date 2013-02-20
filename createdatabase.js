var MongoProvider = require('./MongoProvider').MongoProvider
  , unzip = require('unzip')
  , fs = require('fs')
  , http = require('http')
  , url = require('url')
  , csv = require('csv');  

console.log("Getting Mongo Collection");
var mongo = new MongoProvider('localhost',27017);



console.log("Get Database");

function getdatabase(){
  // See if there is a last downloaded date
  mongo.collection('meta',function(err,collection){
    if(err){ console.dir(err); }

    console.log("Got meta collection");
    collection.findOne({}, function(err, item){
      if(err) { console.log(err); }

      if(item){
        console.log("Found meta item");
        var thresholddate = new Date(); // Set to today
        thresholddate.setDate(thresholddate.getDate() - 5); // Roll back date to 5 days, our threshold

        if(item.lastupdate > thresholddate){
          return console.log("Database is up to date");
        }
      }

      console.log("Database needs to be updated");
      updateDatabase();
    });
  });
}
var updateDatabase = function(){
  console.log("starting to update database");
  mongo.collection('countries',function(err,collection){
    if(err) { console.dir(err); }
    console.log("Cleaning countries collection");
    collection.remove(function(){});
  }) 

  console.log("Getting geo data");
  var downloadfile = "http://geolite.maxmind.com/download/geoip/database/GeoIPCountryCSV.zip";

  if(fs.existsSync('GeoIPCountryCSV.zip')){
    fs.unlinkSync('GeoIPCountryCSV.zip');
  }

  if(fs.existsSync('./output/GeoIPCountryWhois.csv')){
    fs.unlinkSync('./output/GeoIPCountryWhois.csv');
  }

  var host = url.parse(downloadfile).hostname;
  var filename = url.parse(downloadfile).pathname.split('/').pop();

  var options = {
    hostname:host,
    port:80,
    path:downloadfile,
    method:'GET'
  }
  var request = http.request(options,function(response){
    processResponse(response,filename);
  });
  request.end();

  dlprogress = 0; 
}

var processRecord = function(row, index){
    var startGroup = row[0].split('.');
    var endGroup = row[1].split('.');
    var recordToInsert = {
      startAddress : row[0],
      endAddress : row[1],
      startFirstOctet : parseInt(startGroup[0]),
      startSecondOctet : parseInt(startGroup[1]),
      startThirdOctet : parseInt(startGroup[2]),
      endFirstOctet : parseInt(endGroup[0]),
      endSecondOctet : parseInt(endGroup[1]),
      endThirdOctet : parseInt(endGroup[2]),
      countryAbbr : row[4],
      country : row[5]
    };

    mongo.collection('countries',function(err,collection){
      if(err) { console.dir(err); }
      collection.insert(recordToInsert, function(err, result){
        if(err) { console.log(err); }
      });
    });
  }

  var processResponse = function(response,filename) {
    var downloadfile = fs.createWriteStream(filename, {'flags': 'a'});

    response.on('data', function (chunk) {
        dlprogress += chunk.length;
        downloadfile.write(chunk, encoding='binary');
    });
    response.on("end", function() {
        downloadfile.end();
        console.log("Download has finished");

      fs.createReadStream(filename)
        .pipe(unzip.Extract({path:'output/'})
            .on('close',function(){
              console.log("File has been unarchived");
              csv().from.stream(fs.createReadStream('./output/GeoIPCountryWhois.csv'))
                .on('record',processRecord)
                .on('end', function(count){
                  console.log("Processed "+ count + " records");
                  mongo.collection('meta',function(err,collection){
                    console.log("Updating meta record");
                    // Check if there is already a record
                    collection.findOne({},function(err,item){
                      var updateItem = item || {};
                      updateItem.lastupdate = new Date();
                      collection.save(updateItem);
                    });
                  });

                })
                .on('error',function(err){
                  console.log("ERROR ERROR ERROR || "+err.message);
                });
            })
          );

    });

  }

getdatabase();
