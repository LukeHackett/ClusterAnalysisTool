/// Copyright (c) 2012, Research In Motion Limited.
/// All rights reserved.
/// 
/// Redistribution and use in source and binary forms, with or without 
/// modification, are permitted provided that the following conditions are met:
/// 
///  [*] Redistributions of source code must retain the above copyright notice, 
///      this list of conditions and the following disclaimer.
///  [*] Redistributions in binary form must reproduce the above copyright 
///      notice, this list of conditions and the following disclaimer in the 
///      documentation and/or other materials provided with the distribution.
///  [*] Neither the name of the Research In Motion Limited nor the names of its 
///      contributors may be used to endorse or promote products derived from 
///      this software without specific prior written permission.
/// 
/// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
/// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
/// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
/// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE 
/// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
/// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
/// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
/// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
/// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
/// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
/// POSSIBILITY OF SUCH DAMAGE.

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
  /// 
  /// References used: 
  /// http://www.movable-type.co.uk/scripts/latlong.html
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