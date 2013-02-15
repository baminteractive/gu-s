var MongoProvider = require('./MongoProvider').MongoProvider
  , unzip = require('unzip')
  , fs = require('fs')
  , http = require('http')
  , url = require('url')
  , csv = require('csv');  

var mongo = new MongoProvider('localhost',27017);

exports.getDatabase = function(){
  
  console.log("Getting ips");

  // See if there is a last downloaded date
  mongo.collection('meta',function(err,collection){
    if(err){ console.dir(err); }

    console.log("Fetching meta data");

    console.log("Meta Count: ",collection.count());

    collection.findOne({}, function(err, item){
      if(err) { console.log(err); }

      if(item){
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
 console.log("Cleaning database");

  /*mongo.connect("mongodb://localhost:27017/geoip",function(err,db){
    if(err) { return console.log(err); }
    db.collection('countries').remove(function(){ console.log("database clean"); });
  });*/

  mongo.collection('countries',function(err,collection){
    if(err) { console.dir(err); }
    collection.remove();
  }) 

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
  console.log("Making request");
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
        console.log(result);
      });
    });
  }

  var processResponse = function(response,filename) {
    var downloadfile = fs.createWriteStream(filename, {'flags': 'a'});
    console.log("File size " + filename + ": " + response.headers['content-length'] + " bytes.");

    response.on('data', function (chunk) {
        dlprogress += chunk.length;
        downloadfile.write(chunk, encoding='binary');
    });
    response.on("end", function() {
        downloadfile.end();
        console.log("Finished downloading " + filename);

      fs.createReadStream(filename)
        .pipe(unzip.Extract({path:'output/'})
            .on('close',function(){
              console.log("Finished extracting archive");
              csv().from.stream(fs.createReadStream('./output/GeoIPCountryWhois.csv'))
                .on('record',processRecord)
                .on('end', function(count){
                  console.log("Number of rows: "+count);
                  mongo.collection('meta',function(err,collection){
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