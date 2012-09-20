using Cluster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace KML
{
  /// <summary>
  /// A simple static KML reader class.
  /// </summary>
  public static class KMLReader
  {
    #region Public Static Methods
    
    /// <summary>
    /// This method will read a given KML file, and pull all the <coordinate> values from the file. 
    /// These values will be stored and returned within one CoordinateCollection object (a list of 
    /// coordinates).
    /// </summary>
    /// <param name="file">the KML to be read</param>
    /// <returns>A list of cooridnates as a CoordinateCollection</returns>
    public static CoordinateCollection GetCoordinates(String file)
    {    
      XmlTextReader reader = new XmlTextReader(file);
      CoordinateCollection coordinates = new CoordinateCollection(); 
      // Loop over all coordinate elements
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
          Coordinate c = new Coordinate(lat, lon);
          // Add to the coordinates list
          coordinates.Add(c);
        }
      }
      return coordinates;
    }
    
    #endregion
  }
}
