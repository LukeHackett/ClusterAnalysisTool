/// Copyright (c) 2013, Research In Motion Limited.
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
using KML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analysis
{
  public class MultiAnalysis
  {
    #region Properties

    /// <summary>
    /// The multi collection of events
    /// </summary>
    public EventCollection Events { get; protected set; }

    /// <summary>
    /// The DBSCAN object that performs the multi clustering.
    /// </summary>
    protected DBSCAN DBscan;

    #endregion

    #region Constructor

    /// <summary>
    /// Primary Constructor
    /// </summary>
    public MultiAnalysis()
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
    /// <param name="eps">The epsilon value</param>
    /// <param name="min">The minimum number of coordinates per cluster</param>
    public void Cluster(double eps, int min)
    {
      // Cluster the data
      DBscan = new DBSCAN(Events, eps, min);
      DBscan.Analyse();
    }

    /// <summary>
    /// This method will output the analysed KML file to the given location.
    /// </summary>
    /// <param name="file">The output location of the KML file</param>
    public void GenerateKML(String file)
    {
      // Ensure that analysis has been performed
      if (DBscan == null)
      {
        throw new Exception("Input data has not been clustered");
      }

      // Output the KML
      KMLWriter.GenerateKML(DBscan.Clusters, DBscan.Noise, file);
    }

    /// <summary>
    /// This method will output the analysed KMZ file to the given location. 
    /// </summary>
    /// <param name="file">The output location of the KMZ file</param>
    public void GenerateKMZ(String file)
    {
      // Ensure that analysis has been performed
      if (DBscan == null)
      {
        throw new Exception("Input data has not been clustered");
      }

      // Output the KMZ
      KMZWriter.GenerateKMZ(DBscan.Clusters, DBscan.Noise, file);
    }

    #endregion
  }
}
