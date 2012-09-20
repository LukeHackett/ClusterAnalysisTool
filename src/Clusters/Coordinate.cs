using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cluster
{ 
  /// <summary>
  /// This class represents a Coordinate. 
  /// Each coordinate can be identified by it's Longitude and Latitude value, which are set when the 
  /// coordinate object is instantiated.
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
    /// The cluster that this coordinate belongs to.
    /// </summary>
    public int ClusterId { get; set; }
    
    /// <summary>
    /// Whether or not this coordinate is classed as noise.
    /// </summary>
    public bool Noise { get; set; }
    
    /// <summary>
    /// Whether or not this coordinate has bee classified (or visited).
    /// </summary>
    public bool Classified { get; set; }
    
    /// <summary>
    /// Status of all possible calls
    /// </summary>
    public enum CallType { drop = -1, fail = 0, success = 1 };
    
    /// <summary>
    /// Status of the call
    /// </summary>
    public CallType CallStatus { get; set; }
    
    #endregion
    
    #region Constructors
    
    /// <summary>
    /// Primary Constructor.
    /// </summary>
    /// <param name="Long">The Longitude value of the coordinate.</param>
    /// <param name="Lat">The Latitude value of the coordinate.</param>
    public Coordinate(double Lat, double Long)
    { 
      Longitude = Long;
      Latitude = Lat;
      ClusterId = -1;
      Noise = false;
      Classified = false;
    }
    
    #endregion
    
    #region Public Methods
    
    /// <summary>
    /// This method returns the Longitude value as a radian value.
    /// </summary>
    /// <returns>longitude as a radian value.</returns>
    public double LongitudeAsRadians()
    {
      return Longitude * (Math.PI / 180);
    }
    
    /// <summary>
    /// This method returns the Latitude value as a radian value.
    /// </summary>
    /// <returns>latitude as a radian value.</returns>
    public double LatitudeAsRadians()
    {
      return Latitude * (Math.PI / 180);
    }    
    
    /// <summary>
    /// String representation of the Latitude and the Longitude of this cluster.
    /// </summary>
    /// <returns>a formatted string.</returns>
    public override String ToString()
    {
      String latString = Latitude.ToString("N8");
      String lonString = Longitude.ToString("N8");
      
      return String.Format("{0}, {1}", latString, lonString);
    }

    /// <summary>
    /// This method will compare the Cluster ID, Latitude and Longitude values of the passed 
    /// coordinate object to the values held within this coordinate object.
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
        return ClusterId == coordinate.ClusterId &&
                 Latitude == coordinate.Latitude &&
                   Longitude == coordinate.Longitude;
      }
      return false;
    }
    
    /// <summary>
    /// This method will return whether or not this coordinate is between the lower and upper 
    /// coordinates.
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
  }
}
