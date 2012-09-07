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
    public Coordinate Centroid { get; set; }
    
    #endregion
    
    #region Constructors
    
    /// <summary>
    /// Default Constructor
    /// </summary>
    public CoordinateCollection(): base()
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
    
    #endregion    
  }
}
