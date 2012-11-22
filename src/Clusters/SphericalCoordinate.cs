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
  /// This class represents a spherical coordinate. A spherical coordinate is a 
  /// three-dimensional space, defined by the radius, a polar angle and the 
  /// azimuth angle.
  /// 
  /// References Used:
  /// http://code.google.com/p/kmlcircle/
  /// </summary>
  public class SphericalCoordinate
  {
    #region Properties

    /// <summary>
    /// X-AXIS coordinate value in radians
    /// </summary>
    public double X { get; private set; } 

    /// <summary>
    /// Y-AXIS cartesian coordinate value in radians
    /// </summary>
    public double Y { get; private set; } 

    /// <summary>
    /// Z-AXIS cartesian coordinate value in radians
    /// </summary>
    public double Z { get; private set; }

    #endregion

    #region Contructors

    /// <summary>
    /// Primary Constructor
    /// </summary>
    /// <param name="x">x-axis cartesian coordinate value</param>
    /// <param name="y">y-axis cartesian coordinate value</param>
    /// <param name="z">z-axis cartesian coordinate value</param>
    public SphericalCoordinate(double x, double y, double z)
    {
      X = x;
      Y = y;
      Z = z;
    }

    /// <summary>
    /// Constructor that will convert a longitude and latitude values into the 
    /// required radius, polar angle and azimuth angle values.
    /// </summary>
    /// <param name="Long">The longitude value</param>
    /// <param name="Lat">The latitude value</param>
    public SphericalCoordinate(double longitude, double latitude)
    {
      double theta = longitude;
      double phi = Math.PI / 2.0 - latitude;

      X = Math.Cos(theta) * Math.Sin(phi);
      Y = Math.Sin(theta) * Math.Sin(phi);
      Z = Math.Cos(phi);
    }
  
    #endregion

    #region Public Methods

    /// <summary>
    /// This method will convert the spherical x, y, z coordinate back to a 
    /// longitude and latitude coordinate object.
    /// </summary>
    /// <returns>A coordinate object</returns>
    public Coordinate ToCoordinate()
    {
      // Longitude value
      double longitude = (X == 0) ? Math.PI / 2 : Math.Atan(Y / X);

      // Calculate the colatitude angle (complementary angle of the latitude)
      double colatitude = Math.Acos(Z);
      double latitude = Math.PI / 2 - colatitude;

      // Select the correct branch of arctan
      if(X == 0)
      {
        longitude = (Y <= 0) ? longitude - Math.PI : longitude + Math.PI;
      }

      // Convert the radian values back into degrees
      latitude *= 180 / Math.PI;
      longitude *= 180 / Math.PI;

      return new Coordinate(longitude, latitude);
    }

    #endregion
  }
}
