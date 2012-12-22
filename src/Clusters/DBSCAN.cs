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
  /// This DBSCAN class provides an interface to allow for clustering of a 
  /// collection of events. 
  ///
  /// Basic DBSCAN algorithm:
  ///  1. Label all points as core or noise points.
  ///  2. Eliminate noise points.
  ///  3. Put an edge between all core points that are within Eps of each other.
  ///  4. Make each group of connected core points into a separate cluster.
  ///  5. Assign each border point to one of the clusters of its associated core 
  ///     points.
  /// 
  /// References used:
  /// http://www.c-sharpcorner.com/uploadfile/b942f9/implementing-the-dbscan-algorithm-using-C-Sharp/
  /// </summary>
  public class DBSCAN
  {
    #region Properties

    /// <summary>
    /// Original list of call events
    /// </summary>
    public EventCollection Calls { get; private set; }

    /// <summary>
    /// Original list of coordinates
    /// </summary>
    public CoordinateCollection Coordinates { get; private set; }

    /// <summary>
    /// List of clustered coordinates
    /// </summary>
    public List<CoordinateCollection> ClusteredCoordinates { get; private set; }

    /// <summary>
    /// List of clustered call events
    /// </summary>
    public List<EventCollection> ClusteredEvents { get; private set; }

    /// <summary>
    /// List of noisy coordinates
    /// </summary>
    public CoordinateCollection NoiseCoordinates { get; private set; }

    /// <summary>
    /// List of noisy events
    /// </summary>
    public EventCollection NoiseEvents { get; private set; }

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
    /// <param name="coordinates">A collection of call events</param>
    /// <param name="eps">The epsilon value</param>
    /// <param name="min">The minimum number of coordinates per cluster</param>
    public DBSCAN(EventCollection calls, double eps = 1.5, int min = 3)
    {
      Calls = calls;
      Coordinates = (CoordinateCollection) calls.ToCoordinateList();
      Epsilon = eps;
      MinPoints = min;
    }

    /// <summary>
    /// Secondary Constructor
    /// </summary>
    /// <param name="coordinates">A collection of coordinates</param>
    /// <param name="eps">The epsilon value</param>
    /// <param name="min">The minimum number of coordinates per cluster</param>
    public DBSCAN(CoordinateCollection coordinates, double eps = 1.5, int min = 3)
    {
      Coordinates = coordinates;
      Epsilon = eps;
      MinPoints = min;
    }

    #endregion    
    
    #region Public Methods
    
    /// <summary>
    /// This method is the main entry point for initalising the DBSCAN algorithm. 
    /// The results of the analysis are put into the Clusters List found as a 
    /// member variable within this class. 
    /// Any coordinates that are deduced as noise, are added into the Noise 
    /// Coordinate Collection, found as a member variable within this class.
    /// The method will initially for N number of times, where N is the number 
    /// of coordinates within the Coordinates Collection. The N value does not 
    /// take visiting neighbouring coordinates into consideration, so the number 
    /// of passes will be larger.
    /// </summary>
    public void Analyse()
    {
      // Setup the final data stores
      ClusteredCoordinates = new List<CoordinateCollection>();
      NoiseCoordinates = new CoordinateCollection();
      NoiseCoordinates.Name = "Noisy Clusters";
      NoiseCoordinates.Noise = true;

      // Place each coordinate into a cluster or deduce it as noise
      int clusterId = 1;
      foreach (Coordinate coordinate in Coordinates)
      {

        if (!coordinate.Classified && ExpandCluster(coordinate, clusterId))
        {
          clusterId++;
        }
      }
      
      // Group all of the coordinates as either Noise or as part of it's respected cluster
      foreach (Coordinate coordinate in Coordinates)
      {
        if (coordinate.Noise)
        {
          NoiseCoordinates.Add(coordinate);
        }
        else if (coordinate.Classified)
        {
          // Decrement the Cluster ID by 1 to make use of index 0 of the final list
          int index = coordinate.ClusterId - 1;
          
          // Setup the Cluster if it doesn't already exist (any any previous to this)
          InitaliseClusters(index);

          ClusteredCoordinates[index].Add(coordinate);
        }
      }
      
      // Remove any unrequired (empty) clusters
      CleanClusters();

      // Convert the clustered coordinates to a clustered event collection
      if (Calls != null)
      {
        CreateEventClusters(clusterId);
      }
    }
    
    #endregion
    
    #region Private Methods
    
    /// <summary>
    /// This method will return all coordinates within the given coordinate's 
    /// eps-neighbourhood.
    /// </summary>
    /// <param name="coordinate">The centroid coordinate</param>
    /// <returns>A list of neighbouring coordinates</returns>
    private CoordinateCollection GetRegion(Coordinate coordinate)
    {
      // Collection of neighbouring coordinates
      CoordinateCollection neighbours = new CoordinateCollection();
      // Square the Epsilon (radius) to get the diameter of the neighbourhood
      double eps = Epsilon * Epsilon;
      // Loop over all coordinates
      foreach (Coordinate neighbour in Coordinates)
      {
        // Calculate the distance of the two coordinates
        double distance = Distance.haversine(coordinate, neighbour);
        // Class as a neighbour if it falls in the 'catchment area'
        if (eps >= distance)
        {
          neighbours.Add(neighbour);
        }
      }
      return neighbours;
    }

    /// <summary>
    /// This function will expand each of the given coordinate neighbours, and 
    /// all of their neighbours. This will decide which coordinates are within 
    /// the given coordinate's EPS, and therefore whether or not the neighbours 
    /// belong to the new cluster.
    /// </summary>
    /// <param name="c">The coordinate to expand</param>
    /// <param name="clusterId">The current cluster ID</param>
    /// <returns>whether or not a new cluster has been defined or not</returns>
    private bool ExpandCluster(Coordinate c, int clusterId)
    {
      // Get the all of evt's neighbours.
      CoordinateCollection neighbours = GetRegion(c);
      neighbours.Centroid.Radius = Epsilon;
      
      // Remove itself from the neighbours
      neighbours.Remove(c);

      // Decrement the MinPoints by one so that we take 
      int minimumPoints = MinPoints - 1;

      // Check to see if there is a core point (based on the minimum number of points per cluster)
      if (neighbours.Count < minimumPoints)
      {
        c.Classified = true;
        c.Noise = true;
        return false;
      }
      
      // Assume that all points are reachable from C
      neighbours.UpdateAllClusterID(clusterId);
      
      // Explore all the neighbours
      while(neighbours.Count > 0)
      {
        // Get C's neighbour
        Coordinate currentNeighbour = neighbours[0];
        // Get all the neighbours of the original neighbour
        CoordinateCollection neighbouringNeighbours = GetRegion(currentNeighbour);

        if (neighbouringNeighbours.Count >= minimumPoints)
        {
          // Check to see if 
          foreach (Coordinate resultP in neighbouringNeighbours)
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
      int increment = (index - ClusteredCoordinates.Count) + 1;
      for (int i = 0; i < increment; i++)
      {
        ClusteredCoordinates.Insert(ClusteredCoordinates.Count, new CoordinateCollection());
      }
    }
    
    /// <summary>
    /// This method will remove any elements from the Clusters list, if any 
    /// elements are found to be empty. This will prevent any empty placemarks 
    /// within a KML file if saved to KML.
    /// </summary>
    private void CleanClusters()
    {
      for (int i = 0; i < ClusteredCoordinates.Count; i++)
      {
        if (ClusteredCoordinates[i].Count == 0)
        {
          ClusteredCoordinates.RemoveAt(i);
          i--;
        }
      }
    }

    /// <summary>
    /// This method will create the various event clusters based upon the 
    /// original call input data. If any noise has been found, then these events 
    /// will be added to the noise cluster.
    /// </summary>
    /// <param name="clusterId">the maximum cluster id (including noise)</param>
    private void CreateEventClusters(int clusterId)
    {
      // Setup a new ClusteredEvents list.
      ClusteredEvents = new List<EventCollection>();

      // Loop over each of the known clusters
      for (int n = 0; n < clusterId; n++)
      {
        // Find all the coordinates that are part of the Nth cluster
        var result = Calls.Where(c => c.Coordinate.ClusterId == n);

        // Add to the correct list
        if (n == 0)
        {
          // Noise Events
          NoiseEvents = new EventCollection(result);
        }
        else
        {
          // Clustered Events
          ClusteredEvents.Add(new EventCollection(result));
        }
      }
    }
    
    #endregion
  }
}
