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
using System.Globalization;

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
    /// The timestamp of the event
    /// </summary>
    public DateTime Timestamp { get; set; }

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
    /// The state rrc state of the device
    /// </summary>
    public String StartRRCState { get; set; }

    /// <summary>
    /// Latitude Value (North-South Position) of the event.
    /// </summary>
    public Coordinate Coordinate { get; set; }

    #endregion

    #region Public Methods

    /// <summary>
    /// This method will return the week number based upon the timestamp.
    /// </summary>
    /// <param name="StartDay">The start day of a week</param>
    /// <returns>Week number as an integer</returns>
    public int GetWeekNumber(DayOfWeek StartDay = DayOfWeek.Sunday)
    {
      Calendar cal = DateTimeFormatInfo.CurrentInfo.Calendar;
      return cal.GetWeekOfYear(Timestamp, CalendarWeekRule.FirstFullWeek, StartDay);
    }

    /// <summary>
    /// This method returns the type of event as a String.
    /// </summary>
    /// <returns>Object type as a String</returns>
    public new String ToString()
    {
      // Switch based upon the three types of events
      switch (GetType().Name)
      {
        case "Drop":
          Drop d = (Drop) this;
          return d.ToString();

        case "Fail":
          Fail f = (Fail) this;
          return f.ToString();

        case "Success":
          Success s = (Success) this;
          return s.ToString();

        default:
          return null;
      }
    }

    #endregion

  }
}
