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
using JSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analysis
{
  public class MultiWeekAnalysis
  {
    #region Properties

    /// <summary>
    /// The multi-week collection of events
    /// </summary>
    public EventCollection Events { get; private set; }

    /// <summary>
    /// The DBSCAN object that performs the multi-week clustering.
    /// </summary>
    private DBSCAN DBscan;

    /// <summary>
    /// A list of multi-week analysis objects
    /// </summary>
    public Dictionary<int, WeekAnalysis> WeeklyAnalysis { get; private set; }

    /// <summary>
    /// A list of product analysis objects
    /// </summary>
    public Dictionary<String, ProductAnalysis> ProductsAnalysis { get; private set; }

    #endregion
    
    #region Constructors

    /// <summary>
    /// Primary Constructor
    /// </summary>
    public MultiWeekAnalysis()
    {
      Events = new EventCollection();
    }

    #endregion

    #region Public Methods
    
    /// <summary>
    /// This method will add a given event to the list of events. Any previous 
    /// clustering information will be removed.
    /// </summary>
    /// <param name="evt">The event to be added</param>
    public void Add(Event evt)
    {
      // Remove any previous clustering data
      evt.Coordinate.ClearClusterData();
      // Add the event to the EventCollection
      Events.Add(evt);
    }

    /// <summary>
    /// This method will add an exisitng EventCollection to the EventCollection 
    /// object within this class. Any previous clustering information will be 
    /// removed.
    /// </summary>
    /// <param name="collection">The EventCollection to be added</param>
    public void AddRange(EventCollection collection)
    {
      // Remove any previous clustering data
      collection.ClearAllClusterData();
      // Add the events to the EventCollection
      Events.AddRange(collection);
    }

    /// <summary>
    /// This method will add a number of exisitng EventCollections being held 
    /// within a list to the EventCollection object within this class. Any 
    /// previous clustering information will be removed.
    /// </summary>
    /// <param name="collections">A list ov EventCollections</param>
    public void AddRange(List<EventCollection> collections)
    {
      foreach (EventCollection collection in collections)
      {
        AddRange(collection);
      }
    }

    /// <summary>
    /// This method will initalise the clustering algorithm, in order to cluster
    /// the data set held within this object.
    /// </summary>
    public void Cluster()
    {
      // Cluster the data
      DBscan = new DBSCAN(Events);
      DBscan.Analyse();
    }

    /// <summary>
    /// This method will analyse the clustered events upon a Multi-Week basis.
    /// This means that events will be grouped by their week number deduced 
    /// from the timestamp, and comparisons of events will be made from this 
    /// aspect only. This method does not take the device name into 
    /// consideration when grouping and analysing.
    /// </summary>
    public void AnalyseMultiWeek()
    {
      // Ensure the data has been clustered
      if (DBscan == null)
      {
        throw new Exception("Data has not been clustered.");
      }

      // Setup the analysis storage objects
      WeeklyAnalysis = new Dictionary<int, WeekAnalysis>();

      // Split the clustered data into the various weeks
      CreateWeeklyAnalysis();
    }

    /// <summary>
    /// This method will return a WeekAnalysis based upon the given week number.
    /// If a week does not exist, an exception is thrown.
    /// </summary>
    /// <param name="weekNumber">The week number of the analysis</param>
    /// <returns>A week analysis object for the given week</returns>
    public WeekAnalysis AnalyseWeek(int weekNumber)
    {
      // Ensure that the given key exists 
      if (!WeeklyAnalysis.ContainsKey(weekNumber))
      {
        throw new Exception("Key: " + weekNumber + " does not exist.");
      }

      return WeeklyAnalysis[weekNumber];
    }

    /// <summary>
    /// This method will analyse the clustered events upon a Product basis. 
    /// This means that products will be grouped by their device name, and
    /// comparisons of events will be made from a product perspective. This 
    /// method does not take the timestamps into consideration when grouping 
    /// and analysing.
    /// </summary>
    public void AnalyseProducts()
    {
      // Ensure the data has been clustered
      if (DBscan == null)
      {
        throw new Exception("Data has not been clustered.");
      }

      // Setup the analysis storage objects
      ProductsAnalysis = new Dictionary<String, ProductAnalysis>();

      // Split the clustered data into the various weeks
      CreateProductAnalysis();
    }

    /// <summary>
    /// This method will return a ProductAnalysis object based upon the given 
    /// device name. If the device name does not exist, an exception is thrown.
    /// </summary>
    /// <param name="device">The name of the device</param>
    /// <returns>A product analysis object for the given device</returns>
    public ProductAnalysis AnalyseProduct(String device)
    {
      // Ensure that the given key exists 
      if (!ProductsAnalysis.ContainsKey(device))
      {
        throw new Exception("Devce: " + device + " does not exist.");
      }

      return ProductsAnalysis[device];
    }

    /// <summary>
    /// This method is the main entry point to create the various weekly JSON 
    /// analysis files. A total of four files will be created, one for each 
    /// type of analysis (DropRAT, DropMixBand, FailRAT, FailMixBand) within the 
    /// specified output directory. Minification of the file is available to 
    /// save space if required.
    /// </summary>
    /// <param name="directory">The output directory</param>
    /// <param name="minify">Whether or not to minify the results</param>
    public void CreateWeeklyAnalysisJSON(String directory, Boolean minify = true)
    {
      // Ensure there is a trailing backslash upon the directory
      directory = directory.TrimEnd('\\') + @"\";

      // Obtain the JSON String
      String drop_rats = MergeDropRATJSON();
      String drop_mixbands = MergeDropMixBandJSON();
      String fail_rats = MergeFailRATJSON();
      String fail_mixbands = MergeFailMixBandJSON();

      // Check to see if the output requires prettifying (un-minifying)
      if (!minify)
      {
        drop_rats = JSONWriter.PrettifyJSON(drop_rats);
        drop_mixbands = JSONWriter.PrettifyJSON(drop_mixbands);
        fail_rats = JSONWriter.PrettifyJSON(fail_rats);
        fail_mixbands = JSONWriter.PrettifyJSON(fail_mixbands);
      }

      // Create each output file
      System.IO.File.WriteAllText(directory + "week_drop_rats.json", drop_rats);
      System.IO.File.WriteAllText(directory + "week_drop_mix_band.json", drop_mixbands);
      System.IO.File.WriteAllText(directory + "week_fail_rats.json", fail_rats);
      System.IO.File.WriteAllText(directory + "week_fail_mix_band.json", fail_mixbands);
    }

    /// <summary>
    /// This method will merge all the Drop RAT JSON formatted strings for each 
    /// given week within this object. The method will the ultimatley return a
    /// new JSON string which will contain the multi-week analysis.
    /// </summary>
    /// <returns>A well formed JSON string</returns>
    private String MergeDropRATJSON()
    {
      // Stores all the JSON strings for each week
      List<String> weeks = new List<String>();
      // Loop over each week
      foreach (KeyValuePair<int, WeekAnalysis> pair in WeeklyAnalysis)
      {
        // Create the JSON String
        String json = pair.Value.CreateDropRATJSON();
        // Add the string to the list of weeks
        weeks.Add(json);
      }
      // Create the name of the data structure
      String name = JSONWriter.CreateKeyValue("name", "Drop Events RAT Usage");
      // Create the children JSON array
      String children = JSONWriter.CreateJSONArray("children", weeks);
      // Create and return the final JSON object
      return JSONWriter.CreateJSONObject(name, children);        
    }

    /// <summary>
    /// This method will merge all the Drop MixBand JSON formatted strings for 
    /// each given week within this object. The method will the ultimatley 
    /// return a new JSON string which will contain the multi-week analysis.
    /// </summary>
    /// <returns>A well formed JSON string</returns>
    private String MergeDropMixBandJSON()
    {
      // Stores all the JSON strings for each week
      List<String> weeks = new List<String>();
      // Loop over each week
      foreach (KeyValuePair<int, WeekAnalysis> pair in WeeklyAnalysis)
      {
        // Create the JSON String
        String json = pair.Value.CreateDropMixBandJSON();
        // Add the string to the list of weeks
        weeks.Add(json);
      }
      // Create the name of the data structure
      String name = JSONWriter.CreateKeyValue("name", "Drop Events Mix-Band Usage");
      // Create the children JSON array
      String children = JSONWriter.CreateJSONArray("children", weeks);
      // Create and return the final JSON object
      return JSONWriter.CreateJSONObject(name, children);    
    }

    /// <summary>
    /// This method will merge all the Fail RAT JSON formatted strings for each 
    /// given week within this object. The method will the ultimatley return a
    /// new JSON string which will contain the multi-week analysis.
    /// </summary>
    /// <returns>A well formed JSON string</returns>
    private String MergeFailRATJSON()
    {
      // Stores all the JSON strings for each week
      List<String> weeks = new List<String>();
      // Loop over each week
      foreach (KeyValuePair<int, WeekAnalysis> pair in WeeklyAnalysis)
      {
        // Create the JSON String
        String json = pair.Value.CreateFailRATJSON();
        // Add the string to the list of weeks
        weeks.Add(json);
      }
      // Create the name of the data structure
      String name = JSONWriter.CreateKeyValue("name", "Fail Events RAT Usage");
      // Create the children JSON array
      String children = JSONWriter.CreateJSONArray("children", weeks);
      // Create and return the final JSON object
      return JSONWriter.CreateJSONObject(name, children); 
    }

    /// <summary>
    /// This method will merge all the Fail MixBand JSON formatted strings for 
    /// each given week within this object. The method will the ultimatley 
    /// return a new JSON string which will contain the multi-week analysis.
    /// </summary>
    /// <returns>A well formed JSON string</returns>
    private String MergeFailMixBandJSON()
    {
      // Stores all the JSON strings for each week
      List<String> weeks = new List<String>();
      // Loop over each week
      foreach (KeyValuePair<int, WeekAnalysis> pair in WeeklyAnalysis)
      {
        // Create the JSON String
        String json = pair.Value.CreateFailMixBandJSON();
        // Add the string to the list of weeks
        weeks.Add(json);
      }
      // Create the name of the data structure
      String name = JSONWriter.CreateKeyValue("name", "Fail Events Mix-Band Usage");
      // Create the children JSON array
      String children = JSONWriter.CreateJSONArray("children", weeks);
      // Create and return the final JSON object
      return JSONWriter.CreateJSONObject(name, children); 
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method will create the analysis object when wanting to analyse the 
    /// events utilising the week as the main pivot point.
    /// </summary>
    private void CreateWeeklyAnalysis()
    {
      // Get a list of non-noise calls and group by week number
      var results = DBscan.Calls.Where(evt => evt.Coordinate.Noise == false)
                                .GroupBy(evt => evt.GetWeekNumber());
      // Loop over each grouped calls
      foreach (var result in results)
      {
        // Get the key - Week Number
        int key = result.Key;
        // Setup a new list of EventCollections to store all clusters
        List<EventCollection> week = new List<EventCollection>();
        // Start from 1 as 0 reserved for noise
        for(int i = 1; i < DBscan.ClusteredEvents.Count; i++)
        {
          // Obtain the cluster from the inital results set
          var cluster = result.Where(evt => evt.Coordinate.ClusterId == i);
          // Add the cluster to the week's worth of clusters
          week.Add(new EventCollection(cluster));
        }
        // Add the week analysis to the known weeks
        WeeklyAnalysis.Add(key, new WeekAnalysis(week, key));
      }
    }

    /// <summary>
    /// This method will create the analysis object when wanting to analyse the 
    /// events utilising the product (name) as the main pivot point.
    /// </summary
    private void CreateProductAnalysis()
    {
      // Get a list of non-noise calls and group by device name
      var results = DBscan.Calls.Where(evt => evt.Coordinate.Noise == false)
                                .GroupBy(evt => evt.Device);
      // Loop over each grouped calls
      foreach (var result in results)
      {
        // Get the key - Device Name
        String key = result.Key;
        // Setup a new list of EventCollections to store all clusters
        List<EventCollection> week = new List<EventCollection>();
        // Start from 1 as 0 reserved for noise
        for (int i = 1; i < DBscan.ClusteredEvents.Count; i++)
        {
          // Obtain the cluster from the inital results set
          var cluster = result.Where(evt => evt.Coordinate.ClusterId == i);
          // Add the cluster to the week's worth of clusters
          week.Add(new EventCollection(cluster));
        }
        // Add the week analysis to the known weeks
        ProductsAnalysis.Add(key, new ProductAnalysis(week, key));
      }
    }

    #endregion
  }
}
