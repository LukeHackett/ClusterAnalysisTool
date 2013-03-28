/// Copyright (c) 2013, Research In Motion Limited.
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
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cluster;
using Analysis.Heatmap;
using Ionic.Zip;

namespace KML
{
  /// <summary>
  /// This class provides numerious methods that allows a Google Earth Keyhole
  /// Markup Language compressed file (KMZ) to be created.
  /// A KMZ file can contain numerious KML files, and addtional supporting 
  /// material such as html, css, javascript and image files.
  /// </summary>
  public static class KMZWriter
  {

    /// <summary>
    /// This method will create a KMZ file from an existing KML file, and an
    /// existing heatmap image.
    /// </summary>
    /// <param name="kml">The location of the KML file</param>
    /// <param name="heatmap">The location of the heatmap image</param>
    /// <param name="output">The location to output the KMZ file</param>
    public static void GenerateKMZ(String kml, String heatmap, String output)
    {
      using (ZipFile zip = new ZipFile())
      {
        // Add the KML file
        zip.AddFile(kml, "./");
        
        // Add the heatmap image
        zip.AddDirectory(heatmap, "./");

        // Save the file
        zip.Save(output);
      }
    }

    /// <summary>
    /// This method will create a KMZ file based upon the input clusters. A new 
    /// internal KML file will be created, along with a heatmap. Both of these 
    /// seperate elements will be merged into one KMZ file, at the given output 
    /// location.
    /// </summary>
    /// <param name="clusters">A list of clusters</param>
    /// <param name="output">The location to output the KMZ file</param>
    public static void GenerateKMZ(List<EventCollection> clusters, String output)
    {
      GenerateKMZ(clusters, new EventCollection(), output);
    }

    /// <summary>
    /// This method will create a KMZ file based upon the input clusters, and 
    /// the collection of noise. A new internal KML file will be created, along
    /// with a heatmap. The heatmap created will only be for the clusters that 
    /// were deemed not to be noise. 
    /// Both of these seperate elements will be merged into one KMZ file, at 
    /// the given output location.
    /// </summary>
    /// <param name="clusters">An list of EventCollections representing clusters</param>
    /// <param name="noise">An EventCollection of noise</param>
    /// <param name="output">The location to output the KMZ file</param>
    public static void GenerateKMZ(List<EventCollection> clusters, EventCollection noise, String output)
    {
      // Setup a temp directory to do the work in
      String tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
      tempDirectory += "\\";
      Directory.CreateDirectory(tempDirectory);
      
      // Create the KML and heatmap file paths
      String kml = tempDirectory + "data.kml";
      String heatmap = tempDirectory + "heatmap.png";

      // Generate the heatmap inc the KML if possible
      if (clusters.Count > 0)
      {
        Heatmap map = new Heatmap(clusters);
        map.GenerateHeatMap(heatmap);
        // Generate the KML
        KMLWriter.GenerateKML(clusters, noise, kml, "heatmap.png");
      }
      else
      {
        // Generate the KML on its own (no heatmap)
        KMLWriter.GenerateKML(clusters, noise, kml);
      }
      // Create the GMZ File
      SaveToGMZ(tempDirectory, output);

      // Remove the temp directory
      Directory.Delete(tempDirectory, true);
    }

    /// <summary>
    /// This method will merge all files found within the given directory into 
    /// a new KMZ file.
    /// </summary>
    /// <param name="directory">The directory to merge into the KMZ file</param>
    /// <param name="output">The location to output the KMZ file</param>
    private static void SaveToGMZ(String directory, String output)
    {
      using (ZipFile zip = new ZipFile())
      {
        // Add the entire directory to the archive
        zip.AddDirectory(directory, "./");

        // Save the file
        zip.Save(output);
      }
    }    
  }
}
