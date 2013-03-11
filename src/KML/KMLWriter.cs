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
using System.IO;
using System.Collections;
using System.Collections.Specialized;

namespace KML
{
  /// <summary>
  /// A bespoke, simple KML writer class, that will write coordinates into a set 
  /// structed file.
  /// </summary>
  public static class KMLWriter
  {
    #region Properties
    
    /// <summary>
    /// KML Writer object
    /// </summary>
    private static XmlTextWriter Kml;
    
    /// <summary>
    /// BaseURL for any kml images hosted by Google
    /// </summary>
    private static String IconBaseUrl = "http://google.com/mapfiles/ms/micons/";

    /// <summary>
    /// A list of various states and their styles
    /// </summary>
    private static List<String[]> Styles;
   
    #endregion
    
    #region Constructor
    
    /// <summary>
    /// Default Constructor
    /// </summary>
    static KMLWriter()
    {    
      Styles = new List<String[]> { new String[]{"drop", "Call Drop", "red.png"}, 
                                    new String[]{"fail", "Call Setup Failure", "yellow.png"}, 
                                    new String[]{"success", "Successful Failure", "green.png"},
                                    
                                    new String[]{"drop-noise", "Call Drop (Noise)", "red-dot.png"}, 
                                    new String[]{"fail-noise", "Call Setup Failure (Noise)", "yellow-dot.png"}, 
                                    new String[]{"success-noise", "Successful Failure (Noise)", "green-dot.png"},

                                    new String[]{"centroid", "Centroid", "blue.png"}
                                  };
    }
    
    #endregion
    
    #region Public Static Methods
    
    /// <summary>
    /// This method will generate a well formed KML file, based upon the list of 
    /// event clusters passed. The method assumes that the data contains no noise, 
    /// if there is noise to be represented in its own KML folder, then 
    /// GenerateKML/3 should be used.
    /// </summary>
    /// <see cref="KML.GenerateKML"/>
    /// <param name="file">The KML file output name</param>
    /// <param name="clusters">A list of collection of events</param>
    public static void GenerateKML(String file, List<EventCollection> clusters, String location = null)
    {
      GenerateKML(file, clusters, new EventCollection(), location);
    }

    /// <summary>
    /// This method will write a list of clusters and a list of coordinates deemed 
    /// as noise to a given KML file. The wite process will overwrite the file if 
    /// it already exists, there is no ability ato append to the file. 
    /// Each cluster will be assigned to a KML Folder, with the collection of 
    /// noise coordinates being assigned to a KML folder of its own.
    /// </summary>
    /// <param name="file">the name of the KML file</param>
    /// <param name="clusters">A list of collection of events</param>
    /// <param name="noise">A collection of events that are deemed to be noise</param>
    public static void GenerateKML(String file, List<EventCollection> clusters, EventCollection noise, String location = null)
    {
      // Initalise a new Writer
      Kml = new XmlTextWriter(file, Encoding.UTF8);
      Kml.Formatting = Formatting.Indented;
      Kml.Indentation = 2;
      
      // Define the KML document
      Kml.WriteStartDocument();
      Kml.WriteStartElement("kml", "http://www.opengis.net/kml/2.2");
      
      // Define the Document & Style Sheets
      Kml.WriteStartElement("Document");
      Kml.WriteElementString("name", Path.GetFileName(file));
      GenerateStyles();

      // Generate a Heatmap if requested
      if (location != null)
      {
        WriteHeatmap(location, clusters, noise);
      }
      
      // Write the Clusters to the KML file
      for (int i = 0; i < clusters.Count; i++)
      {
        WriteCluster(clusters[i], "Cluster " + i);
      }
      
      //Write the Noise to the KML file
      if(noise.Count > 0)
      {
        WriteCluster(noise);
      }

      // End Document
      Kml.WriteEndElement();
      
      // Write the XML to file and close the writer.
      Kml.Flush();
      Kml.Close();
    }
    
    #endregion
    
    #region Private Static Methods
    
    /// <summary>
    /// This method will write the styles to the KML file. The styles are defined 
    /// within the styles property.
    /// </summary>
    private static void GenerateStyles()
    {
      foreach (String[] style in Styles)
      {
        // Create the seperate Show / Hide styles for each Style
        for (int i = 0; i <= 1; i++)
        {
          Kml.WriteStartElement("Style");
          String styleId = style[0];
          styleId += (i == 0) ? "-hide" : "-show";
          Kml.WriteAttributeString("id", styleId);

          // Label Style
          String scale = (i == 0) ? "0.0" : "1.2"; 
          Kml.WriteStartElement("LabelStyle");
          Kml.WriteElementString("scale", scale);
          Kml.WriteEndElement();

          // IconStyle
          Kml.WriteStartElement("IconStyle");
          Kml.WriteElementString("color", "FFFFFFFF");
          Kml.WriteElementString("scale", "1.2");
          Kml.WriteStartElement("Icon");
          Kml.WriteElementString("href", IconBaseUrl + style[2]);
          Kml.WriteEndElement();

          // End Ballon Style
          Kml.WriteEndElement();

          // End Style
          Kml.WriteEndElement();
        }

        // Create the main style
        Kml.WriteStartElement("StyleMap");
        Kml.WriteAttributeString("id", style[0]);
        // Create the "Hide Pair"
        Kml.WriteStartElement("Pair");
        Kml.WriteElementString("key", "normal");
        Kml.WriteElementString("styleUrl", String.Format("#{0}-hide", style[0]));
        Kml.WriteEndElement();
        // Create the "Show Pair"
        Kml.WriteStartElement("Pair");
        Kml.WriteElementString("key", "highlight");
        Kml.WriteElementString("styleUrl", String.Format("#{0}-show", style[0]));
        Kml.WriteEndElement();
        // End Style
        Kml.WriteEndElement();

        // Create the Radius Style
        Kml.WriteStartElement("Style");
        Kml.WriteAttributeString("id", "centroid-radius");
        // LineStyle
        Kml.WriteStartElement("LineStyle");
        Kml.WriteElementString("color", "ffff0000");
        Kml.WriteElementString("width", "2");
        Kml.WriteEndElement();
        // PolyStyle
        Kml.WriteStartElement("PolyStyle");
        Kml.WriteElementString("color", "7fff7920");
        Kml.WriteEndElement();
        // End Style
        Kml.WriteEndElement();
      }
    }

    /// <summary>
    /// This method will create a Heatmap folder within the KML file to allow 
    /// for a heatmap to be displayed. The location of the heatmap must be 
    /// given as a parameter.
    /// </summary>
    /// <param name="location">The location of the image</param>
    private static void WriteHeatmap(String location, List<EventCollection> clusters, EventCollection noise)
    {
      // Obtain a hastable of postitions for the heatmap
      StringDictionary positions = GetImagePositions(clusters, noise);

      // Folder
      Kml.WriteStartElement("Folder");
      Kml.WriteElementString("name", "Heatmap");

      // Ground Overlay
      Kml.WriteStartElement("GroundOverlay");

      // Icon href
      Kml.WriteStartElement("Icon");
      Kml.WriteElementString("href", location);
      Kml.WriteEndElement();

      // LatLonBox
      Kml.WriteStartElement("LatLonBox");
      Kml.WriteElementString("north", positions["north"]);
      Kml.WriteElementString("south", positions["south"]);
      Kml.WriteElementString("east", positions["east"]);
      Kml.WriteElementString("west", positions["west"]);

      Kml.WriteEndElement();
      Kml.WriteEndElement();
      Kml.WriteEndElement();
    }

    /// <summary>
    /// This method will return the positional coordinates for each of the four
    /// corners of the heatmap. This allows for the heatmap image to be placed 
    /// at these locations (north, south, east, west).
    /// </summary>
    /// <param name="clusters">A list of Clusters</param>
    /// <param name="noise">A list of noise data</param>
    /// <returns>A StringDictionarry of positions</returns>
    private static StringDictionary GetImagePositions(List<EventCollection> clusters, EventCollection noise)
    {
      // Make a 'clone' of the input data structures
      // Add the noise to the list of event collections
      List<EventCollection> collection = new List<EventCollection>();
      collection.AddRange(clusters);
      collection.Add(noise);

      // Obtain all the longitude values
      var longitudes = collection.SelectMany(z => z.GetEvents())
                                 .Select(evt => evt.Coordinate.Longitude);

      // Obtain all the latitude values
      var latitudes = collection.SelectMany(z => z.GetEvents())
                                .Select(evt => evt.Coordinate.Latitude);

      // Create a new Hashtable to store the min/max values.
      StringDictionary table = new StringDictionary();
      table["north"] = latitudes.Max().ToString();
      table["south"] = latitudes.Min().ToString();
      table["east"] = longitudes.Max().ToString();
      table["west"] = longitudes.Min().ToString();

      return table;
    }

    /// <summary>
    /// This method will write each coordinate found within the given event 
    /// Collection to the KML file. Additional meta data such as the name and 
    /// description of the cluster, and the name and description of each coordinate 
    /// within the cluster. The method will also deduce which type of visual 
    /// pointer should be used to show the point upon the map.
    /// </summary>
    /// <param name="cluster">A Collection of events</param>
    private static void WriteCluster(EventCollection cluster, String name = null)
    {
      // Get the name of the cluster if over written via the parameter
      name = (name == null) ? cluster.Name : name;

      // Create a new Folder
      Kml.WriteStartElement("Folder");
      Kml.WriteElementString("name", name);
      Kml.WriteElementString("description", cluster.Description);
      
      // Create a placemark for the centroid cluster
      WriteCentroid(cluster.Centroid);

      // Create a Radius around the centroid
      if (!cluster.Noise)
      {
        double radius = cluster.Centroid.Radius * 1000;
        WriteRadialCircle(cluster.Centroid, radius);
      }

      // Create a new Placemark for each Coordinate
      foreach(Event evt in cluster)
      {
        WriteEvent(evt);
      }

      // Finish the Folder
      Kml.WriteEndElement();
    }

    /// <summary>
    /// This method will write the given event to the output KML file.
    /// </summary>
    /// <param name="cluster">The event to be written to the output kml</param>
    private static void WriteEvent(Event cluster)
    {
      // Get the event name based upon the class name
      String eventName = cluster.GetType().Name;

      // Create a new Placemark
      Kml.WriteStartElement("Placemark");
      Kml.WriteElementString("name", eventName);

      // Create a fancy description
      String description = CreateCoordinateDescription(cluster);
      Kml.WriteStartElement("description");
      Kml.WriteCData(description);
      Kml.WriteEndElement();

      // Set the Style
      String style = "#" + eventName.ToLower();
      style += (cluster.Coordinate.Noise) ? "-noise" : "";
      Kml.WriteElementString("styleUrl", style);

      // Create a new Point
      Kml.WriteStartElement("Point");
      Kml.WriteElementString("coordinates", cluster.Coordinate.ToString());
      Kml.WriteEndElement();

      // Finish the Placemark
      Kml.WriteEndElement();   
    }

    /// <summary>
    /// This method will write the given centroid to the output KML file.
    /// </summary>
    /// <param name="cluster">The centroid to be written to the output kml</param>
    private static void WriteCentroid(Centroid centroid)
    {
      // Create a new Placemark
      Kml.WriteStartElement("Placemark");
      Kml.WriteElementString("name", "Centroid");

      // Set the Style
      Kml.WriteElementString("styleUrl", "#centroid");

      // Create a fancy description
      String description = CreateCoordinateDescription(centroid);
      Kml.WriteStartElement("description");
      Kml.WriteCData(description);
      Kml.WriteEndElement();

      // Create a new Point
      Kml.WriteStartElement("Point");
      Kml.WriteElementString("coordinates", centroid.ToString());
      Kml.WriteEndElement();

      // Finish the Placemark
      Kml.WriteEndElement();  
    }

    /// <summary>
    /// This method will write a radial circle to the output KML file. Google 
    /// Earth does not support a circle, a point is written and rotated around 
    /// a given centroid object 360 times to make a radial circle.
    /// </summary>
    /// <param name="centre">The centroid of the radial circle</param>
    /// <param name="radius">The radius of the circle (in KM)</param>
    private static void WriteRadialCircle(Centroid centre, double radius)
    {
      // New Placemark
      Kml.WriteStartElement("Placemark");
      Kml.WriteElementString("name", "Cluster Radius");
      Kml.WriteElementString("styleUrl", "#centroid-radius");
      
      // Setup various polgon elements
      Kml.WriteStartElement("Polygon");
      Kml.WriteStartElement("outerBoundaryIs");
      Kml.WriteStartElement("LinearRing");
      Kml.WriteStartElement("coordinates");

      // Get a list of Radial Locations to create the circle
      RadialCircle r = new RadialCircle(centre, radius);
      List<Coordinate> radialLocations = r.CreateRadialCircle();

      // Add each coordinate to the coordinates list
      foreach (Coordinate coordinate in radialLocations)
      {
        Kml.WriteValue(coordinate.ToString() + Environment.NewLine);
      }

      // End the Elements
      Kml.WriteEndElement();
      Kml.WriteEndElement();
      Kml.WriteEndElement();
      Kml.WriteEndElement();
      Kml.WriteEndElement();
    }

    /// <summary>
    /// This method will create a html string that can be used as a hover 
    /// description.
    /// </summary>
    /// <param name="evt">The event to have a description created for</param>
    /// <returns>A well formed html string description</returns>
    private static String CreateCoordinateDescription(Event evt)
    {
      return String.Format("<div class=\"googft-info-window\" style=\"width:300px\">" + 
                              "<table>" + 
                                "<tr>" +
                                  "<td><b>Coordinate: </b></td>" +
                                  "<td>{0}</td>" +
                                "</tr>" +
                                "<tr>" +
                                  "<td><b>Device: </b></td>" +
                                  "<td>{1}</td>" +
                                "<tr>" +
                                "<tr>" +
                                  "<td><b>Reference Device: </b></td>" +
                                  "<td>{2}</td>" +
                                "<tr>" +
                                "<tr>" +
                                  "<td><b>PIN: </b></td>" +
                                  "<td>{3}</td>" +
                                "<tr>" +
                                "<tr>" +
                                  "<td><b>Timestamp: </b></td>" +
                                  "<td>{4}</td>" +
                                "<tr>" +
                                "<tr>" +
                                  "<td><b>Start RAT: </b></td>" +
                                  "<td>{5}</td>" +
                                "<tr>" +
                                "<tr>" +
                                  "<td><b>End RAT: </b></td>" +
                                  "<td>{6}</td>" +
                                "<tr>" +
                                "<tr>" +
                                  "<td><b>Start Mix-Band: </b></td>" +
                                  "<td>{7}</td>" +
                                "<tr>" +
                                "<tr>" +
                                  "<td><b>End Mix-Band: </b></td>" +
                                  "<td>{8}</td>" +
                                "<tr>" +
                                "<tr>" +
                                  "<td><b>Start RRC State: </b></td>" +
                                  "<td>{9}</td>" +
                                "<tr>" +
                              "</table>" +
                            "</div>",
                             evt.Coordinate.ToString(), 
                             evt.Device, 
                             evt.Reference.ToString(),
                             evt.Pin,
                             evt.Timestamp.ToString(),
                             evt.StartRat, 
                             evt.EndRat, 
                             evt.StartMixBand,
                             evt.EndMixBand,
                             evt.StartRRCState);
    }

    /// <summary>
    /// This method will create a html string that can be used as a hover 
    /// description. This method takes a straight Coordinate object, and should 
    /// be used when wanting to output the centroid
    /// </summary>
    /// <param name="c">The coordinate to have a description created for</param>
    /// <param name="centroid">Whether or not the coordinate is the centroid</param>
    /// <returns>A well formed html string description</returns>
    private static String CreateCoordinateDescription(Centroid centroid)
    {
      return String.Format("<div class=\"googft-info-window\" style=\"width:250px\">" +
                              "<table>" +
                                "<tr>" +
                                  "<td><b>Coordinate: </b></td>" +
                                  "<td>{0}</td>" +
                                "</tr>" +
                                "<tr>" +
                                  "<td><b>Radius: </b></td>" +
                                  "<td>{1}</td>" +
                                "</tr>" +
                              "</table>" +
                            "</div>",
                             centroid.ToString(),
                             centroid.Radius);
    }

    #endregion
  }
}
