using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cluster;

namespace Tests
{
  [TestClass]
  public class EventCollectionTest : Test
  {
    #region Properties


    private EventCollection collection;

    #endregion


    #region Initalisation

    /// <summary>
    /// Initalises a new Coordinate Collection object, within known values
    /// </summary>
    [TestInitialize]
    public void Initalise()
    {
      // Initialise an EventCollection
      collection = new EventCollection();

      // Add four Events in the shape of a square
      Drop drop1 = new Drop();
      drop1.Coordinate = new Coordinate(10, 10, 0);
      collection.Add(drop1);

      Drop drop2 = new Drop();
      drop2.Coordinate = new Coordinate(10, 20, 0);
      collection.Add(drop2);

      Fail fail1 = new Fail();
      fail1.Coordinate = new Coordinate(20, 10, 0);
      collection.Add(fail1);

      Fail fail2 = new Fail();
      fail2.Coordinate = new Coordinate(20, 20, 0);
      collection.Add(fail2);
    }

    #endregion

    #region Public Methods


    [TestMethod]
    public void AddEventTest()
    {
      // Ensure there are four elements before starting
      Assert.AreEqual(4, collection.Count);

      // Add a new coordiante
      Drop drop = new Drop();
      drop.Coordinate = new Coordinate(0,0,0);
      collection.Add(drop);

      // Test to ensure the count has gone up.
      Assert.AreEqual(5, collection.Count);
    }

    [TestMethod]
    public void AddRangeTest()
    {
      // Ensure there are four elements before starting
      Assert.AreEqual(4, collection.Count);

      // Make a clone of the collection
      EventCollection collection2 = (EventCollection) collection.Clone();

      // Add the cloned data back into the original collection
      collection.AddRange(collection2);

      // Test to ensure the count has gone up.
      Assert.AreEqual(8, collection.Count);
    }

    [TestMethod]
    public void UpdateCentroidTest()
    {
      // Setup a new Collection
      EventCollection col = new EventCollection();

      // Check the centroid has been initalised
      Assert.AreEqual(0, col.Centroid.Latitude);
      Assert.AreEqual(0, col.Centroid.Latitude);
      Assert.AreEqual(0, col.Centroid.Altitude);

      // Add four Events in the shape of a square
      Drop drop1 = new Drop();
      drop1.Coordinate = new Coordinate(10, 10, 0);
      col.Add(drop1);

      Drop drop2 = new Drop();
      drop2.Coordinate = new Coordinate(10, 20, 0);
      col.Add(drop2);

      Fail fail1 = new Fail();
      fail1.Coordinate = new Coordinate(20, 10, 0);
      col.Add(fail1);

      Fail fail2 = new Fail();
      fail2.Coordinate = new Coordinate(20, 20, 0);
      col.Add(fail2);

      // Check the centroid has been updated
      Assert.AreEqual(15, col.Centroid.Longitude);
      Assert.AreEqual(15, col.Centroid.Latitude);
      Assert.AreEqual(0, col.Centroid.Altitude);
    }

    [TestMethod]
    public void SplitTest()
    {
      // Split the collection into two equal sized collections
      List<EventCollection> collections = collection.Split(2);

      // Ensure that two Collections have been created
      Assert.AreEqual(2, collections.Count);

      // Ensure that they both have an equal number of coordinates
      Assert.AreEqual(2, collections[0].Count);
      Assert.AreEqual(2, collections[1].Count);
    }

    [TestMethod]
    public void AppendTest()
    {
      // Make a clone of the collection
      EventCollection collection2 = (EventCollection) collection.Clone();

      // Append collection2 onto the end of collection 1
      collection.Append(collection2);

      // Enusre that that count has gone up
      Assert.AreEqual(8, collection.Count);
    }

    #endregion
  }
}