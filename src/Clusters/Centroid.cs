using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cluster
{
  public class Centroid : Coordinate
  {
    #region Properties

    /// <summary>
    /// The total radial covering distance 
    /// </summary>
    public double Radius { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Primary Constructor.
    /// </summary>
    /// <param name="Long">The Longitude value of the coordinate.</param>
    /// <param name="Lat">The Latitude value of the coordinate.</param>
    public Centroid(double longitude, double latitude)
      : base(longitude, latitude)
    {

    }

    #endregion
  }
}
