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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Analysis
{
  public class MultiProductAnalysis : MultiAnalysis
  {
    #region Properties

    /// <summary>
    /// A list of device analysis objects
    /// </summary>
    public Dictionary<String, ProductAnalysis> ProductsAnalysis { get; private set; }

    #endregion

    #region Public Methods

    /// <summary>
    /// This method will analyse the clustered events upon a Multi-device basis.
    /// This means that events will be grouped by their device name, and 
    /// comparisons of events will be made from this aspect only. 
    /// This method does not take any other attribute into consideration when 
    /// grouping and analysing.
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

      // Split the clustered data into the various devices
      CreateProductAnalysis();
    }

    /// <summary>
    /// This method will return a ProductAnalysis object based upon the given 
    /// device name. If the device name does not exist, an exception is thrown.
    /// </summary>
    /// <param name="device">The name of the device</param>
    /// <returns>A product analysis object for the given week</returns>
    public ProductAnalysis AnalyseProduct(String device)
    {
      // Ensure that the given key exists 
      if (!ProductsAnalysis.ContainsKey(device))
      {
        throw new Exception("Key: " + device + " does not exist.");
      }

      return ProductsAnalysis[device];
    }

    /// <summary>
    /// This method is the main entry point to create the various device JSON 
    /// analysis files. A total of four files will be created, one for each 
    /// type of analysis (DropRAT, DropMixBand, FailRAT, FailMixBand) within the 
    /// specified output directory. Minification of the file is available to 
    /// save space if required.
    /// </summary>
    /// <param name="directory">The output directory</param>
    /// <param name="minify">Whether or not to minify the results</param>
    public void CreateProductsAnalysisJSON(String directory, Boolean minify = true)
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
      File.WriteAllText(directory + "product_drop_rat.json", drop_rats);
      File.WriteAllText(directory + "product_drop_mix_band.json", drop_mixbands);
      File.WriteAllText(directory + "product_fail_rat.json", fail_rats);
      File.WriteAllText(directory + "product_fail_mix_band.json", fail_mixbands);
    }

    /// <summary>
    /// This method will merge all the Drop RAT JSON formatted strings for each 
    /// given device within this object. The method will the ultimately return 
    /// a new JSON string which will contain the multi-product analysis.
    /// </summary>
    /// <returns>A well formed JSON string</returns>
    private String MergeDropRATJSON()
    {
      // Stores all the JSON strings for each device
      List<String> devices = new List<String>();
      // Loop over each device
      foreach (KeyValuePair<String, ProductAnalysis> pair in ProductsAnalysis)
      {
        // Create the JSON String
        String json = pair.Value.CreateDropRATJSON();
        // Add the string to the list of devices
        devices.Add(json);
      }
      // Create the name of the data structure
      String name = JSONWriter.CreateKeyValue("name", "Drop Events RAT Usage");
      // Create the children JSON array
      String children = JSONWriter.CreateJSONArray("children", devices);
      // Create and return the final JSON object
      return JSONWriter.CreateJSONObject(name, children);
    }

    /// <summary>
    /// This method will merge all the Drop MixBand JSON formatted strings for 
    /// each given device within this object. The method will ultimately return 
    /// a new JSON string which will contain the multi-product analysis.
    /// </summary>
    /// <returns>A well formed JSON string</returns>
    private String MergeDropMixBandJSON()
    {
      // Stores all the JSON strings for each device
      List<String> devices = new List<String>();
      // Loop over each week
      foreach (KeyValuePair<String, ProductAnalysis> pair in ProductsAnalysis)
      {
        // Create the JSON String
        String json = pair.Value.CreateDropMixBandJSON();
        // Add the string to the list of weeks
        devices.Add(json);
      }
      // Create the name of the data structure
      String name = JSONWriter.CreateKeyValue("name", "Drop Events Mix-Band Usage");
      // Create the children JSON array
      String children = JSONWriter.CreateJSONArray("children", devices);
      // Create and return the final JSON object
      return JSONWriter.CreateJSONObject(name, children);
    }

    /// <summary>
    /// This method will merge all the Fail RAT JSON formatted strings for each 
    /// given device within this object. The method will ultimately return a 
    /// new JSON string which will contain the multi-device analysis.
    /// </summary>
    /// <returns>A well formed JSON string</returns>
    private String MergeFailRATJSON()
    {
      // Stores all the JSON strings for each device
      List<String> devices = new List<String>();
      // Loop over each week
      foreach (KeyValuePair<String, ProductAnalysis> pair in ProductsAnalysis)
      {
        // Create the JSON String
        String json = pair.Value.CreateFailRATJSON();
        // Add the string to the list of weeks
        devices.Add(json);
      }
      // Create the name of the data structure
      String name = JSONWriter.CreateKeyValue("name", "Fail Events RAT Usage");
      // Create the children JSON array
      String children = JSONWriter.CreateJSONArray("children", devices);
      // Create and return the final JSON object
      return JSONWriter.CreateJSONObject(name, children);
    }

    /// <summary>
    /// This method will merge all the Fail MixBand JSON formatted strings for 
    /// each given device within this object. The method will ultimately return
    /// a new JSON string which will contain the multi-device analysis.
    /// </summary>
    /// <returns>A well formed JSON string</returns>
    private String MergeFailMixBandJSON()
    {
      // Stores all the JSON strings for each device
      List<String> devices = new List<String>();
      // Loop over each week
      foreach (KeyValuePair<String, ProductAnalysis> pair in ProductsAnalysis)
      {
        // Create the JSON String
        String json = pair.Value.CreateFailMixBandJSON();
        // Add the string to the list of weeks
        devices.Add(json);
      }
      // Create the name of the data structure
      String name = JSONWriter.CreateKeyValue("name", "Fail Events Mix-Band Usage");
      // Create the children JSON array
      String children = JSONWriter.CreateJSONArray("children", devices);
      // Create and return the final JSON object
      return JSONWriter.CreateJSONObject(name, children);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method will create the analysis object when wanting to analyse the 
    /// events utilising the device name as the main pivot point.
    /// </summary>
    private void CreateProductAnalysis()
    {
      // Get a list of non-noise calls and group by the deivce name
      var results = DBscan.Calls.Where(evt => evt.Coordinate.Noise == false)
                                .GroupBy(evt => evt.Device);
      // Loop over each grouped calls
      foreach (var result in results)
      {
        // Get the key - Device Name
        String key = result.Key;
        // Setup a new list of EventCollections to store all clusters
        List<EventCollection> device = new List<EventCollection>();
        // Start from 1 as 0 reserved for noise
        for (int i = 1; i < DBscan.Clusters.Count; i++)
        {
          // Obtain the cluster from the inital results set
          var cluster = result.Where(evt => evt.Coordinate.ClusterId == i);
          // Add the cluster to the device's worth of clusters
          device.Add(new EventCollection(cluster));
        }
        // Add the week analysis to the known devices
        ProductsAnalysis.Add(key, new ProductAnalysis(device, key));
      }
    }

    #endregion
  }
}
