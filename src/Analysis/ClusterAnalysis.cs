using Cluster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analysis
{
  public class ClusterAnalysis
  {
    #region Properties

    /// <summary>
    /// The collection of events within the cluster.
    /// </summary>
    public EventCollection Cluster;
    
    /// <summary>
    /// The ID of the cluster.
    /// </summary>
    public int ClusterID { get; private set; }

    /// <summary>
    /// Drop Analysis object to analysis the dropped events in this cluster.
    /// </summary>
    public DropAnalysis DropAnalysis { get; private set; }

    /// <summary>
    /// Fail Analysis object to analysis the failed events in this cluster.
    /// </summary>
    public FailAnalysis FailAnalysis { get; private set; }


    #endregion

    #region Constructor

    /// <summary>
    /// Primary Constructor
    /// </summary>
    /// <param name="cluster">An EventCollection representing a cluster</param>
    /// <param name="clusterid">The ID of the cluster</param>
    public ClusterAnalysis(EventCollection cluster, int clusterid)
      : base()
    {
      Cluster = cluster;
      ClusterID = clusterid;
      DropAnalysis = new DropAnalysis(Cluster);
      FailAnalysis = new FailAnalysis(Cluster);
    }

    #endregion


  }
}
