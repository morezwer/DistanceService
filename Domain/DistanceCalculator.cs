namespace DistanceService.Domain;

public static class DistanceCalculator
{
    private const double EarthRadiusMiles = 3958.8;

    public static double CalculateMiles(double lat1, double lon1, double lat2, double lon2)
    {
        double ToRadians(double degrees) => Math.PI * degrees / 180.0;

        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);

        var a = Math.Pow(Math.Sin(dLat / 2.0), 2.0) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Pow(Math.Sin(dLon / 2.0), 2.0);
        var c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

        return EarthRadiusMiles * c;
    }
}
