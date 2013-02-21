using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace gu_s.Models
{
    public class Country
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CountryId { get; set; }
        public string StartAddress { get; set; }
        public string EndAddress { get; set; }
        public int StartFirstOctet { get; set; }
        public int StartSecondOctet { get; set; }
        public int StartThirdOctet { get; set; }
        public int EndFirstOctet { get; set; }
        public int EndSecondOctet { get; set; }
        public int EndThirdOctet { get; set; }
        public string CountryName { get; set; }
        public string CountryAbbr { get; set; }
    }
}