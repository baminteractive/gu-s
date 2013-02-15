gu-s
=====

gu-s, pronounced 'goose,' is a node.js application which takes a request with an ip as a query string `http://localhost?ip=123.123.123.123` and returns a JSON response with the country for that IP.  gu-s uses the GeoIp Lite database as the data source and will update the database on a weekly basis using a cron job. 