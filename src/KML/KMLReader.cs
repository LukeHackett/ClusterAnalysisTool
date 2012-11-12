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
using System.Xml;
using Cluster;
using CallEvent;

namespace KML
{
  /// <summary>
  /// A simple KML reader class.
  /// </summary>
  public class KMLReader
  {
    #region Properties

    /// <summary>
    /// The XMLTextReader instance to read a given XML file
    /// </summary>
    private XmlTextReader reader;

    #endregion

    #region Constructors

    /// <summary>
    /// Default Constructor
    /// </summary>
    /// <param name="file">The file to read</param>
    public KMLReader(String file)
    {
      reader = new XmlTextReader(file);
      reader.WhitespaceHandling = WhitespaceHandling.None;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// This method will return an event collection of events from the given KML 
    /// file in the constructor of this object.
    /// </summary>
    /// <returns>A collection of events</returns>
    public EventCollection GetCallLogs()
    {
      EventCollection calls = new EventCollection();
      while (reader.Read())
      {
        // Get the Coordinate
        Coordinate coordinate = ReadCoordinate();
        
        // Move to the ExtendedData Space
        reader.Read();
        reader.ReadToNextSibling("ExtendedData");
        
        //Get the additional data elements
        String device = GetSimpleData("device");
        String pin = GetSimpleData("pin");
        String type = GetSimpleData("type");
        String start_rat = GetSimpleData("start_rat");
        String end_rat = GetSimpleData("end_rat");
        String start_mix_band = GetSimpleData("start_mix_band");
        String end_mix_band = GetSimpleData("end_mix_band");

        // Create a new Call Log Object
        Event callLog = null;

        // Force the call log to be a type
        switch (type)
        {
          case "drop":
            callLog = new Drop();
            break;

          case "fail":
            callLog = new Fail();
            break;

          case "success":
            callLog = new Success();
            break;

          default:
            break;
        }

        // Add additional attributes & add to the list
        if (callLog != null)
        {
          callLog.Device = device;
          callLog.Pin = pin;
          callLog.StartRat = start_rat;
          callLog.EndRat = end_rat;
          callLog.StartMixBand = start_mix_band;
          callLog.EndMixBand = end_mix_band;
          callLog.Coordinate = coordinate;

          calls.Add(callLog);
        }

      }

      return calls;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method will read a coordinates xml tag, and convert it into a 
    /// Coordinate object. If there are no xml tags with coordinates then the 
    /// method will return null.
    /// </summary>
    /// <returns>A coordinate object</returns>
    private Coordinate ReadCoordinate()
    {
      // The final coordinate object.
      Coordinate coordinate = null;

      while (reader.Read())
      {
        if(reader.Name == "coordinates" && reader.NodeType == XmlNodeType.Element)
        {
          // Read the coordinate value
          reader.Read();
          
          // Strip and Split into each Lat/Long/Elevation
          String[] values = reader.Value.Replace("\r\n", "").Split(',');

          // Create a new Coordinate Object
          double lat = Double.Parse(values[0]);
          double lon = Double.Parse(values[1]);
          coordinate = new Coordinate(lat, lon);

          // Break from the loop as a coordinate has been read
          break;
        }
      }

      return coordinate;
    }

    /// <summary>
    /// This method will return the value that is associated with the given name
    /// of a SimpleData element.
    /// </summary>
    /// <param name="attribute">the name of the simple data element</param>
    /// <returns>the string value found with that SimpleData element</returns>
    private String GetSimpleData(String attribute)
    {
      // The final value
      String value = null;

      while (reader.Read())
      {
        if (reader.Name == "SimpleData" && reader.NodeType == XmlNodeType.Element)
        {
          // Get the Name of the attribute
          reader.MoveToAttribute(attribute);
          reader.Read();

          // Grab the value
          value = reader.Value;

          // Break from the loop as the value has been read
          break;
        }
      }

      return value;
    }

    #endregion
  }
}
