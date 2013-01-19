using Cluster;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analysis
{
  public class MultiWeekAnalysis
  {
    #region Properties

    /// <summary>
    /// The multi-week collection of events
    /// </summary>
    public EventCollection Events { get; private set; }

    /// <summary>
    /// The DBSCAN object that performs the multi-week clustering.
    /// </summary>
    private DBSCAN DBscan;

    /// <summary>
    /// A list of multi-week analysis objects
    /// </summary>
    public Dictionary<int, WeekAnalysis> WeeklyAnalysis { get; private set; }

    /// <summary>
    /// A list of product analysis objects
    /// </summary>
    public Dictionary<String, ProductAnalysis> ProductsAnalysis { get; private set; }

    #endregion
    
    #region Constructors

    /// <summary>
    /// Primary Constructor
    /// </summary>
    public MultiWeekAnalysis()
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
    public void Add(EventCollection collection)
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
    public void Add(List<EventCollection> collections)
    {
      foreach (EventCollection collection in collections)
      {
        Add(collection);
      }
    }


    /// <summary>
    /// This method will initalise the clustering algorithm, in order to cluster
    /// the data set held within this object.
    /// </summary>
    public void Cluster()
    {
      // Cluster the data
      DBscan = new DBSCAN(Events);
      DBscan.Analyse();
    }

    /// <summary>
    /// This method will analyse the clustered events upon a Multi-Week basis.
    /// </summary>
    public void AnalyseMultiWeek()
    {
      // Ensure the data has been clustered
      if (DBscan == null)
      {
        throw new Exception("Data has not been clustered.");
      }

      // Setup the analysis storage objects
      WeeklyAnalysis = new Dictionary<int, WeekAnalysis>();

      // Split the clustered data into the various weeks
      CreateWeeklyAnalysis();
    }

    /// <summary>
    /// This method will return a WeekAnalysis based upon the given week number.
    /// If a week does not exist, an exception is thrown.
    /// </summary>
    /// <param name="weekNumber">The week number of the analysis</param>
    /// <returns>A week analysis object for the given week</returns>
    public WeekAnalysis AnalyseWeek(int weekNumber)
    {
      // Ensure that the given key exists 
      if (!WeeklyAnalysis.ContainsKey(weekNumber))
      {
        throw new Exception("Key: " + weekNumber + " does not exist.");
      }

      return WeeklyAnalysis[weekNumber];
    }


    public void AnalyseProducts()
    {
      // Ensure the data has been clustered
      if (DBscan == null)
      {
        throw new Exception("Data has not been clustered.");
      }

      // Setup the analysis storage objects
      ProductsAnalysis = new Dictionary<String, ProductAnalysis>();

      // Split the clustered data into the various weeks
      CreateProductAnalysis();
    }

    public ProductAnalysis AnalyseProduct(String device)
    {
      // Ensure that the given key exists 
      if (!ProductsAnalysis.ContainsKey(device))
      {
        throw new Exception("Devce: " + device + " does not exist.");
      }

      return ProductsAnalysis[device];
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method will create the analysis object when wanting to analyse the 
    /// events utilising the week as the main pivot point.
    /// </summary>
    private void CreateWeeklyAnalysis()
    {
      // Get a list of non-noise calls and group by week number
      var results = DBscan.Calls.Where(evt => evt.Coordinate.Noise == false)
                                .GroupBy(evt => evt.GetWeekNumber());
      // Loop over each grouped calls
      foreach (var result in results)
      {
        // Get the key - Week Number
        int key = result.Key;
        // Setup a new list of EventCollections to store all clusters
        List<EventCollection> week = new List<EventCollection>();
        // Start from 1 as 0 reserved for noise
        for(int i = 1; i < DBscan.ClusteredEvents.Count; i++)
        {
          // Obtain the cluster from the inital results set
          var cluster = result.Where(evt => evt.Coordinate.ClusterId == i);
          // Add the cluster to the week's worth of clusters
          week.Add(new EventCollection(cluster));
        }
        // Add the week analysis to the known weeks
        WeeklyAnalysis.Add(key, new WeekAnalysis(week, key));
      }
    }


    private void CreateProductAnalysis()
    {
      // Get a list of non-noise calls and group by device name
      var results = DBscan.Calls.Where(evt => evt.Coordinate.Noise == false)
                                .GroupBy(evt => evt.Device);
      // Loop over each grouped calls
      foreach (var result in results)
      {
        // Get the key - Device Name
        String key = result.Key;
        // Setup a new list of EventCollections to store all clusters
        List<EventCollection> week = new List<EventCollection>();
        // Start from 1 as 0 reserved for noise
        for (int i = 1; i < DBscan.ClusteredEvents.Count; i++)
        {
          // Obtain the cluster from the inital results set
          var cluster = result.Where(evt => evt.Coordinate.ClusterId == i);
          // Add the cluster to the week's worth of clusters
          week.Add(new EventCollection(cluster));
        }
        // Add the week analysis to the known weeks
        ProductsAnalysis.Add(key, new ProductAnalysis(week, key));
      }
    }

    #endregion
  }
}
