/// Copyright (c) 2012, Research In Motion Limited.
/// All rights reserved.
/// 
/// Redistribution and use in source and binary forms, with or without 
/// modification, are permitted provided that the following conditions are met:
/// 
///  [*] Redistributions of source code must retain the above copyright notice, 
///      this list of conditions and the following disclaimer.
///  [*] Redistributions in binary form must reproduce the above copyright 
///      notice, this list of conditions and the following disclaimer in the 
///      documentation and/or other materials provided with the distribution.
///  [*] Neither the name of the Research In Motion Limited nor the names of its 
///      contributors may be used to endorse or promote products derived from 
///      this software without specific prior written permission.
/// 
/// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
/// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
/// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
/// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE 
/// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
/// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
/// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
/// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
/// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
/// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
/// POSSIBILITY OF SUCH DAMAGE.

using Cluster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;

namespace Analysis
{
  public class WeekAnalysis : Analysis
  {
    /// <summary>
    /// The Week Number associcated with this data set
    /// </summary>
    public int WeekNumber { get; private set; }

    #region Constructor

    /// <summary>
    /// Primary Constructor
    /// </summary>
    /// <param name="events">A list of EventCollections representing clusters</param>
    /// <param name="weekNumber">The week number associated with the events</param>
    public WeekAnalysis(List<EventCollection> events, int weekNumber)
      :base()
    {
      Events = events;
      WeekNumber = weekNumber;
      InitaliseAnalysis();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// This method will produce a JSON object that contains all the data from 
    /// the current instance. Only focusing upon drop events, this method will 
    /// firstly group the events by the start RAT only, and then by the start 
    /// and end RAT values.
    /// </summary>
    /// <returns>A well formed JSON string</returns>
    public String CreateDropRATJSON()
    {
      // Create a new JSONResults Data Structure
      JSONResults json = new JSONResults(WeekNumber);
      // Loop over all clusters in this week
      foreach (KeyValuePair<int, ClusterAnalysis> pair in ClustersAnalysis)
      {
        // Get the Drop Analysis object
        DropAnalysis analysis = pair.Value.DropAnalysis;
        // Used to stores the split up events
        List<String> keys = analysis.GroupByStartRat().Keys.ToList();
        AnalysisResultsCollection groups = new AnalysisResultsCollection(keys);
        // Get the Analysis result
        AnalysisResults results = analysis.GroupByStartEndRat();
        // Loop over each result
        foreach (KeyValuePair<String, EventCollection> p in results)
        {
          // Calculate the Key
          String key = p.Value[0].StartRat;
          // Add into the array
          groups[key].Add(p.Key, p.Value);
        }
        // Add the RAT goup to the JSONResults Data Structure
        json.Add(pair.Key, groups);
      }
      // return the JSON data structure for this week
      return json.ToString();
    }

    /// <summary>
    /// This method will produce a JSON object that contains all the data from 
    /// the current instance. Only focusing upon fail events, this method will 
    /// firstly group the events by the start RAT only, and then by the start 
    /// and end RAT values.
    /// </summary>
    /// <returns>A well formed JSON string</returns>
    public String CreateFailRATJSON()
    {
      // Create a new JSONResults Data Structure
      JSONResults json = new JSONResults(WeekNumber);
      // Loop over all clusters in this week
      foreach (KeyValuePair<int, ClusterAnalysis> pair in ClustersAnalysis)
      {
        // Get the Drop Analysis object
        FailAnalysis analysis = pair.Value.FailAnalysis;
        // Used to stores the split up events
        List<String> keys = analysis.GroupByStartRat().Keys.ToList();
        AnalysisResultsCollection groups = new AnalysisResultsCollection(keys);
        // Get the Analysis result
        AnalysisResults results = analysis.GroupByStartEndRat();
        // Loop over each result
        foreach (KeyValuePair<String, EventCollection> p in results)
        {
          // Calculate the Key
          String key = p.Value[0].StartRat;
          // Add into the array
          groups[key].Add(p.Key, p.Value);
        }
        // Add the RAT goup to the JSONResults Data Structure
        json.Add(pair.Key, groups);
      }
      // return the JSON data structure for this week
      return json.ToString();
    }

    /// <summary>
    /// This method will produce a JSON object that contains all the data from 
    /// the current instance. Only focusing upon drop events, this method will 
    /// firstly group the events by the start MixBand only, and then by the 
    /// start and end MixBand values.
    /// </summary>
    /// <returns>A well formed JSON string</returns>
    public String CreateDropMixBandJSON()
    {
      // Create a new JSONResults Data Structure
      JSONResults json = new JSONResults(WeekNumber);
      // Loop over all clusters in this week
      foreach (KeyValuePair<int, ClusterAnalysis> pair in ClustersAnalysis)
      {
        // Get the Drop Analysis object
        DropAnalysis analysis = pair.Value.DropAnalysis;
        // Used to stores the split up events
        List<String> keys = analysis.GroupByStartMixBand().Keys.ToList();
        AnalysisResultsCollection groups = new AnalysisResultsCollection(keys);
        // Get the Analysis result
        AnalysisResults results = analysis.GroupByStartEndMixBand();
        // Loop over each result
        foreach (KeyValuePair<String, EventCollection> p in results)
        {
          // Calculate the Key
          String key = p.Value[0].StartMixBand;
          // Add into the array
          groups[key].Add(p.Key, p.Value);
        }
        // Add the Mix-Band goup to the JSONResults Data Structure
        json.Add(pair.Key, groups);
      }
      // return the JSON data structure for this week
      return json.ToString();
    }

    /// <summary>
    /// This method will produce a JSON object that contains all the data from 
    /// the current instance. Only focusing upon fail events, this method will 
    /// firstly group the events by the start MixBand only, and then by the 
    /// start and end MixBand values.
    /// </summary>
    /// <returns>A well formed JSON string</returns>
    public String CreateFailMixBandJSON()
    {
      // Create a new JSONResults Data Structure
      JSONResults json = new JSONResults(WeekNumber);
      // Loop over all clusters in this week
      foreach (KeyValuePair<int, ClusterAnalysis> pair in ClustersAnalysis)
      {
        // Get the Drop Analysis object
        FailAnalysis analysis = pair.Value.FailAnalysis;
        // Used to stores the split up events
        List<String> keys = analysis.GroupByStartMixBand().Keys.ToList();
        AnalysisResultsCollection groups = new AnalysisResultsCollection(keys);
        // Get the Analysis result
        AnalysisResults results = analysis.GroupByStartEndMixBand();
        // Loop over each result
        foreach (KeyValuePair<String, EventCollection> p in results)
        {
          // Calculate the Key
          String key = p.Value[0].StartMixBand;
          // Add into the array
          groups[key].Add(p.Key, p.Value);
        }
        // Add the Mix-Band goup to the JSONResults Data Structure
        json.Add(pair.Key, groups);
      }
      // return the JSON data structure for this week
      return json.ToString();
    }

    #endregion

  }
}
