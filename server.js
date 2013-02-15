
/**
 * Module dependencies.
 */
var express = require('express')
  , routes = require('./routes')
  , http = require('http')
  , path = require('path')
  , cons = require('consolidate')
  , createdatabase = require('./createdatabase.js')
  , cronjob = require('cron').CronJob;
  


var app = express();

app.engine('html', cons.mustache);

app.configure(function(){
  app.set('port', process.env.PORT || 3000);
  app.set('views', __dirname + '/views');
  app.set('view engine', 'html');
  app.use(express.favicon());
  app.use(express.logger('dev'));
  app.use(express.bodyParser());
  app.use(express.methodOverride());
  app.use(app.router);
  app.use(express.static(path.join(__dirname, 'public')));
});

app.configure('development', function(){
  app.use(express.errorHandler());
});

app.get('/', routes.index);

http.createServer(app).listen(app.get('port'), function(){
  console.log("Express server listening on port " + app.get('port'));
  createdatabase.getDatabase();
});

var job = new cronjob({
  cronTime: '00 00 02 * * 3',
  onTick : function(){
    createdatabase.getDatabase();
  },
  start : true,
  timeZone : "America/New_York"
});
