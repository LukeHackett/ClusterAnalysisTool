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
  /// <summary>
  /// This class represents a list of Coordinates and extends the List class. Each coordinate can be 
  /// accessed by index. This class will automatically calculate the centroid coordinate, and will 
  /// automatically recalculate only when adding, or removing coordinates.
  /// </summary>
  public class CoordinateCollection : List<Coordinate>
  {
    #region Properties
    
    /// <summary>
    /// The central coordinate of the Collection
    /// </summary>
    public Coordinate Centroid { get; private set; }
    
    /// <summary>
    /// The name of this collection of coordinates
    /// </summary>
    public String Name { get; set; }
    
    /// <summary>
    /// A longer description of the coordinates in this collection
    /// </summary>
    public String Description { get; set; }
    
    #endregion
    
    #region Constructors
    
    /// <summary>
    /// Default Constructor
    /// </summary>
    public CoordinateCollection()
      : base()
    {   
        Centroid = new Coordinate(0, 0);
    }

    #endregion
    
    #region Public Methods
    
    /// <summary>
    /// Adds a coordinate to the end of the coordinate list
    /// </summary>
    /// <param name="c">Coordinate to be added</param>
    public new void Add(Coordinate c)
    {
      base.Add(c);
      UpdateCentroid();
    }
    
    /// <summary>
    /// Removes the first occurrence of the given coordinate.
    /// </summary>
    /// <param name="c">Coordinate to be removed</param>
    public new void Remove(Coordinate c)
    { 
      base.Remove(c);
      UpdateCentroid();
    }
    
    /// <summary>
    /// Removes the element at the specified index.
    /// </summary>
    /// <param name="index">element's index location</param>
    public new void RemoveAt(int index)
    {
      base.RemoveAt(index);
      UpdateCentroid();
    }
       
    /// <summary>
    /// This method will update the Centroid's Latitude and Longitude values. These values are 
    /// obtained by calculating the mean Latitude and Longitude.
    /// </summary>
    public void UpdateCentroid()
    {
      // Create LINQ statements to sum up all the Lat/Long values
      double latitudeTotal = (from coordinate in this select coordinate.Latitude).Sum();
      double longitudeTotal = (from coordinate in this select coordinate.Longitude).Sum();

      // Set the Centroid's Lat/Long values to be the average of it's members
      Centroid.Latitude = (latitudeTotal / (double)this.Count);
      Centroid.Longitude = (longitudeTotal / (double)this.Count);
    }
    
    /// <summary>
    /// This method will set all coordinate IDs within this Collection to the given value
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
    /// This method will split the current Collection into a list of section number of elements. 
    /// Each element is a Coordinate Collection, and each collection will have an equal number of 
    /// coordinates.
    /// </summary>
    /// <param name="sections">The number of clusters the lists should be split into</param>
    /// <returns>A list of CoorindateCollections</returns>
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
      List<Coordinate> currentList = this;
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
        clusters.Add(cluster);
      }
      return clusters;
    }


    /// <summary>
    /// This method will append a collection of coordinates to this collection of coorindates.
    /// </summary>
    /// <param name="collection">The collection to appended to the end of this collection.</param>
    public void Append(CoordinateCollection collection)
    {
      foreach (Coordinate c in collection)
      {
        Add(c);
      }
    }
    
    #endregion    
  }
}
