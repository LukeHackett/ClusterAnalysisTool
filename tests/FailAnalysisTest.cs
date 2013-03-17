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
  public class FailAnalysisTest : Test
  {
    #region Properties

    /// <summary>
    /// The list of Clustered objects
    /// </summary>
    private EventCollection Cluster;

    /// <summary>
    /// Fail Analysis object to perform all the analysis
    /// </summary>
    private FailAnalysis Analysis;

    #endregion

    #region Initialise

    /// <summary>
    /// Initalises a the Clustered events and analysis object.
    /// </summary>
    [TestInitialize]
    public void Initialise()
    {
      // Create a new KML Reader
      KMLReader reader = new KMLReader(ROOT_DIR + "data\\L_wk32_drops.kml");

      // Read in all call logs
      EventCollection collection = reader.GetCallLogs();

      // Cluster the call logs
      DBSCAN scan = new DBSCAN(collection);
      scan.Analyse();

      // Initialise the cluster
      Cluster = new EventCollection(scan.Clusters.OrderBy(col => col.Count).Last());

      // Initialise the Cluster analysis
      Analysis = new FailAnalysis(Cluster);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// This tests to ensure that the Analysis object is correctly setup. The 
    /// Analysis object will filter out all events that are not of type Fail.
    /// </summary>
    [TestMethod]
    public void TestAnalysisLength()
    {
      // Expected number of Fail events in the cluster
      int expected = Cluster.GetEvents("Fail").Count();

      // Actual number of Fail events within the cluster
      int actual = Analysis.GetTotalFailCount();

      // Test to ensure the object has removed all non-Fail events
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// This method will test to ensure that the correct number of Fails are 
    /// being returned based upon grouping by the Start RAT.
    /// </summary>
    [TestMethod]
    public void GroupByStartRatTest()
    {
      // Obtain the results
      AnalysisResults results = Analysis.GroupByStartRat();

      // Loop over each result
      foreach(KeyValuePair<String, EventCollection> pair in results)
      {
        // Obtain the Start Mix Band
        String StartRat = pair.Key;

        // Obtain the expected count
        int expected = Cluster.GetEvents("Fail")
                              .Where(evt => evt.StartRat == StartRat)
                              .Count();

        // Obtain the actual count
        int actual = pair.Value.Count;

        // Ensure the expected results are the same as the actual results
        Assert.AreEqual(expected, actual);
      }
    }

    /// <summary>
    /// This method will test to ensure that the correct number of Fails are 
    /// being returned based upon grouping by the Start and End RAT.
    /// </summary>
    [TestMethod]
    public void GroupByStartEndRatTest()
    {
      // Obtain the results
      AnalysisResults results = Analysis.GroupByStartEndRat();

      // Loop over each result
      foreach (KeyValuePair<String, EventCollection> pair in results)
      {
        // Obtain the Start and End RAT
        String[] keys = Analysis.SplitKey(pair.Key);
        String StartRat = keys[0];
        String EndRat = keys[1];

        // Obtain the expected count
        int expected = Cluster.GetEvents("Fail")
                              .Where(evt => evt.StartRat == StartRat)
                              .Where(evt => evt.EndRat == EndRat)
                              .Count();

        // Obtain the actual count
        int actual = pair.Value.Count;

        // Ensure the expected results are the same as the actual results
        Assert.AreEqual(expected, actual);
      }
    }

    /// <summary>
    /// This method will test to ensure that the correct number of Fails are 
    /// being returned based upon grouping by the Start Mix band.
    /// </summary>
    [TestMethod]
    public void GroupByStartMixBandTest()
    {
      // Obtain the results
      AnalysisResults results = Analysis.GroupByStartMixBand();

      // Loop over each result
      foreach (KeyValuePair<String, EventCollection> pair in results)
      {
        // Obtain the Start Mix Band
        String StartMixBand = pair.Key;

        // Obtain the expected count
        int expected = Cluster.GetEvents("Fail")
                              .Where(evt => evt.StartMixBand == StartMixBand)
                              .Count();

        // Obtain the actual count
        int actual = pair.Value.Count;

        // Ensure the expected results are the same as the actual results
        Assert.AreEqual(expected, actual);
      }
    }

    /// <summary>
    /// This method will test to ensure that the correct number of Fails are 
    /// being returned based upon grouping by the Start and End Mix Band.
    /// </summary>
    [TestMethod]
    public void GroupByStartEndMixBandTest()
    {
      // Obtain the results
      AnalysisResults results = Analysis.GroupByStartEndMixBand();

      // Loop over each result
      foreach (KeyValuePair<String, EventCollection> pair in results)
      {
        // Obtain the Start and End Mix Band
        String[] keys = Analysis.SplitKey(pair.Key);
        String StartMixBand = keys[0];
        String EndMixBand = keys[1];

        // Obtain the expected count
        int expected = Cluster.GetEvents("Fail")
                              .Where(evt => evt.StartMixBand == StartMixBand)
                              .Where(evt => evt.EndMixBand == EndMixBand)
                              .Count();

        // Obtain the actual count
        int actual = pair.Value.Count;

        // Ensure the expected results are the same as the actual results
        Assert.AreEqual(expected, actual);
      }
    }

    /// <summary>
    /// This method will test to ensure that the correct number of Fails are 
    /// being returned based upon grouping by the Start RRC State.
    /// </summary>
    [TestMethod]
    public void GroupByStartRRCStateTest()
    {
      // Obtain the results
      AnalysisResults results = Analysis.GroupByStartRRCState();

      // Loop over each result
      foreach (KeyValuePair<String, EventCollection> pair in results)
      {
        // Obtain the Start RAT
        String StartRRCState = pair.Key;

        // Obtain the expected count
        int expected = Cluster.GetEvents("Fail")
                              .Where(evt => evt.StartRRCState == StartRRCState)
                              .Count();

        // Obtain the actual count
        int actual = pair.Value.Count;

        // Ensure the expected results are the same as the actual results
        Assert.AreEqual(expected, actual);
      }
    }

    #endregion
  }
}
