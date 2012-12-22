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
  public class CoordinateCollection : List<Coordinate>
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
    /// Whether or not this collection is classed as noise
    /// </summary>
    public Boolean Noise { get; set; }

    /// <summary>
    /// The centroid of this cluster
    /// </summary>
    public Centroid Centroid { get; private set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default Constructor
    /// </summary>
    public CoordinateCollection()
      : base()
    {
      Centroid = new Centroid(0, 0);
    }

    /// <summary>
    /// Secondary Constructor
    /// </summary>
    /// <param name="collection">A collection of elements to be added</param>
    public CoordinateCollection(IEnumerable<Coordinate> collection)
      : base(collection)
    {
      Centroid = new Centroid(0, 0);
      UpdateCentroid();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Adds a coordinate to the end of this coordinate collection
    /// </summary>
    /// <param name="coordinate">Coordinate to be added</param>
    public new void Add(Coordinate coordinate)
    {
      base.Add(coordinate);
      UpdateCentroid();
    }

    /// <summary>
    /// Adds a collection of coordinates to the end of this coordinate collection.
    /// </summary>
    /// <param name="collection">a collection of coordinate to be added</param>
    public void AddRange(CoordinateCollection collection)
    {
      base.AddRange(collection);
      UpdateCentroid();
    }

    /// <summary>
    /// Removes the first occurrence of the given coordinate.
    /// </summary>
    /// <param name="coordinate">Coordinate to be removed</param>
    public new void Remove(Coordinate coordinate)
    {
      base.Remove(coordinate);
      UpdateCentroid();
    }

    /// <summary>
    /// Removes the coordinate at the specified index.
    /// </summary>
    /// <param name="index">coordinate's index location</param>
    public new void RemoveAt(int index)
    {
      base.RemoveAt(index);
      UpdateCentroid();
    }

    /// <summary>
    /// This method will calculate the centroid's Longitude and Latitude based 
    /// upon all of the coordinates within this collection.
    /// </summary>
    public void UpdateCentroid()
    {
      // Update the Longitude and Latitude Values
      UpdateCentroidLatLon();

      // Update the radial distance
      UpdateCentroidRadius();
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
    /// This method will deep clone the current object instance.
    /// </summary>
    /// <returns>A CoordinateCollection object</returns>
    public Object Clone()
    {
      // Create a new list to store the clone in
      CoordinateCollection collection = new CoordinateCollection();

      // Copy over each coordinate in the list
      foreach (Coordinate c in this)
      {
        collection.Add(c);
      }

      // Copy over the meta-data
      collection.Name = this.Name;
      collection.Description = this.Description;
      collection.Noise = this.Noise;
      collection.Centroid = this.Centroid;

      // Return the newly cloned coordinate collection
      return collection;
    }

    /// <summary>
    /// This method will split the current Collection into a list of section 
    /// number of elements. Each element is a Call Log Collection, and each 
    /// collection will have an equal number of Call logs.
    /// </summary>
    /// <param name="sections">The number of call logs the lists should be split into</param>
    /// <returns>A list of CallCollections</returns>
    /// <exception cref="System.ArgumentException">Thrown when parameter is less than 1</exception>
    public List<CoordinateCollection> Split(int sections)
    {
      // Validate Input
      if (sections <= 0)
      {
        throw new ArgumentException("Cannot split a list by anything less than 1.");
      }

      // Calculate the number of elements per each section
      int noElements = (int)Math.Ceiling((double)this.Count / (double)sections);

      // Make a copy of the current list and Generate a new list
      List<Coordinate> currentList = (CoordinateCollection) Clone();
      List<List<Coordinate>> splitList = new List<List<Coordinate>>();
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
      List<CoordinateCollection> clusters = new List<CoordinateCollection>();
      foreach (List<Coordinate> list in splitList)
      {
        CoordinateCollection cluster = new CoordinateCollection();
        cluster.AddRange(list);

        // Update the centroid
        cluster.UpdateCentroid();

        // Add this list to the outer list
        clusters.Add(cluster);
      }
      return clusters;
    }
    
    /// <summary>
    /// This method will append a collection of coordinates to this collection 
    /// of coordinates.
    /// </summary>
    /// <param name="collection">The collection to appended to the end of this collection.</param>
    public void Append(CoordinateCollection collection)
    {
      foreach (Coordinate coordinate in collection)
      {
        Add(coordinate);
      }

      // Update all the centroids
      UpdateCentroid();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method will calculate the Centroid of all types of calls. The 
    /// average values are obtained by calculating the mean Latitude and 
    /// Longitude of all calls in the collection.
    /// </summary>
    private void UpdateCentroidLatLon()
    {
      // Create LINQ statements to sup up all the drop Latitude values
      var latitudes = (from call in this
                       select call.Latitude);
      
      // Create LINQ statements to sup up all the drop Longitude values
      var longitudes = (from call in this
                        select call.Longitude);

      // Calculate the average Latitude value
      double latitude = latitudes.Sum() / latitudes.Count();
      Centroid.Latitude = Double.IsNaN(latitude) ? 0 : latitude;

      // Calculate the average Longitude value
      double longitude = longitudes.Sum() / longitudes.Count();
      Centroid.Longitude = Double.IsNaN(longitude) ? 0 : longitude;
    }

    /// <summary>
    /// This method will update the centroid's radius value, which will be updated
    /// to the distance between the centroid and the furthest event in this 
    /// collection.
    /// </summary>
    private void UpdateCentroidRadius()
    {
      // Loop over the collection
      foreach (Coordinate coordinate in this)
      {
        // Calculate the distance
        double distance = Distance.haversine(coordinate, Centroid);
        // Update the distance if a larger one has been found.
        if (distance > Centroid.Radius)
        {
          Centroid.Radius = distance;
        }
      }
    }

    #endregion
  }
  
}
