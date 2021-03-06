﻿/// Copyright (c) 2012, Research In Motion Limited.
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
  /// This class represents a Coordinate. 
  /// Each coordinate can be identified by it's Longitude and Latitude value, 
  /// which are set when the coordinate object is instantiated.
  /// </summary>
  public class Coordinate
  {
    #region Properties

    /// <summary>
    /// Latitude Value (North-South Position).
    /// </summary>
    public double Latitude { get; set; }
    
    /// <summary>
    /// Longitude Value (East-West Position).
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// Elevation value (distance from sea level)
    /// </summary>
    public double Altitude { get; set; }

    /// <summary>
    /// The cluster that this event belongs to.
    /// </summary>
    public int ClusterId { get; set; }

    /// <summary>
    /// Whether or not this event is classed as noise.
    /// </summary>
    public bool Noise { get; set; }

    /// <summary>
    /// Whether or not this event has been classified (or visited).
    /// </summary>
    public bool Classified { get; set; }

    #endregion
    
    #region Constructors
    
    /// <summary>
    /// Primary Constructor.
    /// </summary>
    /// <param name="Long">The Longitude value of the coordinate.</param>
    /// <param name="Lat">The Latitude value of the coordinate.</param>
    public Coordinate(double longitude, double latitude)
    { 
      Longitude = longitude;
      Latitude = latitude;
      Altitude = 0;
    }

    /// <summary>
    /// Alternative Constructor
    /// </summary>
    /// <param name="Long">The Longitude value of the coordinate.</param>
    /// <param name="Lat">The Latitude value of the coordinate.</param>
    /// <param name="altitude">The altitude value of the coordinate.</param>
    public Coordinate(double longitude, double latitude, double altitude)
    {
      Latitude = latitude;
      Longitude = longitude;
      Altitude = altitude;
    }
    
    #endregion
    
    #region Public Methods
    
    /// <summary>
    /// This method returns the Longitude value as a radian value.
    /// </summary>
    /// <returns>longitude as a radian value.</returns>
    public double LongitudeAsRadians()
    {
      return DegreesToRadians(Longitude);
    }
    
    /// <summary>
    /// This method returns the Latitude value as a radian value.
    /// </summary>
    /// <returns>latitude as a radian value.</returns>
    public double LatitudeAsRadians()
    {
      return DegreesToRadians(Latitude);
    }    
    
    /// <summary>
    /// String representation of the Latitude and the Longitude of this cluster.
    /// </summary>
    /// <returns>a formatted string.</returns>
    public override String ToString()
    {
      return String.Format("{0},{1},{2}", Longitude, Latitude, Altitude);
    }

    /// <summary>
    /// Spherical Coordinate based upon the Latitude and Longitude of this 
    /// coordinate.
    /// </summary>
    /// <returns>a spherical coordinate</returns>
    public SphericalCoordinate ToSphericalCoordinate()
    {
      return new SphericalCoordinate(LongitudeAsRadians(), LatitudeAsRadians());
    }

    /// <summary>
    /// This method will compare the Cluster ID, Latitude and Longitude values 
    /// of the passed coordinate object to the values held within this coordinate 
    /// object.
    /// </summary>
    /// <param name="obj">The coordinate object to compare.</param>
    /// <returns>whether or not the object is equal to this.</returns>
    public override bool Equals(object obj)
    {
      if (obj is Coordinate)
      {
        // Cast the object to a coordinate object
        Coordinate coordinate = (Coordinate) obj;
        // Deep Comparison
        return Latitude == coordinate.Latitude && 
                Longitude == coordinate.Longitude;
      }
      return false;
    }
    
    /// <summary>
    /// This method will return whether or not this coordinate is between the 
    /// lower and upper coordinates.
    /// </summary>
    /// <param name="c1">Lower coordinate.</param>
    /// <param name="c2">Upper coordinate.</param>
    /// <returns>Whether or not this coordinate is in between the parameters.</returns>
    public bool IsBetween(Coordinate c1, Coordinate c2)
    {
      return (Latitude >= c1.Latitude && Latitude <= c2.Latitude) &&
                (Longitude >= c1.Longitude && Longitude <= c2.Longitude);
    }

    /// <summary>
    /// This method returns the current object's hash code.
    /// </summary>
    /// <returns>integer hash code of this object.</returns>
    public override int GetHashCode()
    {
      return ( (int)Latitude >> 32 ^ (int)Longitude >> 32 );
    }

    #endregion        

    #region Public Static Methods

    /// <summary>
    /// This method will convert degrees into radians.
    /// </summary>
    /// <param name="degrees">The value to be converted</param>
    /// <returns>The radians value</returns>
    public static double DegreesToRadians(double degrees)
    {
      return (Math.PI / 180) * degrees;
    }

    /// <summary>
    /// This method will clear the values that are associated with the clustering 
    /// methods.
    /// </summary>
    public void ClearClusterData()
    {
      ClusterId = -1;
      Noise = false;
      Classified = false;
    }

    #endregion
  }
}
