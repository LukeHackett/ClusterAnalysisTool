using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cluster;

namespace Tests
{
  [TestClass]
  public class SphericalCoordinateTest : Test
  {
    #region Properties

    /// <summary>
    /// The Longitude value of the Centroid object on test.
    /// </summary>
    private const double LONGITUDE = -0.2341408;

    /// <summary>
    /// The Latitude value of the Centroid object on test.
    /// </summary>
    private const double LATITUDE = 51.3653237;

    /// <summary>
    /// The altitude value of the Centroid object on test.
    /// </summary>
    private const double ALTITUDE = 0;

    /// <summary>
    /// The Latitude value of the Centroid object on test.
    /// </summary>
    private const double X = 0.44135693592891861;

    /// <summary>
    /// The altitude value of the Centroid object on test.
    /// </summary>
    private const double Y = -0.10527044279755021;

    /// <summary>
    /// The altitude value of the Centroid object on test.
    /// </summary>
    private const double Z = 0.891135337073244;

    /// <summary>
    /// 
    /// </summary>
    private SphericalCoordinate coordinate;

    #endregion

    #region Initalisation

    /// <summary>
    /// Initalises a new Coordinate object with known values.
    /// </summary>
    [TestInitialize]
    public void Initalise()
    {
      coordinate = new SphericalCoordinate(LONGITUDE, LATITUDE);
    }

    #endregion

    #region Public Methods

    [TestMethod]
    public void CoordinateConversionTest()
    {
      // Ensure that the conversion from Long/Lat has been performed successfully.
      Assert.AreEqual(X, coordinate.X);
      Assert.AreEqual(Y, coordinate.Y);
      Assert.AreEqual(Z, coordinate.Z);
    }

    #endregion
  }
}
