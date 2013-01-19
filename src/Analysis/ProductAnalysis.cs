using Cluster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analysis
{
  public class ProductAnalysis : Analysis
  {
    #region Constructor
    
    /// <summary>
    /// 
    /// </summary>
    public String Device { get; private set; }

    #endregion


    #region Constructor

    /// <summary>
    /// Primary Constructor
    /// </summary>
    /// <param name="events">A list of EventCollections representing clusters</param>
    /// <param name="weekNumber">The week number associated with the events</param>
    public ProductAnalysis(List<EventCollection> events, String device)
    {
      Events = events;
      Device = device;
      InitaliseAnalysis();
    }

    #endregion
  }
}
