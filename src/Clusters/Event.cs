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
  public abstract class Event
  {
    #region Properties

    /// <summary>
    /// The device name.
    /// </summary>
    public String Device { get; set; }
    
    /// <summary>
    /// The device PIN.
    /// </summary>
    public String Pin { get; set; }

    /// <summary>
    /// Whether or not the given device is a reference device
    /// </summary>
    public Boolean Reference { get; set; }
    
    /// <summary>
    /// The start RAT of the device.
    /// </summary>
    public String StartRat { get; set; }
    
    /// <summary>
    /// The end RAT of the device.
    /// </summary>
    public String EndRat { get; set; }
    
    /// <summary>
    /// The start frequency band of the device.
    /// </summary>
    public String StartMixBand { get; set; }
    
    /// <summary>
    /// The end frequency band of the device.
    /// </summary>
    public String EndMixBand { get; set; }

    /// <summary>
    /// Latitude Value (North-South Position) of the event.
    /// </summary>
    public Coordinate Coordinate { get; set; }

    /// <summary>
    /// The cluster that this event belongs to.
    /// </summary>
    public int ClusterId { get; set; }

    /// <summary>
    /// Whether or not this event is classed as noise.
    /// </summary>
    public bool Noise { get; set; }

    /// <summary>
    /// Whether or not this event has been classified (or visited).
    /// </summary>
    public bool Classified { get; set; }

    #endregion

    #region Public Methods

    public Event()
    {

    }

    #endregion

  }
}
