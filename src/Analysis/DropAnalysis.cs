using Cluster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analysis
{
  public class DropAnalysis : EventAnalysis
  {
    #region Constructor 

    /// <summary>
    /// Primary Constructor
    /// </summary>
    /// <param name="cluster">EventCollection of dropped events</param>
    public DropAnalysis(EventCollection cluster)
    {
      Cluster = new EventCollection(cluster.GetEvents("Drop"));
    }

    #endregion
  }
}