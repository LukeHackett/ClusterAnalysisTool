using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KML;
using Cluster;
using Analysis;

namespace Tests
{
  [TestClass]
  public class WeekAnalysisTest : Test
  {
    #region Properties

    /// <summary>
    /// The list of Clustered objects
    /// </summary>
    private List<EventCollection> Clusters;

    /// <summary>
    /// Week Analysis object to perform all the analysis
    /// </summary>
    private WeekAnalysis Analysis;

    /// <summary>
    /// Week Number associated with the data  (can be set to anything)
    /// </summary>
    private const int WEEK = 32;

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
      
      // Initialise the clusters
      Clusters = scan.Clusters;

      // Initialise the analysis
      Analysis = new WeekAnalysis(Clusters, WEEK);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// This method will test to ensure that the correct number of Drops are 
    /// being returned. This method will test for drop figures across all 
    /// clusters and intra-cluster drop figures.
    /// </summary>
    [TestMethod]
    public void GetDropFiguresTest() 
    {
      // Obtain the list of results
      Dictionary<int, EventCollection> results = Analysis.GetDropFigures();

      // Check to see if the Analysis numbers == Cluster Numbers
      Assert.AreEqual(Clusters.Count, results.Count);

      // Check to ensure that each Cluster has the correct number of drops
      for (int i = 0; i < results.Count; i++)
      {
        // Expected drop value
        int expected = Clusters[i].GetEvents("Drop").Count();
        
        // Actual drop value
        int actual = results[i+1].Count;

        // Ensure that the two values are equal
        Assert.AreEqual(expected, actual);
      }
    }

    /// <summary>
    /// This method will test to ensure that the correct number of Drops are 
    /// being returned based upon grouping by the Start RAT. This method will 
    /// test for drop figures across all clusters and intra-cluster drop 
    /// figures.
    /// </summary>
    [TestMethod]
    public void GetDropStartRatFiguresTest()
    {
      // Obtain the list of results
      Dictionary<int, AnalysisResults> results = Analysis.GetDropStartRatFigures();

      // Check to see if the Analysis numbers == Cluster Numbers
      Assert.AreEqual(Clusters.Count, results.Count);

      // Loop over each cluster
      foreach (KeyValuePair<int, AnalysisResults> pair in results)
      {
        // Cluster Number (0-Indexed Array)
        int cluster = pair.Key - 1;

        // Loop over each Intra-cluster analysis object
        foreach (KeyValuePair<String, EventCollection> analysis in pair.Value)
        {
          // Obtain the Start Rat Key
          String StartRat = analysis.Key;

          // Expected number of Drops (filtered upon Start Rat)
          int expected = Clusters[cluster].GetEvents("Drop")
                                          .Where(evt => evt.StartRat == StartRat)
                                          .Count();

          // Actual drop value
          int actual = pair.Value[StartRat].Count;

          // Ensure that the two values are equal
          Assert.AreEqual(expected, actual);
        }
      }
    }

    /// <summary>
    /// This method will test to ensure that the correct number of Drops are 
    /// being returned based upon grouping by the Start and End Rat. This 
    /// method will test for drop figures across all clusters and intra-cluster
    /// drop figures.
    /// </summary>
    [TestMethod]
    public void GetDropStartEndRatFiguresTest()
    {
      // Obtain the list of results
      Dictionary<int, AnalysisResults> results = Analysis.GetDropStartEndRatFigures();

      // Check to see if the Analysis numbers == Cluster Numbers
      Assert.AreEqual(Clusters.Count, results.Count);

      // Loop over each cluster
      foreach (KeyValuePair<int, AnalysisResults> pair in results)
      {
        // Cluster Number (0-Indexed Array)
        int cluster = pair.Key - 1;

        // Loop over each Intra-cluster analysis object
        foreach (KeyValuePair<String, EventCollection> analysis in pair.Value)
        {
          // Obtain the Start Rat and End Rat Keys
          String[] keys = analysis.Key.Split(new String[] { "=>" }, StringSplitOptions.None);
          String StartRat = keys[0];
          String EndRat = keys[1];

          // Expected number of Drops (filtered upon Start Rat)
          int expected = Clusters[cluster].GetEvents("Drop")
                                          .Where(evt => evt.StartRat == StartRat)
                                          .Where(evt => evt.EndRat == EndRat)
                                          .Count();

          // Actual drop value
          int actual = pair.Value[analysis.Key].Count;

          // Ensure that the two values are equal
          Assert.AreEqual(expected, actual);
        }
      }
    }

    /// <summary>
    /// This method will test to ensure that the correct number of Drops are 
    /// being returned based upon grouping by the Start Mix Band. This method 
    /// will test for drop figures across all clusters and intra-cluster drop 
    /// figures.
    /// </summary>
    [TestMethod]
    public void GetDropStartMixBandFiguresTest()
    {
      // Obtain the list of results
      Dictionary<int, AnalysisResults> results = Analysis.GetDropStartMixBandFigures();

      // Check to see if the Analysis numbers == Cluster Numbers
      Assert.AreEqual(Clusters.Count, results.Count);

      // Loop over each cluster
      foreach (KeyValuePair<int, AnalysisResults> pair in results)
      {
        // Cluster Number (0-Indexed Array)
        int cluster = pair.Key - 1;

        // Loop over each Intra-cluster analysis object
        foreach (KeyValuePair<String, EventCollection> analysis in pair.Value)
        {
          // Obtain the Start Rat Key
          String StartMixBand = analysis.Key;

          // Expected number of Drops (filtered upon Start Rat)
          int expected = Clusters[cluster].GetEvents("Drop")
                                          .Where(evt => evt.StartMixBand == StartMixBand)
                                          .Count();

          // Actual drop value
          int actual = pair.Value[StartMixBand].Count;

          // Ensure that the two values are equal
          Assert.AreEqual(expected, actual);
        }
      }
    }

    /// <summary>
    /// This method will test to ensure that the correct number of Drops are 
    /// being returned based upon grouping by the Start and End Mix Band. This
    /// method will test for drop figures across all clusters and intra-cluster
    /// drop figures.
    /// </summary>
    [TestMethod]
    public void GetDropStartEndMixBandFiguresTest()
    {
      // Obtain the list of results
      Dictionary<int, AnalysisResults> results = Analysis.GetDropStartEndMixBandFigures();

      // Check to see if the Analysis numbers == Cluster Numbers
      Assert.AreEqual(Clusters.Count, results.Count);

      // Loop over each cluster
      foreach (KeyValuePair<int, AnalysisResults> pair in results)
      {
        // Cluster Number (0-Indexed Array)
        int cluster = pair.Key - 1;

        // Loop over each Intra-cluster analysis object
        foreach (KeyValuePair<String, EventCollection> analysis in pair.Value)
        {
          // Obtain the Start Rat and End Rat Keys
          String[] keys = SplitKey(analysis.Key);
          String StartMixBand = keys[0];
          String EndMixBand = keys[1];

          // Expected number of Drops (filtered upon Start Rat)
          int expected = Clusters[cluster].GetEvents("Drop")
                                          .Where(evt => evt.StartMixBand == StartMixBand)
                                          .Where(evt => evt.EndMixBand == EndMixBand)
                                          .Count();

          // Actual drop value
          int actual = pair.Value[analysis.Key].Count;

          // Ensure that the two values are equal
          Assert.AreEqual(expected, actual);
        }
      }
    }

    /// <summary>
    /// This method will test to ensure that the correct number of Drops are 
    /// being returned based upon grouping by the Start RRC State. This method 
    /// will stest for drop figures across all clusters and intra-cluster drop 
    /// figures.
    /// </summary>
    [TestMethod]
    public void GetDropStartRRCStateFiguresTest()
    {
      // Obtain the list of results
      Dictionary<int, AnalysisResults> results = Analysis.GetDropStartRRCStateFigures();

      // Check to see if the Analysis numbers == Cluster Numbers
      Assert.AreEqual(Clusters.Count, results.Count);

      // Loop over each cluster
      foreach (KeyValuePair<int, AnalysisResults> pair in results)
      {
        // Cluster Number (0-Indexed Array)
        int cluster = pair.Key - 1;

        // Loop over each Intra-cluster analysis object
        foreach (KeyValuePair<String, EventCollection> analysis in pair.Value)
        {
          // Obtain the Start Rat Key
          String StartRRCState = analysis.Key;

          // Expected number of Drops (filtered upon Start Rat)
          int expected = Clusters[cluster].GetEvents("Drop")
                                          .Where(evt => evt.StartRRCState == StartRRCState)
                                          .Count();

          // Actual drop value
          int actual = pair.Value[StartRRCState].Count;

          // Ensure that the two values are equal
          Assert.AreEqual(expected, actual);
        }
      }
    }

    /// <summary>
    /// This method will test to ensure that the correct number of Fails are 
    /// being returned. This method will test for fail figures across all 
    /// clusters and intra-cluster fail figures.
    /// </summary>
    [TestMethod]
    public void GetFailFiguresTest()
    {
      // Obtain the list of results
      Dictionary<int, EventCollection> results = Analysis.GetFailFigures();

      // Check to see if the Analysis numbers == Cluster Numbers
      Assert.AreEqual(Clusters.Count, results.Count);

      // Check to ensure that each Cluster has the correct number of drops
      for (int i = 0; i < results.Count; i++)
      {
        // Expected drop value
        int expected = Clusters[i].GetEvents("Fail").Count();

        // Actual drop value
        int actual = results[i + 1].Count;

        // Ensure that the two values are equal
        Assert.AreEqual(expected, actual);
      }
    }

    /// <summary>
    /// This method will test to ensure that the correct number of Fails are 
    /// being returned based upon grouping by the Start RAT. This method will 
    /// test for fail figures across all clusters and intra-cluster fail 
    /// figures.
    /// </summary>
    [TestMethod]
    public void GetFailStartRatFiguresTest()
    {
      // Obtain the list of results
      Dictionary<int, AnalysisResults> results = Analysis.GetFailStartRatFigures();

      // Check to see if the Analysis numbers == Cluster Numbers
      Assert.AreEqual(Clusters.Count, results.Count);

      // Loop over each cluster
      foreach (KeyValuePair<int, AnalysisResults> pair in results)
      {
        // Cluster Number (0-Indexed Array)
        int cluster = pair.Key - 1;

        // Loop over each Intra-cluster analysis object
        foreach (KeyValuePair<String, EventCollection> analysis in pair.Value)
        {
          // Obtain the Start Rat Key
          String StartRat = analysis.Key;

          // Expected number of Drops (filtered upon Start Rat)
          int expected = Clusters[cluster].GetEvents("Fail")
                                          .Where(evt => evt.StartRat == StartRat)
                                          .Count();

          // Actual drop value
          int actual = pair.Value[StartRat].Count;

          // Ensure that the two values are equal
          Assert.AreEqual(expected, actual);
        }
      }
    }

    /// <summary>
    /// This method will test to ensure that the correct number of Fails are 
    /// being returned based upon grouping by the Start and End RAT. This 
    /// method will test for fail figures across all clusters and intra-cluster
    /// fail figures.
    /// </summary>
    [TestMethod]
    public void GetFailStartEndRatFiguresTest()
    {
      // Obtain the list of results
      Dictionary<int, AnalysisResults> results = Analysis.GetFailStartEndRatFigures();

      // Check to see if the Analysis numbers == Cluster Numbers
      Assert.AreEqual(Clusters.Count, results.Count);

      // Loop over each cluster
      foreach (KeyValuePair<int, AnalysisResults> pair in results)
      {
        // Cluster Number (0-Indexed Array)
        int cluster = pair.Key - 1;

        // Loop over each Intra-cluster analysis object
        foreach (KeyValuePair<String, EventCollection> analysis in pair.Value)
        {
          // Obtain the Start Rat and End Rat Keys
          String[] keys = SplitKey(analysis.Key);
          String StartRat = keys[0];
          String EndRat = keys[1];

          // Expected number of Drops (filtered upon Start Rat)
          int expected = Clusters[cluster].GetEvents("Fail")
                                          .Where(evt => evt.StartRat == StartRat)
                                          .Where(evt => evt.EndRat == EndRat)
                                          .Count();

          // Actual drop value
          int actual = pair.Value[analysis.Key].Count;

          // Ensure that the two values are equal
          Assert.AreEqual(expected, actual);
        }
      }
    }

    /// <summary>
    /// This method will test to ensure that the correct number of Fails are 
    /// being returned based upon grouping by the Start Mix Band. This method 
    /// will test for fail figures across all clusters and intra-cluster fail 
    /// figures.
    /// </summary>
    [TestMethod]
    public void GetFailStartMixBandFiguresTest()
    {
      // Obtain the list of results
      Dictionary<int, AnalysisResults> results = Analysis.GetFailStartMixBandFigures();

      // Check to see if the Analysis numbers == Cluster Numbers
      Assert.AreEqual(Clusters.Count, results.Count);

      // Loop over each cluster
      foreach (KeyValuePair<int, AnalysisResults> pair in results)
      {
        // Cluster Number (0-Indexed Array)
        int cluster = pair.Key - 1;

        // Loop over each Intra-cluster analysis object
        foreach (KeyValuePair<String, EventCollection> analysis in pair.Value)
        {
          // Obtain the Start Rat Key
          String StartMixBand = analysis.Key;

          // Expected number of Drops (filtered upon Start Rat)
          int expected = Clusters[cluster].GetEvents("Fail")
                                          .Where(evt => evt.StartMixBand == StartMixBand)
                                          .Count();

          // Actual drop value
          int actual = pair.Value[StartMixBand].Count;

          // Ensure that the two values are equal
          Assert.AreEqual(expected, actual);
        }
      }
    }

    /// <summary>
    /// This method will test to ensure that the correct number of Fails are 
    /// being returned based upon grouping by the Start and End Mix Band. This
    /// method will test for fail figures across all clusters and intra-cluster
    /// fail figures.
    /// </summary>
    [TestMethod]
    public void GetFailStartEndMixBandFiguresTest()
    {
      // Obtain the list of results
      Dictionary<int, AnalysisResults> results = Analysis.GetFailStartEndMixBandFigures();

      // Check to see if the Analysis numbers == Cluster Numbers
      Assert.AreEqual(Clusters.Count, results.Count);

      // Loop over each cluster
      foreach (KeyValuePair<int, AnalysisResults> pair in results)
      {
        // Cluster Number (0-Indexed Array)
        int cluster = pair.Key - 1;

        // Loop over each Intra-cluster analysis object
        foreach (KeyValuePair<String, EventCollection> analysis in pair.Value)
        {
          // Obtain the Start Rat and End Rat Keys
          String[] keys = SplitKey(analysis.Key);
          String StartMixBand = keys[0];
          String EndMixBand = keys[1];

          // Expected number of Drops (filtered upon Start Rat)
          int expected = Clusters[cluster].GetEvents("Fail")
                                          .Where(evt => evt.StartMixBand == StartMixBand)
                                          .Where(evt => evt.EndMixBand == EndMixBand)
                                          .Count();

          // Actual drop value
          int actual = pair.Value[analysis.Key].Count;

          // Ensure that the two values are equal
          Assert.AreEqual(expected, actual);
        }
      }
    }

    /// <summary>
    /// This method will test to ensure that the correct number of Fails are 
    /// being returned based upon grouping by the Start RRC State. This method 
    /// will test for fail figures across all clusters and intra-cluster fail 
    /// figures.
    /// </summary>
    [TestMethod]
    public void GetFailStartRRCStateFiguresTest()
    {
      // Obtain the list of results
      Dictionary<int, AnalysisResults> results = Analysis.GetFailStartRRCStateFigures();

      // Check to see if the Analysis numbers == Cluster Numbers
      Assert.AreEqual(Clusters.Count, results.Count);

      // Loop over each cluster
      foreach (KeyValuePair<int, AnalysisResults> pair in results)
      {
        // Cluster Number (0-Indexed Array)
        int cluster = pair.Key - 1;

        // Loop over each Intra-cluster analysis object
        foreach (KeyValuePair<String, EventCollection> analysis in pair.Value)
        {
          // Obtain the Start Rat Key
          String StartRRCState = analysis.Key;

          // Expected number of Drops (filtered upon Start Rat)
          int expected = Clusters[cluster].GetEvents("Fail")
                                          .Where(evt => evt.StartRRCState == StartRRCState)
                                          .Count();

          // Actual drop value
          int actual = pair.Value[StartRRCState].Count;

          // Ensure that the two values are equal
          Assert.AreEqual(expected, actual);
        }
      }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method will split a key up into its components. Each component is 
    /// split based upon the KeySeperator constant within this class.
    /// </summary>
    /// <param name="key">The key to be split up</param>
    /// <returns>A String array containing each component</returns>
    private String[] SplitKey(String key)
    {
      // Split the key
      return key.Split(new String[] { "=>" }, StringSplitOptions.None);
    }

    #endregion

  }
}
