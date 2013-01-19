using Cluster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;

namespace Analysis
{
  public class WeekAnalysis : Analysis
  {
    /// <summary>
    /// The Week Number associcated with this data set
    /// </summary>
    public int WeekNumber { get; private set; }

    #region Constructor

    /// <summary>
    /// Primary Constructor
    /// </summary>
    /// <param name="events">A list of EventCollections representing clusters</param>
    /// <param name="weekNumber">The week number associated with the events</param>
    public WeekAnalysis(List<EventCollection> events, int weekNumber)
      :base()
    {
      Events = events;
      WeekNumber = weekNumber;
      InitaliseAnalysis();
    }

    #endregion
  }
}
