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

using JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analysis
{
  public class JSONResults : Dictionary<int, AnalysisResultsCollection>
  {
    #region Properties

    /// <summary>
    /// The week number of the cluster
    /// </summary>
    public int WeekNumber { get; private set; }
    
    /// <summary>
    /// The name of the device of the cluster
    /// </summary>
    public String Device { get; private set; }

    #endregion

    #region Constructor

    /// <summary>
    /// Primary Constructor
    /// </summary>
    /// <param name="week_number">The week number associated with the cluster</param>
    public JSONResults(int week_number)
    {
      WeekNumber = week_number;
    }

    /// <summary>
    /// Secondary Constructor
    /// </summary>
    /// <param name="device">The device name associated with the cluster</param>
    public JSONResults(String device)
    {
      Device = device;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// This method will return a well formatted JSON String based upon the 
    /// elements that are held within this object.
    /// </summary>
    /// <returns>A well formatted JSON String</returns>
    public override String ToString()
    {
      // List of each JSON cluster
      List<String> clusters = new List<String>();
      // Loop over each of the clusters
      foreach(KeyValuePair<int, AnalysisResultsCollection> pair in this)
      {
        // Generate the cluster name
        String cluster_name = JSONWriter.CreateKeyValue("name", "Cluster " + pair.Key);
        // Used to store the children JSON array
        List<String> cluster_children = new List<String>();
        // Loop over the children data (E.g. 2G, or 3G, or Freq1)
        foreach (KeyValuePair<String, AnalysisResults> results in pair.Value)
        {
          // Generate the name
          String name = JSONWriter.CreateKeyValue("name", results.Key);
          // Generate the Children array
          String children = JSONWriter.CreateJSONArray("children", results.Value.ToJSON());
          // merge the two above together
          String element = JSONWriter.CreateJSONObject(name, children);
          // Add to the cluster
          cluster_children.Add(element);
        }
        // Merge all the children values into an Array
        String children_values = JSONWriter.CreateJSONArray("children", cluster_children);
        // Create the cluster JSON Object
        String cluster = JSONWriter.CreateJSONObject(cluster_name, children_values);
        // Add the cluster JSON object to all the known clusters
        clusters.Add(cluster);
      }
      // Format the key (either the device name or the week number)
      String key = GetDataKey();
      // Format the children data
      String data = JSONWriter.CreateJSONArray("children", clusters);
      // Final Formatting of the string
      return JSONWriter.CreateJSONObject(key, data);
    }

    /// <summary>
    /// This method will save the current object to the specified file, 
    /// overwriting any existing file
    /// </summary>
    /// <param name="file">Filename of where the data is to be stored</param>
    public void Save(String file)
    {
      // Obtain the JSON representation
      String json = ToString();
      // Save to the given file path
      System.IO.File.WriteAllText(file, json);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method will create a well formed key based upon the values of 
    /// Device and WeekNumber. If the device is null, then the week number 
    /// will be used to create a name key. E.g. "name": "9800" or 
    /// "name": "Week 1".
    /// </summary>
    /// <returns>A well formed key/value pair</returns>
    private String GetDataKey()
    {
      // Return the device name as a key if avaiable.
      if (Device != null)
      {
        return JSONWriter.CreateKeyValue("name", Device);
      }

      return JSONWriter.CreateKeyValue("name", "Week " + WeekNumber);     
    }

    #endregion
  }
}
