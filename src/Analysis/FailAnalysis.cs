using Cluster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analysis
{
  public class FailAnalysis : EventAnalysis
  {
    #region Constructor

    /// <summary>
    /// Primary Constructor
    /// </summary>
    /// <param name="cluster">EventCollection of failed events</param>
    public FailAnalysis(EventCollection cluster)
    {
      Cluster = new EventCollection(cluster.GetEvents("Fail"));
    }

    #endregion
  }
}
