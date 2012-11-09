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
using System.Xml;

namespace KML
{
  /// <summary>
  /// A bespoke, simple KML writer class, that will write coordinates into a set structed file.
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
                                  };
    }
    
    #endregion
    
    #region Public Static Methods
    
    /// <summary>
    /// This method will generate a well formed KML file, based upon the list of clusters passed. The
    /// method assumes that the data contains no noise, if there is noise to be represented in its 
    /// own KML folder, then GenerateKML/3 should be used.
    /// </summary>
    /// <see cref="KML.GenerateKML"/>
    /// <param name="file"></param>
    /// <param name="clusters"></param>
    public static void GenerateKML(String file, List<CoordinateCollection> clusters)
    {
      GenerateKML(file, clusters, new CoordinateCollection());
    }

    /// <summary>
    /// This method will write a list of clusters and a list of coordinates deemed as noise to a given
    /// KML file. The wite process will overwrite the file if it already exists, there is no ability 
    /// to append to the file. Each cluster will be assigned to a KML Folder, with the collection of 
    /// noise coordinates being assigned to a KML folder of its own.
    /// </summary>
    /// <param name="file">the name of the KML file</param>
    /// <param name="clusters">A list of clusters</param>
    /// <param name="noise">A list of coordinates deemed to be noise</param>
    public static void GenerateKML(String file, List<CoordinateCollection> clusters, CoordinateCollection noise)
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
      Kml.WriteElementString("name", "DOCUMENT NAME");
      GenerateStyles();
      
      // Write the Clusters to the KML file
      foreach (CoordinateCollection cluster in clusters)
      {
        WriteCluster(cluster);
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
    /// This method will write the styles to the KML file. The styles are defined within the styles
    /// property.
    /// </summary>
    private static void GenerateStyles()
    {
      foreach (String[] style in Styles)
      {
        Kml.WriteStartElement("Style");
        Kml.WriteAttributeString("id", style[0]);

        // Ballon Style
        Kml.WriteStartElement("BalloonStyle");
        Kml.WriteElementString("text", style[1]);
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
    }
    
    /// <summary>
    /// This method will write each coordinate found within the given Coordinate Collection to the 
    /// KML file. Additional meta data such as the name and description of the cluster, and the name 
    /// and description of each coordinate within the cluster. The method will also deduce which type 
    /// of visual pointer should be used to show the point upon the map.
    /// </summary>
    /// <param name="cluster">A Collection of clusters</param>
    private static void WriteCluster(CoordinateCollection cluster)
    {
      // Create a new Folder
      Kml.WriteStartElement("Folder");
      Kml.WriteElementString("name", cluster.Name);
      Kml.WriteElementString("description", cluster.Description);
      
      // Create a new Placemark for each Coordinate
      int i = 0;
      foreach(Coordinate c in cluster)
      {
        // Create a new Placemark
        Kml.WriteStartElement("Placemark");
        Kml.WriteElementString("name", "Coordinate " + i);
        
        // Create a fancy description
        String description = CreateCooridnateDescription(c);
        Kml.WriteStartElement("description");
        Kml.WriteCData(description);
        Kml.WriteEndElement();
        
        // Set the Style
        String style = "#" + c.CallStatus.ToString();     
        style += (c.Noise) ? "-noise" : "";
        Kml.WriteElementString("styleUrl", style);
        
        // Create a new Point
        Kml.WriteStartElement("Point");
        Kml.WriteElementString("coordinates", c.ToString());
        Kml.WriteEndElement();
        
        // Finish the Placemark
        Kml.WriteEndElement();        
        i++;
      }
      // Finish the Folder
      Kml.WriteEndElement();
    }
    
    /// <summary>
    /// This method will create a html string that can be used as a hover description.
    /// </summary>
    /// <param name="c">Coordinate</param>
    /// <returns>A well formed html string description</returns>
    private static String CreateCooridnateDescription(Coordinate c)
    {
      return String.Format("<div class=\"googft-info-window\">" + 
                              "<b>Cordinate:</b> {0} <br>" +
                              "<b>Latitude:</b> {1} <br>" +
                              "<b>Longitude:</b> {2} <br>" +
                            "</div>", c.ToString(), c.Latitude, c.Longitude);
    }
    
    #endregion
  }
}
