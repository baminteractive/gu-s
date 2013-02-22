using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using gu_s.Models;

namespace gu_s.Services
{
    public class MongoRepository
    {
        private readonly MongoCollection<Country> _countryCollection;

        public MongoRepository()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["GusDatabase"].ToString();
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(ConfigurationManager.AppSettings["DatabaseName"]);
           _countryCollection = database.GetCollection<Country>("countries");
        }

        public IEnumerable<Country> All()
        {
            return _countryCollection.FindAll();
        } 

        public IEnumerable<Country> Search(Expression<Func<Country, bool>> predicate)
        {
            return _countryCollection.AsQueryable().Where(predicate).ToList();
        }
    }
}