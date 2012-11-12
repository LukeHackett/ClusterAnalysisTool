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
using CallEvent;

namespace Cluster
{
  /// <summary>
  /// This DBSCAN class provides an interface to allow for clustering of a collection of coordinates. 
  ///
  /// Basic DBSCAN algorithm:
  ///  1. Label all points as core or noise points.
  ///  2. Eliminate noise points.
  ///  3. Put an edge between all core points that are within Eps of each other.
  ///  4. Make each group of connected core points into a separate cluster.
  ///  5. Assign each border point to one of the clusters of its associated core points.
  /// 
  /// References used:
  /// http://www.c-sharpcorner.com/uploadfile/b942f9/implementing-the-dbscan-algorithm-using-C-Sharp/
  /// </summary>
  public class DBSCAN
  {
    #region Properties

    /// <summary>
    /// List of original coordinates
    /// </summary>
    public EventCollection Calls { get; private set; }

    /// <summary>
    /// List of clustered coordinates
    /// </summary>
    public List<EventCollection> Clusters { get; private set; }

    /// <summary>
    /// List of noisy clusters
    /// </summary>
    public EventCollection Noise { get; private set; }

    /// <summary>
    /// The Epsilon Value
    /// </summary>
    public double Epsilon { get; private set; }

    /// <summary>
    /// The minimum number of coordinates required to form a cluster 
    /// </summary>
    public int MinPoints { get; private set; }
    
    #endregion
    
    #region Constructor

    /// <summary>
    /// Primary Constructor
    /// </summary>
    /// <param name="coordinates">A Collection of coordinates</param>
    /// <param name="eps">The epsilon value</param>
    /// <param name="min">The minimum number of coordinates per cluster</param>
    public DBSCAN(EventCollection calls, double eps, int min)
    {
      Calls = calls;
      Epsilon = eps;
      MinPoints = min;
    }
    
    #endregion    
    
    #region Public Methods
    
    /// <summary>
    /// This method is the main entry point for initalising the DBSCAN algorithm. The results of 
    /// the analysis are put into the Clusters List found as a member variable within this class. 
    /// Any coordinates that are deduced as noise, are added into the Noise Coordinate Collection, 
    /// found as a member variable within this class.
    /// The method will initially for N number of times, where N is the number of coordinates within 
    /// the Coordinates Collection. The N value does not take visiting neighbouring coordinates into 
    /// consideration, so the number of passes will be larger.
    /// </summary>
    public void Analyse()
    {
      // Setup the final data stores
      Clusters = new List<EventCollection>();
      Noise = new EventCollection();
      Noise.Name = "Noisy Clusters";

      // Place each coordinate into a cluster or deduce it as noise
      int clusterId = 1;
      foreach (Event evt in Calls)
      {
        
        if (!evt.Classified && ExpandCluster(evt, clusterId))
        {
          clusterId++;
        }
      }
      
      // Group all of the coordinates as either Noise or as part of it's respected cluster
      foreach (Event evt in Calls)
      {
        if (evt.Noise)
        {
          Noise.Add(evt);
        }
        else if (evt.Classified)
        {
          // Decrement the Cluster ID by 1 to make use of index 0 of the final list
          int index = evt.ClusterId - 1;
          
          // Setup the Cluster if it doesn't already exist (any any previous to this)
          InitaliseClusters(index);

          Clusters[index].Add(evt);
        }
      }
      
      // Remove any unrequired (empty) clusters
      CleanClusters();
    }
    
    #endregion
    
    #region Private Methods
    
    /// <summary>
    /// This method will return all coordinates within C's eps-neighbourhood.
    /// </summary>
    /// <param name="c">The centroid coordinate</param>
    /// <returns>A list of neighbouring coordinates</returns>
    private EventCollection GetRegion(Event evt)
    {
      // Collection of neighbouring coordinates
      EventCollection neighbours = new EventCollection();
      // Square the Epsilon (radius) to get the diameter of the neighbourhood
      double eps = Epsilon * Epsilon;
      // Loop over all coordinates
      foreach (Event neighbour in Calls)
      {
        // Calculate the distance of the two coordinates
        double distance = Distance.haversine(evt.Coordinate, neighbour.Coordinate);
        // Class as a neighbour if it falls in the 'catchment area'
        if (eps >= distance)
        {
          neighbours.Add(neighbour);
        }
      }
      return neighbours;
    }

    /// <summary>
    /// This function will expand each of the given coordinate neighbours, and all of their 
    /// neighbours. This will decide which coordinates are within the given coordinate's EPS, and 
    /// therefore whether or not the neighbours belong to the new cluster.
    /// </summary>
    /// <param name="c">The coordinate to expand</param>
    /// <param name="clusterId">The current cluster ID</param>
    /// <returns>whether or not a new cluster has been defined or not</returns>
    private bool ExpandCluster(Event evt, int clusterId)
    {
      // Get the all of C's neighbours.
      EventCollection neighbours = GetRegion(evt);
      
      // Remove itself from the neighbours
      neighbours.Remove(evt);
      
      // Check to see if there is a core point (based on the minimum number of points per cluster)
      if (neighbours.Count < MinPoints)
      {
        evt.Classified = true;
        evt.Noise = true;
        return false;
      }
      
      // Assume that all points are reachable from C
      neighbours.UpdateAllClusterID(clusterId);
      
      // Explore all the neighbours
      while(neighbours.Count > 0)
      {
        // Get C's neighbour
        Event currentNeighbour = neighbours[0];
        // Get all the neighbours of the original neighbour
        EventCollection neighbouringNeighbours = GetRegion(currentNeighbour);
        
        if (neighbouringNeighbours.Count >= MinPoints)
        {
          // Check to see if 
          foreach (Event resultP in neighbouringNeighbours)
          {
            if (!resultP.Classified || resultP.Noise)
            {
              if (!resultP.Classified)
              {
                resultP.Classified = true;
                resultP.Noise = false;
                neighbours.Add(resultP);
              }
              resultP.ClusterId = clusterId;
            }
          }
        }
        // Remove C's direct neighbour as it has been explored.
        neighbours.Remove(currentNeighbour);
      }
      return true;   
    }
    
    /// <summary>
    /// This function will initalise the cluster list to index number of locations.
    /// </summary>
    /// <param name="index">the number of elements to initalise</param>
    private void InitaliseClusters(int index)
    {
      int increment = (index - Clusters.Count) + 1;
      for (int i = 0; i < increment; i++)
      { 
        Clusters.Insert(Clusters.Count, new EventCollection());
      }
    }
    
    /// <summary>
    /// This method will remove any elements from the Clusters list, if any elements are found to be
    /// empty. This will prevent any empty placemarks within a KML file if saved to KML.
    /// </summary>
    private void CleanClusters()
    {
      for (int i = 0; i < Clusters.Count; i++)
      {
        if (Clusters[i].Count == 0)
        {
          Clusters.RemoveAt(i);
          i--;
        }
      }
    }
    
    #endregion
  }
}
