using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cluster;

namespace Tests
{
  [TestClass]
  public class DBSCANTest : Test
  {

    [TestMethod]
    public void AnalyseTest()
    {
      // Setup a new Event Collection
      EventCollection collection = new EventCollection();

      // Add 4 Clusters
      collection.AddRange(GenerateCluster(51.0, 51.1, -21.0, -21.1, 10));
      collection.AddRange(GenerateCluster(41.0, 41.1, -11.0, -11.1, 10));
      collection.AddRange(GenerateCluster(31.0, 31.1, -1.0, -1.1, 10));
      collection.AddRange(GenerateCluster(21.0, 21.1, 1.0, 1.1, 10));

      // Cluster the data
      DBSCAN scan = new DBSCAN(collection, 5);
      scan.Analyse();

      // Ensure that there are 4 clusters
      Assert.AreEqual(4, scan.Clusters.Count);

      // Esnure that there are 10 objects within each cluster
      Assert.AreEqual(10, scan.Clusters[0].Count);
      Assert.AreEqual(10, scan.Clusters[1].Count);
      Assert.AreEqual(10, scan.Clusters[2].Count);
      Assert.AreEqual(10, scan.Clusters[3].Count);
    }


    #region Private Methods

    private EventCollection GenerateCluster(double latlower, double latupper, double longlower, double longupper, int size)
    {
      // Initalise a new EventCollection
      EventCollection collection = new EventCollection();
      // Initalise a new Random number generator
      Random random = new Random();
      // Create size number of Coordinates
      for (int i = 0; i < size; i++)
      {
        // Generate a Longitude
        double longitude = random.NextDouble() * (longupper - longlower) + longlower;
        // Generate a Latitude
        double latitude = random.NextDouble() * (latupper - latlower) + latlower;
        // Setup a Test Event - it doesn't matter if its a drop, failure or success
        Drop evt = new Drop();
        evt.Coordinate = new Coordinate(longitude, latitude);
        // Add to the collection
        collection.Add(evt);
      }
      // return the collection of events
      return collection;
    }

    #endregion



  }
}
