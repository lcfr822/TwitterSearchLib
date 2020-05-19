using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterCrawlerLib.Core.Data
{
    public class PlaceObject
    {
        public string id;
        public string url;
        public string place_type;
        public string name;
        public string full_name;
        public string country_code;
        public string country;
        public BoundingBox bounding_box;
        public Object attributes;
    }

    public class BoundingBox
    {
        public float[][][] coordinates;
        public string type;

        public BoundingBox(string newType, float[][][] newCoordinates)
        {
            type = newType;
            coordinates = newCoordinates;
        }

        public string GetTopCoordString()
        {
            return coordinates[0][0][0].ToString() + ", " + coordinates[0][0][1].ToString();
        }
    }
}
