using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
  /// Source:
  /// http://www.c-sharpcorner.com/uploadfile/b942f9/implementing-the-dbscan-algorithm-using-C-Sharp/
  /// </summary>
  public class DBSCAN
  {
    #region Properties

    /// <summary>
    /// List of original coordinates
    /// </summary>
    public CoordinateCollection Coordinates { get; private set; }

    /// <summary>
    /// List of clustered coordinates
    /// </summary>
    public List<CoordinateCollection> Clusters { get; private set; }

    /// <summary>
    /// List of noisy clusters
    /// </summary>
    public CoordinateCollection Noise { get; private set; }

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
    public DBSCAN(CoordinateCollection coordinates, double eps, int min)
    {
      Coordinates = coordinates;
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
      Clusters = new List<CoordinateCollection>();
      Noise = new CoordinateCollection();

      // Place each coordinate into a cluster or deduce it as noise
      int clusterId = 1;
      foreach (Coordinate c in Coordinates)
      {
        if (!c.Classified && ExpandCluster(c, clusterId))
        {
          clusterId++;
        }
      }
      
      // Group all of the coordinates as either Noise or as part of it's respected cluster
      foreach (Coordinate c in Coordinates)
      {
        if (c.Noise)
        {
          Noise.Add(c);
        }
        else if (c.Classified)
        {
          // Decrement the Cluster ID by 1 to make use of index 0 of the final list
          int index = c.ClusterId - 1;
          
          // Setup the Cluster if it doesn't already exist          
          if (Clusters.Count <= index)
          {
            Clusters.Insert(index, new CoordinateCollection());
          }
          Clusters[index].Add(c);
        }
      }
    }
    
    #endregion
    
    #region Private Methods
    
    /// <summary>
    /// This method will return all coordinates within C's eps-neighbourhood.
    /// </summary>
    /// <param name="c">The centroid coordinate</param>
    /// <returns>A list of neighbouring coordinates</returns>
    private CoordinateCollection GetRegion(Coordinate c)
    {
      // Collection of neighbouring coordinates
      CoordinateCollection neighbours = new CoordinateCollection();
      // Square the Epsilon (radius) to get the diameter of the neighbourhood
      double eps = Epsilon * Epsilon;
      // Loop over all coordinates
      foreach (Coordinate neighbour in Coordinates)
      {
        // Calculate the distance of the two coordinates
        double distance = Distance.DistanceSquared(c, neighbour);
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
    private bool ExpandCluster(Coordinate c, int clusterId)
    {
      // Get the all of C's neighbours.
      CoordinateCollection neighbours = GetRegion(c);
      // Check to see if there is a core point (based on the minimum number of points per cluster)
      if (neighbours.Count < MinPoints)
      {
        c.Classified = true;
        c.Noise = true;
        return false;
      }
      
      // Assume that all points are reachable from C
      neighbours.UpdateAllClusterID(clusterId);
      neighbours.Remove(c);
      
      // Explore all the neighbours
      while(neighbours.Count > 0)
      {
        // Get C's neighbour
        Coordinate currentNeighbour = neighbours[0];
        // Get all the neighbours of the original neighbour
        CoordinateCollection neighbouringNeighbours = GetRegion(currentNeighbour);
        
        if (neighbouringNeighbours.Count >= MinPoints)
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
    
    #endregion
  }
}
