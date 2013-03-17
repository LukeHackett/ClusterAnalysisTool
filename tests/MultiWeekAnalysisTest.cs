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
  public class MultiWeekAnalysisTest : Test
  {
    #region Properties

    /// <summary>
    /// The MultiWeekAnalysis object that performs analysis upon the Events
    /// </summary>
    private MultiWeekAnalysis Analysis;

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
    private Dictionary<int, EventCollection> Events;

    #endregion

    #region Initialise

    /// <summary>
    /// Initalises a the Clustered events and analysis object.
    /// </summary>
    [TestInitialize]
    public void Initialise()
    {
      // A dictionary of week-events
      Events = new Dictionary<int, EventCollection>();

      // Read in the Week 32 logs
      KMLReader w32_reader = new KMLReader(ROOT_DIR + "data\\L_wk32_drops.kml");
      Events[32] = w32_reader.GetCallLogs();

      // Read in the Week 33 logs
      KMLReader w33_reader = new KMLReader(ROOT_DIR + "data\\L_wk33_drops.kml");
      Events[33] = w33_reader.GetCallLogs();

      // Read in the Week 34 logs
      KMLReader w34_reader = new KMLReader(ROOT_DIR + "data\\L_wk34_drops.kml");
      Events[34] = w34_reader.GetCallLogs();

      // Read in the Week 35 logs
      KMLReader w35_reader = new KMLReader(ROOT_DIR + "data\\L_wk35_drops.kml");
      Events[35] = w35_reader.GetCallLogs();

      // Initialise the Multi-Week Analysis Object
      Analysis = new MultiWeekAnalysis();
      Analysis.AddRange(Events[32]);
      Analysis.AddRange(Events[33]);
      Analysis.AddRange(Events[34]);
      Analysis.AddRange(Events[35]);

      // Cluster the data
      Analysis.Cluster(EPS, MIN_POINTS);

      // Analyse all weeks
      Analysis.AnalyseWeeks();
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
      int week = 33;

      // Obtain the Week analysis
      WeekAnalysis wk33_analysis = Analysis.AnalyseWeek(week);

      // Ensure that the requested week has been recieved
      Assert.AreEqual(week, wk33_analysis.WeekNumber);
    }

    /// <summary>
    /// This will test to ensure that a muliple weeks can be analysed correctly.
    /// </summary>
    [TestMethod]
    public void AnalyseWeeksTest()
    {
      // Ensure that there are four weeks of results
      Assert.AreEqual(4, Analysis.WeeklyAnalysis.Count);

      // Loop over each week's worth of testing
      foreach(KeyValuePair<int, WeekAnalysis> pair in Analysis.WeeklyAnalysis)
      {
        // Ensure that the same week was imported from a KML file
        Assert.IsTrue(Events.ContainsKey(pair.Key));
      }
    }

    #endregion
  }
}
