using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cluster;
using KML;

namespace Tests
{
  [TestClass]
  public class KMLWriterTest : Test
  {
    #region Properties

    /// <summary>
    /// List of EventCollection objects to represent each cluster
    /// </summary>
    private List<EventCollection> Clusters;

    /// <summary>
    /// An EventCollection of all events that are deemed to be noise
    /// </summary>
    private EventCollection Noise;

    #endregion

    #region Initalisation

    /// <summary>
    /// This method will import a sample KML file, and then cluster the imported
    /// data. The clustered events and the noise data can be found within the 
    /// Clusters and Noise member variables.
    /// </summary>
    [TestInitialize]
    public void Initalise()
    {
      // Read in a Sample KML File
      KMLReader reader = new KMLReader(ROOT_DIR + "data\\L_wk32_drops.kml");
      // Obtain all the events
      EventCollection collection = reader.GetCallLogs();
      // Cluster the Events
      DBSCAN scan = new DBSCAN(collection);
      scan.Analyse();
      // Get a copy of the Clustered Events
      Clusters = scan.Clusters;
      // Get a copy of the Noise Events
      Noise = scan.Noise;
      // Make a new TEMP dir
      Directory.CreateDirectory(OUTPUT_DIR);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// This method will test to ensure that a KML output file can be correctly 
    /// generated based upon a given list of clustered events.
    /// </summary>
    [TestMethod]
    public void TestGenerateKMLNoNoise()
    {
      // Generate the output file path
      String file = OUTPUT_DIR + "kml_no_noise.kml";
      // Write the Clusters to disk
      KMLWriter.GenerateKML(Clusters, file);
      // Get the output file as a String array
      String[] kml = ReadFile(file);
      // Ensure that there is an XML heading tag in the output file
      Assert.AreEqual(kml[0], "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
      // Ensure that there is a KML namespace present.
      Assert.AreEqual(kml[1], "<kml xmlns=\"http://www.opengis.net/kml/2.2\">");
      // Ensure that there are Folders
      Assert.AreEqual(kml[305], "<Folder>");
      // Ensure that a folder has a name
      Assert.AreEqual(kml[306], "<name>Cluster 0</name>");
      // Ensure that there is a placemark
      Assert.AreEqual(kml[308], "<Placemark>");
      // Ensure that the above placemark is closed
      Assert.AreEqual(kml[315], "</Placemark>");
      // Ensure that the above folder is closed
      Assert.AreEqual(kml[760], "</Folder>");
    }

    /// <summary>
    /// This method will test to ensure that a KML output file can be correctly 
    /// generated based upon a given list of clustered events, and a given 
    /// list of noise events.
    /// </summary>
    [TestMethod]
    public void TestGenerateKMLNoise()
    {
      // Generate the output file path
      String file = OUTPUT_DIR + "kml_noise.kml";
      // Write the Clusters to disk
      KMLWriter.GenerateKML(Clusters, Noise, file);
      // Get the output file as a String array
      String[] kml = ReadFile(file);
      // Ensure that there is an XML heading tag in the output file
      Assert.AreEqual(kml[0], "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
      // Ensure that there is a KML namespace present.
      Assert.AreEqual(kml[1], "<kml xmlns=\"http://www.opengis.net/kml/2.2\">");
      // Ensure that there are Folders
      Assert.AreEqual(kml[305], "<Folder>");
      // Ensure that a folder has a name
      Assert.AreEqual(kml[306], "<name>Cluster 0</name>");
      // Ensure that there is a placemark
      Assert.AreEqual(kml[308], "<Placemark>");
      // Ensure that the above placemark is closed
      Assert.AreEqual(kml[315], "</Placemark>");
      // Ensure that the above folder is closed
      Assert.AreEqual(kml[760], "</Folder>");
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method will read a given file into a String array. All lines will 
    /// have any leading or trailing whitespace removed.
    /// </summary>
    /// <param name="file">The file to be read</param>
    /// <returns>A String array representing the file</returns>
    private String[] ReadFile(String file)
    {
      // Get an array of all the lines in the file
      String[] lines = File.ReadAllLines(file);
      // Time each line within the lines array
      for (int i = 0; i < lines.Length; i++)
      {
        lines[i] = lines[i].Trim();
      }
      // Return the lines list as an array
      return lines;
    }

    #endregion
  }
}
