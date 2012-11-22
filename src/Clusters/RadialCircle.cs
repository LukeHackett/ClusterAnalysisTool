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
  /// This class provides methods to allow a complete list of coordinates to be 
  /// generated in order to create a complete radius.
  /// 
  /// References Used:
  /// http://code.google.com/p/kmlcircle/
  /// http://blog.modp.com/2007/09/rotating-point-around-vector.html
  /// </summary>
  public class RadialCircle
  {
    #region Properties

    /// <summary>
    /// The radius of the circle in meters
    /// </summary>
    public double Radius { get; private set; }

    /// <summary>
    /// The centroid of the radius
    /// </summary>
    public Centroid Centre { get; private set; }

    /// <summary>
    /// The Spherical centroid of the radius
    /// </summary>
    public SphericalCoordinate Spherical { get; private set; }

    #endregion

    #region Constructor

    /// <summary>
    /// Primary Constructor
    /// </summary>
    /// <param name="centre">the centroid of the radius</param>
    /// <param name="radius">the radius of the circle in meters</param>
    public RadialCircle(Centroid centre, double radius)
    {
      Centre = centre;
      Radius = radius;
      Spherical = centre.ToSphericalCoordinate();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// This method will create a circle around the centre point founf within 
    /// this instantiated object. The method will make use of Earth's Radius at 
    /// the centre of the circle, as this is a good enough approximation for 
    /// small circle.
    /// </summary>
    /// <param name="n">The number of points to be generated</param>
    /// <param name="offset">the offset of radius value</param>
    /// <returns>A list of coordiantes</returns>
    public List<Coordinate> CreateRadialCircle(int n = 360, double offset = 0)
    {
      // Mean Radius of Earth in meters
      double offsetRadians = offset * (Math.PI / 180);

      // Get the longitude and latitudes as radians for further calculations
      double longitude = Centre.LongitudeAsRadians();
      double latitude = Centre.LatitudeAsRadians();

      // compute longitude degrees (in radians) at the latitude
      double r = (Radius / (Distance.EarthRadius(Centre.Latitude) * Math.Cos(latitude)));

      // Create a new centre Spherical point
      SphericalCoordinate centre = new SphericalCoordinate(longitude, latitude);

      // Create one outer point to use for rotating N times
      SphericalCoordinate point = new SphericalCoordinate(longitude + r, latitude);

      // Create a new coordinates list for all the radial points
      List<Coordinate> coordinates = new List<Coordinate>();

      // Rotate around the centre n number of times
      for (int i = 0; i < n; i++)
      {
        // Generate new Coordinate
        double phi = offsetRadians + (2.0 * Math.PI / n) * i;
        Coordinate c = rotatePoint(point, phi);
        coordinates.Add(c);
      }
      // To create a complete circle
      coordinates.Add(coordinates[0]);

      return coordinates;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method will rotate a given point around the given centre centroid 
    /// within this object by phi radians.
    /// </summary>
    /// <param name="point">The point to be rotated</param>
    /// <param name="phi">The number of radians to move the point by</param>
    /// <returns>A coordinate</returns>
    private Coordinate rotatePoint(SphericalCoordinate point, double phi)
    {
      // Map values to new variables for reasons of maintaining sanity
      double u = Spherical.X;
      double v = Spherical.Y;
      double w = Spherical.Z;
      double x = point.X;
      double y = point.Y;
      double z = point.Z;

      double a = u * x + v * y + w * z;
      double d = Math.Cos(phi);
      double e = Math.Sin(phi);

      // Create a new Spherical Coordinate
      double newX = (a * u + (x - a * u) * d + (v * z - w * y) * e);
      double newY = (a * v + (y - a * v) * d + (w * x - u * z) * e);
      double newZ = (a * w + (z - a * w) * d + (u * y - v * x) * e);

      return new SphericalCoordinate(newX, newY, newZ).ToCoordinate();
    }

    #endregion
  }
}
