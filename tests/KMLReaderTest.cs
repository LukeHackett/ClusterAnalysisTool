using KML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cluster;


namespace Tests
{
  /// <summary>
  /// This test class will test the KMLReader class found within the KML 
  /// library.
  /// </summary>
  [TestClass]
  public class KMLReaderTest : Test
  {
    #region Public Methods

    /// <summary>
    /// This method will test to ensure that the L_wk32_drops.kml file found 
    /// within the data directory can be correctly parsed into an in-memory 
    /// EventCollection object.
    /// </summary>
    [TestMethod]
    public void ReadKMLFile_L32()
    {
      // Create the path of the KML file to import
      String file = ROOT_DIR + "data\\L_wk32_drops.kml";
      // Obtain the collection of events from the KML file
      EventCollection collection = ReadKMLFile(file);
      // Ensure there are 25 elements in the collection
      Assert.AreEqual(25, collection.Count);
    }

    /// <summary>
    /// This method will test to ensure that the L_wk33_drops.kml file found 
    /// within the data directory can be correctly parsed into an in-memory 
    /// EventCollection object.
    /// </summary>
    [TestMethod]
    public void ReadKMLFile_L33()
    {
      // Create the path of the KML file to import
      String file = ROOT_DIR + "data\\L_wk33_drops.kml";
      // Obtain the collection of events from the KML file
      EventCollection collection = ReadKMLFile(file);
      // Ensure there are 25 elements in the collection
      Assert.AreEqual(36, collection.Count);
    }

    /// <summary>
    /// This method will test to ensure that the L_wk34_drops.kml file found 
    /// within the data directory can be correctly parsed into an in-memory 
    /// EventCollection object.
    /// </summary>
    [TestMethod]
    public void ReadKMLFile_L34()
    {
      // Create the path of the KML file to import
      String file = ROOT_DIR + "data\\L_wk34_drops.kml";
      // Obtain the collection of events from the KML file
      EventCollection collection = ReadKMLFile(file);
      // Ensure there are 25 elements in the collection
      Assert.AreEqual(43, collection.Count);
    }

    /// <summary>
    /// This method will test to ensure that the L_wk35_drops.kml file found 
    /// within the data directory can be correctly parsed into an in-memory 
    /// EventCollection object.
    /// </summary>
    [TestMethod]
    public void ReadKMLFile_L35()
    {
      // Create the path of the KML file to import
      String file = ROOT_DIR + "data\\L_wk35_drops.kml";
      // Obtain the collection of events from the KML file
      EventCollection collection = ReadKMLFile(file);
      // Ensure there are 25 elements in the collection
      Assert.AreEqual(42, collection.Count);
    }

    /// <summary>
    /// This method will test to ensure that the T_wk32_drops.kml file found 
    /// within the data directory can be correctly parsed into an in-memory 
    /// EventCollection object.
    /// </summary>
    [TestMethod]
    public void ReadKMLFile_T32()
    {
      // Create the path of the KML file to import
      String file = ROOT_DIR + "data\\T_wk32_drops.kml";
      // Obtain the collection of events from the KML file
      EventCollection collection = ReadKMLFile(file);
      // Ensure there are 25 elements in the collection
      Assert.AreEqual(11, collection.Count);
    }

    /// <summary>
    /// This method will test to ensure that the T_wk33_drops.kml file found 
    /// within the data directory can be correctly parsed into an in-memory 
    /// EventCollection object.
    /// </summary>
    [TestMethod]
    public void ReadKMLFile_T33()
    {
      // Create the path of the KML file to import
      String file = ROOT_DIR + "data\\T_wk33_drops.kml";
      // Obtain the collection of events from the KML file
      EventCollection collection = ReadKMLFile(file);
      // Ensure there are 25 elements in the collection
      Assert.AreEqual(14, collection.Count);
    }

    /// <summary>
    /// This method will test to ensure that the T_wk34_drops.kml file found 
    /// within the data directory can be correctly parsed into an in-memory 
    /// EventCollection object.
    /// </summary>
    [TestMethod]
    public void ReadKMLFile_T34()
    {
      // Create the path of the KML file to import
      String file = ROOT_DIR + "data\\T_wk34_drops.kml";
      // Obtain the collection of events from the KML file
      EventCollection collection = ReadKMLFile(file);
      // Ensure there are 25 elements in the collection
      Assert.AreEqual(15, collection.Count);
    }

    /// <summary>
    /// This method will test to ensure that the T_wk35_drops.kml file found 
    /// within the data directory can be correctly parsed into an in-memory 
    /// EventCollection object.
    /// </summary>
    [TestMethod]
    public void ReadKMLFile_T35()
    {
      // Create the path of the KML file to import
      String file = ROOT_DIR + "data\\T_wk35_drops.kml";
      // Obtain the collection of events from the KML file
      EventCollection collection = ReadKMLFile(file);
      // Ensure there are 25 elements in the collection
      Assert.AreEqual(7, collection.Count);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This is a helper method, to reduce repeating code throughout this Test 
    /// class. The objective of this method is to parse a given KML file, and 
    /// return an EventCollection object.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    private EventCollection ReadKMLFile(String file)
    {
      // Create a new KML reader object to read the data
      KMLReader reader = new KMLReader(file);
      // Return a collection of events from the KML file
      return reader.GetCallLogs();
    }

    #endregion
  }
}
