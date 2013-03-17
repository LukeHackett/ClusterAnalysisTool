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
    {
      Cluster = cluster;
      ClusterID = clusterid;
      DropAnalysis = new DropAnalysis(Cluster);
      FailAnalysis = new FailAnalysis(Cluster);
    }

    #endregion
  }
}
