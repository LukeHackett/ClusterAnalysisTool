using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cluster;
using KML;
using Analysis;

namespace Tests
{
  [TestClass]
  public class MultiProductAnalysisTest : Test
  {
    #region Properties

    /// <summary>
    /// The MultiWeekAnalysis object that performs analysis upon the Events
    /// </summary>
    private MultiProductAnalysis Analysis;

    /// <summary>
    /// The EPS value used by the DBSCAN algorithm
    /// </summary>
    private const double EPS = 1.5;

    /// <summary>
    /// The minimum number of pointers for cluster 
    /// </summary>
    private const int MIN_POINTS = 3;

    /// <summary>
    /// A Dictionary of the Events parsed from each KML file
    /// </summary>
    private EventCollection Events;

    #endregion

    #region Initialise

    /// <summary>
    /// Initalises a the Clustered events and analysis object.
    /// </summary>
    [TestInitialize]
    public void Initialise()
    {
      // A dictionary of week-events
      Events = new EventCollection();

      // Read in the Week 32 logs
      KMLReader w32_reader = new KMLReader(ROOT_DIR + "data\\L_wk32_drops.kml");
      Events.AddRange(w32_reader.GetCallLogs());

      // Read in the Week 33 logs
      KMLReader w33_reader = new KMLReader(ROOT_DIR + "data\\L_wk33_drops.kml");
      Events.AddRange(w33_reader.GetCallLogs());

      // Read in the Week 34 logs
      KMLReader w34_reader = new KMLReader(ROOT_DIR + "data\\L_wk34_drops.kml");
      Events.AddRange(w34_reader.GetCallLogs());

      // Read in the Week 35 logs
      KMLReader w35_reader = new KMLReader(ROOT_DIR + "data\\L_wk35_drops.kml");
      Events.AddRange(w35_reader.GetCallLogs());

      // Initialise the Multi-Product Analysis Object
      Analysis = new MultiProductAnalysis();
      Analysis.AddRange(Events);

      // Cluster the data
      Analysis.Cluster(EPS, MIN_POINTS);

      // Analyse all weeks
      Analysis.AnalyseProducts();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// This will test to ensure that a valid week's analysis can be obtained 
    /// correctly from the multi-week analysis object.
    /// </summary>
    [TestMethod]
    public void AnalyseWeekTest()
    {
      // The week number that is expected
      String device = "9360";

      // Obtain the Week analysis
      ProductAnalysis wk33_analysis = Analysis.AnalyseProduct(device);

      // Ensure that the requested week has been recieved
      Assert.AreEqual(device, wk33_analysis.Device);
    }

    /// <summary>
    /// This will test to ensure that a muliple weeks can be analysed correctly.
    /// </summary>
    [TestMethod]
    public void AnalyseWeeksTest()
    {
      // Ensure that there are five product results
      Assert.AreEqual(5, Analysis.ProductsAnalysis.Count);

      // Loop over each product's testing
      foreach (KeyValuePair<String, ProductAnalysis> pair in Analysis.ProductsAnalysis)
      {
        // Count how many Devices of name device there are
        int count = Events.Where(evt => evt.Device == pair.Key).Count();

        // Ensure that there are some results
        Assert.IsTrue(count > 0);
      }
    }

    #endregion
  }
}
