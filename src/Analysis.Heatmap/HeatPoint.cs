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
using System.Drawing;
using System.Linq;
using System.Text;

namespace Analysis.Heatmap
{
  /// <summary>
  /// This class metaphorically speaking extends the Point structure in order to 
  /// create a HeatPoint object. This object will mimic a heat point, and 
  /// contains location information as well as density information.
  /// 
  /// Resources:
  /// http://dylanvester.com/post/Creating-Heat-Maps-with-NET-20-(C-Sharp).aspx
  /// </summary>
  public class HeatPoint
  {
    #region Properties

    /// <summary>
    /// Horizontal axis value
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// Vertical axis value
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// Intensity value of the point
    /// </summary>
    public byte Intensity { get; set; }

    #endregion

    #region Contructor

    /// <summary>
    /// Primary Constructor
    /// </summary>
    /// <param name="x">Horizontal axis value</param>
    /// <param name="y">Vertical axis value</param>
    public HeatPoint(int x, int y)
      : base()
    {
      X = x;
      Y = y;
    }

    /// <summary>
    /// Secondary Constructor
    /// </summary>
    /// <param name="x">Horizontal axis value</param>
    /// <param name="y">Vertical axis value</param>
    /// <param name="intensity">Intensity value</param>
    public HeatPoint(int x, int y, byte intensity)
      : base()
    {
      X = x;
      Y = y;
      Intensity = intensity;
    }

    #endregion

  }
}
