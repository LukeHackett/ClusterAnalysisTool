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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cluster
{
  public class EventCollection : List<Event>
  {
    #region Properties

    /// <summary>
    /// The name of this collection of coordinates
    /// </summary>
    public String Name { get; set; }

    /// <summary>
    /// A longer description of the coordinates in this collection
    /// </summary>
    public String Description { get; set; }

    /// <summary>
    /// The central coordinate of all types of calls
    /// </summary>
    public Coordinate Centroid { get; private set; }

    /// <summary>
    /// The central coordinate of the dropped calls
    /// </summary>
    public Coordinate DropCentroid { get; private set; }

    /// <summary>
    /// The central coordinate of the failed calls
    /// </summary>
    public Coordinate FailCentroid { get; private set; }

    /// <summary>
    /// The central coordinate of the successful calls
    /// </summary>
    public Coordinate SuccessCentroid { get; private set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default Constructor
    /// </summary>
    public EventCollection()
      : base()
    {
      Centroid = new Coordinate(0, 0);
      DropCentroid = new Coordinate(0, 0);
      FailCentroid = new Coordinate(0, 0);
      SuccessCentroid = new Coordinate(0, 0);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Adds a event to the end of this event collection
    /// </summary>
    /// <param name="evt">Event to be added</param>
    public new void Add(Event evt)
    {
      base.Add(evt);
      UpdateAllCentroids();
    }
    
    /// <summary>
    /// Adds a collection events to the end of this event collection.
    /// </summary>
    /// <param name="collection">a collection of events to be added</param>
    public void AddRange(EventCollection collection)
    {
      foreach (Event evt in collection)
      {
        Add(evt);
      }
      UpdateAllCentroids();
    }

    /// <summary>
    /// Removes the first occurrence of the given event.
    /// </summary>
    /// <param name="evt">Event to be removed</param>
    public new void Remove(Event evt)
    {
      base.Remove(evt);
      UpdateAllCentroids();
    }

    /// <summary>
    /// Removes the event at the specified index.
    /// </summary>
    /// <param name="index">event's index location</param>
    public new void RemoveAt(int index)
    {
      base.RemoveAt(index);
      UpdateAllCentroids();
    }

    /// <summary>
    /// This method will calculate the Centroid of all the dropped calls within 
    /// this set. The average values are obtained by calculating the mean 
    /// Latitude and Longitude of every dropped call.
    /// </summary>
    public void UpdateDropCentroid() 
    {
      // Create LINQ statements to sup up all the drop Latitude values
      var latitudes = ( from call in this
                          where call is Drop
                          select call.Coordinate.Latitude );

      // Create LINQ statements to sup up all the drop Longitude values
      var longitudes = ( from call in this
                           where call is Drop
                           select call.Coordinate.Longitude );

      // Calculate the average Latitude value
      double latitude = latitudes.Sum() / latitudes.Count();
      DropCentroid.Latitude = Double.IsNaN(latitude) ? 0 : latitude;

      // Calculate the average Longitude value
      double longitude = longitudes.Sum() / longitudes.Count();
      DropCentroid.Longitude = Double.IsNaN(longitude) ? 0 : longitude;
    }

    /// <summary>
    /// This method will calculate the Centroid of all the failed calls within 
    /// this set. The average values are obtained by calculating the mean 
    /// Latitude and Longitude of every failed call.
    /// </summary>
    public void UpdateFailCentroid()
    {
      // Create LINQ statements to sup up all the drop Latitude values
      var latitudes = (from call in this
                      where call is Fail
                      select call.Coordinate.Latitude);

      // Create LINQ statements to sup up all the drop Longitude values
      var longitudes = (from call in this
                       where call is Fail
                       select call.Coordinate.Longitude);

      // Calculate the average Latitude value
      double latitude = latitudes.Sum() / latitudes.Count();
      FailCentroid.Latitude = Double.IsNaN(latitude) ? 0 : latitude;

      // Calculate the average Longitude value
      double longitude = longitudes.Sum() / longitudes.Count();
      FailCentroid.Longitude = Double.IsNaN(longitude) ? 0 : longitude;
    }

    /// <summary>
    /// This method will calculate the Centroid of all the successful calls 
    /// within this set. The average values are obtained by calculating the mean 
    /// Latitude and Longitude of every successful call.
    /// </summary>
    public void UpdateSuccessCentroid()
    {
      // Create LINQ statements to sup up all the drop Latitude values
      var latitudes = (from call in this
                      where call is Success
                      select call.Coordinate.Latitude);

      // Create LINQ statements to sup up all the drop Longitude values
      var longitudes = (from call in this
                       where call is Success
                       select call.Coordinate.Longitude);

      // Calculate the average Latitude value
      double latitude = latitudes.Sum() / latitudes.Count();
      SuccessCentroid.Latitude = Double.IsNaN(latitude) ? 0 : latitude;

      // Calculate the average Longitude value
      double longitude = longitudes.Sum() / longitudes.Count();
      SuccessCentroid.Longitude = Double.IsNaN(longitude) ? 0 : longitude;
    }

    /// <summary>
    /// This method will calculate the Centroid of all types of calls (drops, 
    /// fails and successful). The average values are obtained by calculating 
    /// the mean Latitude and Longitude of every failed call.
    /// </summary>
    public void UpdateCentroid() 
    {
      // Create LINQ statements to sup up all the drop Latitude values
      var latitudes = (from call in this
                      select call.Coordinate.Latitude);

      // Create LINQ statements to sup up all the drop Longitude values
      var longitudes = (from call in this
                        select call.Coordinate.Longitude);

      // Calculate the average Latitude value
      double latitude = latitudes.Sum() / latitudes.Count();
      Centroid.Latitude = Double.IsNaN(latitude) ? 0 : latitude;

      // Calculate the average Longitude value
      double longitude = longitudes.Sum() / longitudes.Count();
      Centroid.Longitude = Double.IsNaN(longitude) ? 0 : longitude;
    }

    /// <summary>
    /// This method will calculate the Centroid of all types of calls (drops, 
    /// fails and successful) indivudally, and then will calculate the average 
    /// of those. The average values are obtained by calculating the mean 
    /// Latitude and Longitude of every failed call.
    /// </summary>
    public void UpdateAllCentroids()
    {
      UpdateDropCentroid();
      UpdateFailCentroid();
      UpdateSuccessCentroid();
      UpdateCentroid();
    }

    /// <summary>
    /// This method will set all coordinate IDs within this Collection to the 
    /// given value.
    /// </summary>
    /// <param name="clusterID">the new cluster ID</param>
    public void UpdateAllClusterID(int clusterID)
    {
      for (int i = 0; i < this.Count; i++)
      {
        this[i].ClusterId = clusterID;
        this[i].Noise = false;
      }
    }

    /// <summary>
    /// This method will split the current Collection into a list of section 
    /// number of elements. Each element is a Call Log Collection, and each 
    /// collection will have an equal number of Call logs.
    /// </summary>
    /// <param name="sections">The number of call logs the lists should be split into</param>
    /// <returns>A list of CallCollections</returns>
    /// <exception cref="System.ArgumentException">Thrown when parameter is less than 1</exception>
    public List<EventCollection> Split(int sections)
    {
      // Validate Input
      if (sections <= 0)
      {
        throw new ArgumentException("Cannot split a list by anything less than 1.");
      }

      // Calculate the number of elements per each section
      int noElements = (int)Math.Ceiling((double)this.Count / (double)sections);

      // Make a copy of the current list and Generate a new list
      List<Event> currentList = this;
      List<List<Event>> splitList = new List<List<Event>>();
      //Loop until there is no more
      while (currentList.Count > 0)
      {
        // Determine how many elements should be obtained
        int count = currentList.Count > noElements ? noElements : currentList.Count;
        // Add the range to the new list, and remove from the 
        splitList.Add(currentList.GetRange(0, count));
        currentList.RemoveRange(0, count);
      }

      // Loop over the split collection and copy each element into a CoordinateCollection
      List<EventCollection> clusters = new List<EventCollection>();
      foreach (List<Event> list in splitList)
      {
        EventCollection cluster = new EventCollection();
        cluster.AddRange(list);
        // Update all the centroids
        cluster.UpdateAllCentroids();
        // Add this list to the outer list
        clusters.Add(cluster);
      }
      return clusters;
    }


    /// <summary>
    /// This method will append a collection of call logs to this collection 
    /// of call logs.
    /// </summary>
    /// <param name="collection">The collection to appended to the end of this collection.</param>
    public void Append(EventCollection collection)
    {
      foreach (Event call in collection)
      {
        Add(call);
      }

      // Update all the centroids
      UpdateAllCentroids();
    }

    #endregion    
  }
}
