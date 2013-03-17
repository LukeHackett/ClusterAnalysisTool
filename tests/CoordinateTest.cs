using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cluster;

namespace Tests
{
  [TestClass]
  public class CoordinateTest : Test
  {
    #region Properties

    /// <summary>
    /// The Longitude value of the Coordinate object on test.
    /// </summary>
    private const double LONGITUDE = -0.2341408;

    /// <summary>
    /// The Latitude value of the Coordinate object on test.
    /// </summary>
    private const double LATITUDE = 51.3653237;

    /// <summary>
    /// The altitude value of the Coordinate object on test.
    /// </summary>
    private const double ALTITUDE = 0;

    /// <summary>
    /// The Coordinate object on test
    /// </summary>
    private Coordinate coordinate;

    #endregion

    #region Initalisation

    /// <summary>
    /// Initalises a new Coordinate object with known values.
    /// </summary>
    [TestInitialize]
    public void Initalise()
    {
      coordinate = new Coordinate(LONGITUDE, LATITUDE, ALTITUDE);
    }

    #endregion

    #region Public Methods

    [TestMethod]
    public void TestCoordinateInstantiation()
    {
      // Ensure the input Longitude value is correctly stored
      Assert.AreEqual(LONGITUDE, coordinate.Longitude);
      // Ensure the input Latitude value is correctly stored
      Assert.AreEqual(LATITUDE, coordinate.Latitude);
      // Ensure the input Altitude value is correctly stored
      Assert.AreEqual(ALTITUDE, coordinate.Altitude);
    }


    [TestMethod]
    public void TestLongitudeAsRadians()
    {
      // Expected value to be returned from the calculation
      double expected = (Math.PI / 180) * LONGITUDE;
      // Obtain the radians value
      double radians = coordinate.LongitudeAsRadians();
      //Ensure the calculation returns the correct value
      Assert.AreEqual(expected, radians);
    }


    [TestMethod]
    public void TestLatitudeAsRadians()
    {
      // Expected value to be returned from the calculation
      double expected = (Math.PI / 180) * LATITUDE;
      // Obtain the radians value
      double radians = coordinate.LatitudeAsRadians();
      //Ensure the calculation returns the correct value
      Assert.AreEqual(expected, radians);
    }


    [TestMethod]
    public void TestToString()
    {
      // Expected string representation
      String expected = LONGITUDE + "," + LATITUDE + "," + ALTITUDE;
      // Obtain the string representation
      String result = coordinate.ToString();
      // Ensure the strings are the same
      Assert.AreEqual(expected, result);
    }


    [TestMethod]
    public void TestToSphericalCoordinate()
    {
      // Obtain the Longitude and Latitude as Radians
      double lon = coordinate.LongitudeAsRadians();
      double lat = coordinate.LatitudeAsRadians();
      // Create an expected SphericalCoordinate object
      SphericalCoordinate expected = new SphericalCoordinate(lon, lat);
      // Obtain the actual SphericalCoordinate
      SphericalCoordinate result = coordinate.ToSphericalCoordinate();
      // Ensure the X values are the same
      Assert.AreEqual(expected.X, result.X);
      // Ensure the Y values are the same
      Assert.AreEqual(expected.Y, result.Y);
      // Ensure the Z values are the same
      Assert.AreEqual(expected.Z, result.Z);
    }

    #endregion
  }
}
