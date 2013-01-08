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
  /// This K-Means class provides an interface to allow for a simple clustering 
  /// of a collection of events. The class will estimate a K value (number of 
  /// clusters desired), if a K value ia unknown however this is an estimate, 
  /// and suppling a K value is recommended. 
  /// Each of the coordinates will be assigned to a cluster, based upon the 
  /// distance from the centre of the cluster (centroid) to the given coordinate. 
  /// The pseudo K-Means algorithm is shown below for reference purposes:
  ///  
  /// Basic K-means algorithm:
  ///   1. Select K points as initial centroids.
  ///   2. repeat
  ///   3. Form K clusters by assigning each point to its closest centroid. 
  ///   4: Recompute the centroid of each cluster.
  ///   5: until Centroids do not change.
  ///   
  /// References used: 
  /// http://codeding.com/?article=14
  /// </summary>
  public class KMeans
  {
    #region Properties
    
    /// <summary>
    /// Collection of original calls
    /// </summary>
    public EventCollection Calls { get; private set; }

    /// <summary>
    /// Collection of coordinates
    /// </summary>
    public CoordinateCollection Coordinates { get; private set; }

    /// <summary>
    /// List of clustered events
    /// </summary>
    public List<EventCollection> ClusteredEvents { get; private set; }

    /// <summary>
    /// List of clustered coordinates
    /// </summary>
    public List<CoordinateCollection> ClusteredCoordinates { get; private set; }
        
    /// <summary>
    /// The number of Clusters to be used (K Value)
    /// </summary>
    public int K { get; private set; }
    
    #endregion
    
    #region Constructors
    
    /// <summary>
    /// Primary Constructor
    /// </summary>
    /// <param name="events">The events to be clustered</param>
    /// <param name="k">The number of clusters</param>
    public KMeans(EventCollection events, int k = 0)
    {
      Calls = events;
      Coordinates = events.ToCoordinateCollection();
      K = (k > 0) ? k : CalcKValue();
    }

    /// <summary>
    /// Secondary Constructor
    /// </summary>
    /// <param name="coordinates">The coordinates to be clustered</param>
    /// <param name="k">The number of clusters</param>
    public KMeans(CoordinateCollection coordinates, int k = 0)
    {
      Coordinates = coordinates;
      K = (k > 0) ? k : CalcKValue();
    }
       
    #endregion
    
    #region Public Methods
    
    /// <summary>
    /// This method is the main entry point for initialising the K-Means algorithm.
    /// The results of the analysis are put into the Clusters List found as a 
    /// member variable within this class.
    /// The method will continue running until there have been no detected 
    /// changes. A change is defined as at least one event moving from one cluster
    /// to another.
    /// </summary>
    public void Analyse()
    {
      // Divide the Coordinates in K Clusters
      //ClusteredEvents = Calls.Split(K);
      ClusteredCoordinates = Coordinates.Split(K);

      // Number of changes made within a pass
      int changes = 0;

      // Continuously recompute, until no changes were made.
      do
      {
        // Reset the number of changes made
        changes = 0;

        // Loop over all Clusters
        foreach (CoordinateCollection cluster in ClusteredCoordinates)
        {
          // Loop over each Coordinate
          for (int i = 0; i < cluster.Count; i++)
          {
            // Get the coordinate
            Coordinate coordinate = cluster[i];
            
            // Find the nearest cluster's index
            int nearest = FindNearestCluster(ClusteredCoordinates, coordinate);
            
            // Check to see if the point has moved and is not empty
            if (nearest != ClusteredCoordinates.IndexOf(cluster) && cluster.Count > 1)
            {
              // Remove the event from the current location
              cluster.Remove(coordinate);

              // Insert previously removed event into the correct cluster
              coordinate.ClusterId = nearest;
              ClusteredCoordinates[nearest].Add(coordinate);

              // A change has happened
              changes++;
            }
          }
        }
      } 
      while (changes > 0);

      // Convert the clustered coordinates to events
      if (Calls != null)
      {
        CreateEventClusters();
      }
    }
        
    #endregion

    #region Private Methods

    /// <summary>
    /// This method will find a given event's closest cluster. The distance between 
    /// the coordinate and the cluster's centroid is calculated using the haversine 
    /// formula.
    /// </summary>
    /// <see cref="Distance.haversine"/>
    /// <param name="clusters">a list of clusters</param>
    /// <param name="evt">the pivot event</param>
    /// <returns>index location of the cluster</returns>
    private int FindNearestCluster(List<CoordinateCollection> clusters, Coordinate coordinate)
    {
      // The minimum distance to a coordinate
      double minimumDistance = Double.MaxValue;

      // The index of the nearest cluster
      int clusterIndex = -1;
      
      // Find the nearest cluster
      for (int k = 0; k < clusters.Count; k++)
      {
        // Calculate the distance
        double distance = Distance.haversine(coordinate, clusters[k].Centroid);
        
        // Compare the two distance values
        if (minimumDistance > distance)
        {
          // A smaller distance has been found
          minimumDistance = distance;
          clusterIndex = k;
        }
      }
      return clusterIndex;
    }
        
    /// <summary>
    /// This method will calculate the K value based upon the number of objects 
    /// within the data set.
    /// </summary>
    /// <returns>The K value</returns>
    private int CalcKValue()
    {
      return (int) Math.Ceiling( Math.Sqrt(Calls.Count / 2) );
    }

    /// <summary>
    /// This method will create the various event clusters based upon the 
    /// original call input data.
    /// </summary>
    private void CreateEventClusters()
    {
      // Setup a new ClusteredEvents list.
      ClusteredEvents = new List<EventCollection>();
      
      // Loop over each cluster
      for (int n = 0; n < K; n++)
      {
        // Find all the coordinates that are part of the Nth cluster
        var result = Calls.Where(c => c.Coordinate.ClusterId == n);

        // Clustered Events
        ClusteredEvents.Add(new EventCollection(result));
      }
    }

    #endregion Private Methods
  }
}
