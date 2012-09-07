using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cluster
{
  /// <summary>
  /// This static class is partial translation from the "Latitude/longitude Spherical Geodesy 
  /// Formulae" originally developed by Chris Veness. 
  /// All the methods found within this class take coorindates as parameters to minimise the 
  /// additional mathematical work that might have been required.
  /// Source: http://www.movable-type.co.uk/scripts/latlong.html
  /// </summary>
  public static class Distance
  {
    #region Properties

    /// <summary>
    /// Radius of the Earth in KM. 
    /// </summary>
    public static int EARTH_RADIUS
    {
      get { return 6371; }
    }

    #endregion
    
    #region Public Methods

    /// <summary>
    /// This uses the 'haversine' formula to calculate the great-circle distance between two points. 
    /// This is the shortest distance over the earth’s surface – giving an 'as-the-crow-flies' 
    /// distance between the two coordinates.
    /// </summary>
    /// <param name="c1">First coordinate</param>
    /// <param name="c2">Second coordinate</param>
    /// <returns>the distance between coordinate 1 and coordinate 2</returns>
    public static double haversine(Coordinate c1, Coordinate c2)
    {
      // Calculate the differences in the Lat/Long
      double dLat = c2.LatitudeAsRadians() - c1.LatitudeAsRadians();
      double dLon = c2.LongitudeAsRadians() - c1.LongitudeAsRadians();

      // Convert the signed decimal degrees to radians
      double lat1 = c1.LatitudeAsRadians();
      double lat2 = c2.LatitudeAsRadians();

      // Calculate the square of half the chord length between the points
      double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2)
                  * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);

      // Calculate the angular distance (in radians)
      double angDistance = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

      return EARTH_RADIUS * angDistance;
    }

    /// <summary>
    /// This method uses the law of cosines to calculate the distance between the two coordinates.
    /// Results are generally accurate to within a few metres. 
    /// </summary>
    /// <param name="c1">First coordinate</param>
    /// <param name="c2">Second coordinate</param>
    /// <returns>the distance between coordinate 1 and coordinate 2</returns>
    public static double spherical(Coordinate c1, Coordinate c2)
    {
      // Get the radian values of the coordinate
      double lat1 = c1.LatitudeAsRadians();
      double lon1 = c1.LongitudeAsRadians();
      double lat2 = c2.LatitudeAsRadians();
      double lon2 = c2.LongitudeAsRadians();

      return Math.Acos(Math.Sin(lat1) * Math.Sin(lat2) + Math.Cos(lat1)
                * Math.Cos(lat2) * Math.Cos(lon2 - lon1)) * EARTH_RADIUS;
    }

    /// <summary>
    /// This method will calculate the distance between two coordinates based upon Pythagoras' 
    /// theorem, which is not as accurate as a spherical approach. This method will work well when 
    /// the distance between the two coordinates is relatively small.
    /// </summary>
    /// <param name="c1">First coordinate</param>
    /// <param name="c2">Second coordinate</param>
    /// <returns>the distance between coordinate 1 and coordinate 2</returns>
    public static double equirectangular(Coordinate c1, Coordinate c2)
    {
      // Get the radian values of the coordinate
      double lat1 = c1.LatitudeAsRadians();
      double lon1 = c1.LongitudeAsRadians();
      double lat2 = c2.LatitudeAsRadians();
      double lon2 = c2.LongitudeAsRadians();

      // Calculation     
      double x = (lon2 - lon1) * Math.Cos((lat1 + lat2) / 2);
      double y = (lat2 - lat1);

      return Math.Sqrt(x * x + y * y) * EARTH_RADIUS;
    }

    /// <summary>
    /// This method will square the distance between two coordinates.
    /// </summary>
    /// <param name="c1">First coordinate</param>
    /// <param name="c2">Second coordinate</param>
    /// <returns>the distance squared</returns>
    public static double DistanceSquared(Coordinate c1, Coordinate c2)
    {
      double diffX = c2.Latitude - c1.Latitude;
      double diffY = c2.Longitude - c1.Longitude;
      return diffX * diffX + diffY * diffY;
    }

    #endregion
  }
}