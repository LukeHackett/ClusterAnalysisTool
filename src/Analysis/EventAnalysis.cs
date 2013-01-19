using Cluster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Analysis
{
  public abstract class EventAnalysis
  {
    #region Properties

    /// <summary>
    /// The collection of events representing a cluster.
    /// </summary>
    public EventCollection Cluster { get; protected set; }    

    /// <summary>
    /// The seperator character(s) between the multi-keys.
    /// </summary>
    private const String KeySeperator = "=>";

    #endregion

    #region Public Methods

    /// <summary>
    /// This method will return the total number of drop events held within this 
    /// object.
    /// </summary>
    /// <returns>The total number of drop events</returns>
    public int GetTotalDropCount()
    {
      return Cluster.Count();
    }

    /// <summary>
    /// This method will return the total number of drop events held within this 
    /// object that dropped upon the given start RAT.
    /// </summary>
    /// <param name="startrat">The start RAT value</param>
    /// <returns>Total number of drops upon the given start RAT</returns>
    public int GetRatCount(String startrat)
    {
      return Cluster.Where(evt => evt.StartRat == startrat).Count();
    }

    /// <summary>
    /// This method will return the total number of drop events held within this 
    /// object that dropped upon the given start/end RAT.
    /// </summary>
    /// <param name="startrat">The start RAT value</param>
    /// <param name="endrat">The end RAT value</param>
    /// <returns>Total number of drops upon the given start/end RAT</returns>
    public int GetRatCount(String startrat, String endrat)
    {
      return Cluster.Where(evt => evt.StartRat == startrat)
                    .Where(evt => evt.EndRat == endrat).Count();
    }


    /// <summary>
    /// This method will return the total number of drop events held within this 
    /// object that dropped upon the given start frequency.
    /// </summary>
    /// <param name="start">The start Mix-Band value</param>
    /// <returns>Total number of drops on the given frequency</returns>
    public int GetMixBandCount(String start)
    {
      return Cluster.Where(evt => evt.StartMixBand == start).Count();
    }


    /// <summary>
    /// This method will return the total number of drop events held within this 
    /// object that dropped upon the given start/end frequency.
    /// </summary>
    /// <param name="start">The start Mix-Band value</param>
    /// <param name="end">The end Mix-Band value</param>
    /// <returns>Total number of drops on the given frequency</returns>
    public int GetMixBandCount(String start, String end)
    {
      return Cluster.Where(evt => evt.StartMixBand == start)
                    .Where(evt => evt.EndMixBand == end)
                    .Count();
    }

    /// <summary>
    /// This method will return all the events grouped by the cluster ID and the 
    /// start RAT value.
    /// </summary>
    /// <returns>A Dictionary of Analysis results</returns>
    public AnalysisResults GroupByStartRat() 
    {
      // Group all events by the Start RAT only.
      var query = Cluster.GetEvents()
                         .GroupBy(evt => new
                          {
                            evt.StartRat,
                          },
                          (key, group) => new
                          {
                            StartRat = key.StartRat,
                            Events = group
                          });
      // Setup a new results object
      AnalysisResults results = new AnalysisResults();
      // Covert each group to an EventCollection
      foreach (var row in query)
      {
        results.Add(row.StartRat, new EventCollection(row.Events));        
      }
      return results;
    }

    /// <summary>
    /// This method will return all the events grouped by the cluster ID and the 
    /// start/end RAT value.
    /// </summary>
    /// <returns>A Dictionary of Analysis results</returns>
    public AnalysisResults GroupByStartEndRat()
    {
      // Group all events by the Start RAT and End RAT only.
      var query = Cluster.GetEvents()
                         .GroupBy(evt => new
                         {
                           evt.StartRat,
                           evt.EndRat
                         },
                         (key, group) => new
                         {
                           StartRat = key.StartRat,
                           EndRat = key.EndRat,
                           Events = group
                         });
      // Setup a new results object
      AnalysisResults results = new AnalysisResults();
      // Covert each group to an EventCollection
      foreach (var row in query)
      {
        String key = GetKeySeperator(row.StartRat, row.EndRat);
        results.Add(key, new EventCollection(row.Events));
      }
      return results;
    }

    /// <summary>
    /// This method will return all the events grouped by the cluster ID and the 
    /// start Mix-Band value.
    /// </summary>
    /// <returns>A Dictionary of Analysis results</returns>
    public AnalysisResults GroupByStartMixBand()
    {
      // Group all events by the Start Mix-Band only.
      var query = Cluster.GetEvents()
                         .GroupBy(evt => new
                         {
                           evt.StartMixBand,
                         },
                         (key, group) => new
                         {
                           StartMixBand = key.StartMixBand,
                           Events = group
                         });
      // Setup a new results object
      AnalysisResults results = new AnalysisResults();
      // Covert each group to an EventCollection
      foreach (var row in query)
      {
        results.Add(row.StartMixBand, new EventCollection(row.Events));
      }
      return results;
    }

    /// <summary>
    /// This method will return all the events grouped by the cluster ID and the 
    /// start/end Mix-Band value.
    /// </summary>
    /// <returns>A Dictionary of Analysis results</returns>
    public AnalysisResults GroupByStartEndMixBand()
    {
      // Group all events by the Start Mix-Band and End Mix-Band only.
      var query = Cluster.GetEvents()
                         .GroupBy(evt => new
                         {
                           evt.StartMixBand,
                           evt.EndMixBand
                         },
                         (key, group) => new
                         {
                           StartMixBand = key.StartMixBand,
                           EndMixBand = key.EndMixBand,
                           Events = group
                         });
      // Setup a new results object
      AnalysisResults results = new AnalysisResults();
      // Covert each group to an EventCollection
      foreach (var row in query)
      {
        String key = GetKeySeperator(row.StartMixBand, row.EndMixBand);
        results.Add(key, new EventCollection(row.Events));
      }
      return results;
    }

    /// <summary>
    /// This method will return all the events grouped by the cluster ID and the 
    /// start RRC state.
    /// </summary>
    /// <returns>A Dictionary of Analysis results</returns>
    public AnalysisResults GroupByStartRRCState()
    {
      // Group all events by the Start RRC State only.
      var query = Cluster.GetEvents()
                         .GroupBy(evt => new
                         {
                           evt.StartRRCState,
                         },
                         (key, group) => new
                         {
                           StartRRCState = key.StartRRCState,
                           Events = group
                         });
      // Setup a new results object
      AnalysisResults results = new AnalysisResults();
      // Covert each group to an EventCollection
      foreach (var row in query)
      {
        results.Add(row.StartRRCState, new EventCollection(row.Events));
      }
      return results;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method will produce a single String key based upon the two given 
    /// String key values. The keys are seperated by a declared character 
    /// sequence.
    /// </summary>
    /// <param name="key1">The first key</param>
    /// <param name="key2">The second key</param>
    /// <returns>A String key</returns>
    private String GetKeySeperator(String key1, String key2)
    {
      return String.Format("{0}{1}{2}", key1, KeySeperator, key2);
    }

    #endregion
  }

}
