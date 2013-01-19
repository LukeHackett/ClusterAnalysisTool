using Cluster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;

namespace Analysis
{
  public class WeekAnalysis
  {
    #region Properties

    /// <summary>
    /// The Week Number associcated with this data set
    /// </summary>
    public int WeekNumber { get; private set; }

    /// <summary>
    /// List of Events for the given week
    /// </summary>
    public List<EventCollection> Events { get; private set; }

    /// <summary>
    /// A List of each cluster contained within this week
    /// </summary>
    public Dictionary<int, ClusterAnalysis> Analysis { get; private set; }

    #endregion

    #region Constructor

    /// <summary>
    /// Primary Constructor
    /// </summary>
    /// <param name="events">A list of EventCollections representing clusters</param>
    /// <param name="weekNumber">The week number associated with the events</param>
    public WeekAnalysis(List<EventCollection> events, int weekNumber)
    {
      Events = events;
      WeekNumber = weekNumber;
      InitaliseWeekAnalysis();
    }

    #endregion

    #region Public Methods (Drops)

    /// <summary>
    /// This method will return all the dropped events grouped by the cluster ID 
    /// and the start RAT value.
    /// </summary>
    /// <returns>A Dictionary of Analysis results</returns>
    public Dictionary<int, AnalysisResults> GetDropStartRatFigures()
    {
      // Dictionary to hold the final results in.
      Dictionary<int, AnalysisResults> results = new Dictionary<int, AnalysisResults>();
      // Loop over each Cluster
      foreach (KeyValuePair<int, ClusterAnalysis> pair in Analysis)
      {
        // Get the index key (Cluster ID)
        int index = pair.Key;
        // Get the Start RAT figures
        AnalysisResults AnalysisResult = pair.Value.DropAnalysis.GroupByStartRat();
        // Add the figures to the dictionary using the cluster ID as the index
        results.Add(index, AnalysisResult);
      }
      
      return results;
    }

    /// <summary>
    /// This method will return all the dropped events grouped by the cluster ID 
    /// and the start/end RAT value.
    /// </summary>
    /// <returns>A Dictionary of Analysis results</returns>
    public Dictionary<int, AnalysisResults> GetDropStartEndRatFigures()
    {
      // Dictionary to hold the final results in.
      Dictionary<int, AnalysisResults> results = new Dictionary<int, AnalysisResults>();
      // Loop over each Cluster
      foreach (KeyValuePair<int, ClusterAnalysis> pair in Analysis)
      {
        // Get the index key (Cluster ID)
        int index = pair.Key;
        // Get the Start/End RAT figures
        AnalysisResults AnalysisResult = pair.Value.DropAnalysis.GroupByStartEndRat();
        // Add the figures to the dictionary using the cluster ID as the index
        results.Add(index, AnalysisResult);
      }

      return results;
    }

    /// <summary>
    /// This method will return all the dropped events grouped by the cluster ID 
    /// and the start Mix-Band value.
    /// </summary>
    /// <returns>A Dictionary of Analysis results</returns>
    public Dictionary<int, AnalysisResults> GetDropStartMixBandFigures()
    {
      // Dictionary to hold the final results in.
      Dictionary<int, AnalysisResults> results = new Dictionary<int, AnalysisResults>();
      // Loop over each Cluster
      foreach (KeyValuePair<int, ClusterAnalysis> pair in Analysis)
      {
        // Get the index key (Cluster ID)
        int index = pair.Key;
        // Get the Start Mix-Band figures
        AnalysisResults AnalysisResult = pair.Value.DropAnalysis.GroupByStartMixBand();
        // Add the figures to the dictionary using the cluster ID as the index
        results.Add(index, AnalysisResult);
      }

      return results;
    }

    /// <summary>
    /// This method will return all the dropped events grouped by the cluster ID 
    /// and the start/end Mix-Band value.
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, AnalysisResults> GetDropStartEndMixBandFigures()
    {
      // Dictionary to hold the final results in.
      Dictionary<int, AnalysisResults> results = new Dictionary<int, AnalysisResults>();
      // Loop over each Cluster
      foreach (KeyValuePair<int, ClusterAnalysis> pair in Analysis)
      {
        // Get the index key (Cluster ID)
        int index = pair.Key;
        // Get the Start/End Mix-Band figures
        AnalysisResults AnalysisResult = pair.Value.DropAnalysis.GroupByStartEndMixBand();
        // Add the figures to the dictionary using the cluster ID as the index
        results.Add(index, AnalysisResult);
      }

      return results;
    }

    /// <summary>
    /// This method will return all the dropped events grouped by the cluster ID 
    /// and the start RRC state value.
    /// </summary>
    /// <returns>A Dictionary of Analysis results</returns>
    public Dictionary<int, AnalysisResults> GetDropStartRRCStateFigures()
    {
      // Dictionary to hold the final results in.
      Dictionary<int, AnalysisResults> results = new Dictionary<int, AnalysisResults>();
      // Loop over each Cluster
      foreach (KeyValuePair<int, ClusterAnalysis> pair in Analysis)
      {
        // Get the index key (Cluster ID)
        int index = pair.Key;
        // Get the Start Mix-Band figures
        AnalysisResults AnalysisResult = pair.Value.DropAnalysis.GroupByStartRRCState();
        // Add the figures to the dictionary using the cluster ID as the index
        results.Add(index, AnalysisResult);
      }

      return results;
    }

    #endregion

    #region Public Methods (Fails)

    /// <summary>
    /// This method will return all the failed events grouped by the cluster ID 
    /// and the start RAT value.
    /// </summary>
    /// <returns>A Dictionary of Analysis results</returns>
    public Dictionary<int, AnalysisResults> GetFailStartRatFigures()
    {
      // Dictionary to hold the final results in.
      Dictionary<int, AnalysisResults> results = new Dictionary<int, AnalysisResults>();
      // Loop over each Cluster
      foreach (KeyValuePair<int, ClusterAnalysis> pair in Analysis)
      {
        // Get the index key (Cluster ID)
        int index = pair.Key;
        // Get the Start RAT figures
        AnalysisResults AnalysisResult = pair.Value.FailAnalysis.GroupByStartRat();
        // Add the figures to the dictionary using the cluster ID as the index
        results.Add(index, AnalysisResult);
      }

      return results;
    }

    /// <summary>
    /// This method will return all the failed events grouped by the cluster ID 
    /// and the start/end RAT value.
    /// </summary>
    /// <returns>A Dictionary of Analysis results</returns>
    public Dictionary<int, AnalysisResults> GetFailStartEndRatFigures()
    {
      // Dictionary to hold the final results in.
      Dictionary<int, AnalysisResults> results = new Dictionary<int, AnalysisResults>();
      // Loop over each Cluster
      foreach (KeyValuePair<int, ClusterAnalysis> pair in Analysis)
      {
        // Get the index key (Cluster ID)
        int index = pair.Key;
        // Get the Start/End RAT figures
        AnalysisResults AnalysisResult = pair.Value.FailAnalysis.GroupByStartEndRat();
        // Add the figures to the dictionary using the cluster ID as the index
        results.Add(index, AnalysisResult);
      }

      return results;
    }

    /// <summary>
    /// This method will return all the failed events grouped by the cluster ID 
    /// and the start Mix-Band value.
    /// </summary>
    /// <returns>A Dictionary of Analysis results</returns>
    public Dictionary<int, AnalysisResults> GetFailStartMixBandFigures()
    {
      // Dictionary to hold the final results in.
      Dictionary<int, AnalysisResults> results = new Dictionary<int, AnalysisResults>();
      // Loop over each Cluster
      foreach (KeyValuePair<int, ClusterAnalysis> pair in Analysis)
      {
        // Get the index key (Cluster ID)
        int index = pair.Key;
        // Get the Start Mix-Band figures
        AnalysisResults AnalysisResult = pair.Value.FailAnalysis.GroupByStartMixBand();
        // Add the figures to the dictionary using the cluster ID as the index
        results.Add(index, AnalysisResult);
      }

      return results;
    }

    /// <summary>
    /// This method will return all the failed events grouped by the cluster ID 
    /// and the start/end Mix-Band value.
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, AnalysisResults> GetFailStartEndMixBandFigures()
    {
      // Dictionary to hold the final results in.
      Dictionary<int, AnalysisResults> results = new Dictionary<int, AnalysisResults>();
      // Loop over each Cluster
      foreach (KeyValuePair<int, ClusterAnalysis> pair in Analysis)
      {
        // Get the index key (Cluster ID)
        int index = pair.Key;
        // Get the Start/End Mix-Band figures
        AnalysisResults AnalysisResult = pair.Value.FailAnalysis.GroupByStartEndMixBand();
        // Add the figures to the dictionary using the cluster ID as the index
        results.Add(index, AnalysisResult);
      }

      return results;
    }

    /// <summary>
    /// This method will return all the failed events grouped by the cluster ID 
    /// and the start RRC state value.
    /// </summary>
    /// <returns>A Dictionary of Analysis results</returns>
    public Dictionary<int, AnalysisResults> GetFailStartRRCStateFigures()
    {
      // Dictionary to hold the final results in.
      Dictionary<int, AnalysisResults> results = new Dictionary<int, AnalysisResults>();
      // Loop over each Cluster
      foreach (KeyValuePair<int, ClusterAnalysis> pair in Analysis)
      {
        // Get the index key (Cluster ID)
        int index = pair.Key;
        // Get the Start Mix-Band figures
        AnalysisResults AnalysisResult = pair.Value.FailAnalysis.GroupByStartRRCState();
        // Add the figures to the dictionary using the cluster ID as the index
        results.Add(index, AnalysisResult);
      }

      return results;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method will setup the WeekAnalysis object held within this object. 
    /// Each collection within the Events object will form a new ClusterAnalysis 
    /// object within the WeekAnalysis dictionary.
    /// </summary>
    private void InitaliseWeekAnalysis()
    {
      // Setup the analysis object
      Analysis = new Dictionary<int, ClusterAnalysis>();
      // Loop over all known clusters
      foreach (EventCollection collection in Events)
      {
        if (collection.Count > 0)
        {
          // Get the cluster ID based upon the first event's coordinate
          int clusterID = collection[0].Coordinate.ClusterId;
          // Add a new ClusterAnalysis object to the list
          Analysis.Add(clusterID, new ClusterAnalysis(collection, clusterID));
        }
      }
    }

    #endregion

  }
}
