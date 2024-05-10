namespace server.Helper
{
    public class Coordinate
    {
        private Action<object> value;

        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Firebaseid { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Coordinate(string name, string mobile, string firebaseid, double latitude, double longitude)
        {
            Name = name;
            Mobile = mobile;
            Firebaseid = firebaseid;
            Latitude = latitude;
            Longitude = longitude;
        }

        public Coordinate(Action<object> value)
        {
            this.value = value;
        }

        public double DistanceTo(double targetLatitude, double targetLongitude)
        {
            // Haversine formula for calculating distance between two coordinates
            const double earthRadius = 6371; // Earth's radius in kilometers
            var dLat = (targetLatitude - Latitude) * (Math.PI / 180);
            var dLon = (targetLongitude - Longitude) * (Math.PI / 180);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(Latitude * (Math.PI / 180)) * Math.Cos(targetLatitude * (Math.PI / 180)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = earthRadius * c;
            return distance;
        }
    }

    public class NearestCoordinateFinder
    {
        public static async Task<Coordinate> FindNearestCoordinate(double targetLatitude, double targetLongitude, List<Coordinate> coordinates)
        {
            if (coordinates == null || coordinates.Count == 0)
            {
                throw new ArgumentException("Coordinates list cannot be null or empty.");
            }

            Coordinate nearestCoordinate = null;
            double minDistance = double.MaxValue;

            foreach (var coordinate in coordinates)
            {
                var distance = coordinate.DistanceTo(targetLatitude, targetLongitude);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestCoordinate = coordinate;
                }
            }

            return nearestCoordinate;
        }
    }

}