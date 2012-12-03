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
using Cluster;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace Analysis.Heatmap
{
  /// <summary>
  /// This class provides methods and functionality to be able to create a heat 
  /// map based upon an input set of coordinates. This class is a merge between 
  /// the GHeat library and a guide found upon the internet. The ideas of this 
  /// merge was to create a simple, non-bloated class to create an accurate heat 
  /// map.
  /// 
  /// Resources:
  /// http://dylanvester.com/post/Creating-Heat-Maps-with-NET-20-(C-Sharp).aspx
  /// http://code.google.com/p/gheat/
  /// </summary>
  public class Heatmap
  {
    #region Properties

    /// <summary>
    /// A list of heat points used to create the final image.
    /// </summary>
    private List<HeatPoint> HeatPoints { get; set; }

    /// <summary>
    /// A list of longitude and latitude coordinates.
    /// </summary>
    public List<Coordinate> Coordinates { get; private set; }

    /// <summary>
    /// The final size of the image.
    /// </summary>
    public Size Size { get; set; }

    /// <summary>
    /// The Radius of each heat point in pixels.
    /// </summary>
    public int Radius { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Primary Constructor
    /// </summary>
    /// <param name="coordinates">A list of coordinates to be processed</param>
    public Heatmap(List<Coordinate> coordinates)
    {
      Coordinates = coordinates;
      Size = new Size(1024, 1024);
      Radius = 50;
      HeatPoints = GenerateHeatPoints(coordinates);
    }

    /// <summary>
    /// Secondary Constructor
    /// </summary>
    /// <param name="coordinates">A list of coordinates to be processed</param>
    /// <param name="radius">The radius of each heat point</param>
    /// <param name="border">The size of the border around the image</param>
    public Heatmap(List<Coordinate> coordinates, Size size, int radius)
    {
      Coordinates = coordinates;
      Size = size;
      Radius = radius;
      HeatPoints = GenerateHeatPoints(coordinates);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// This method will generate a heat map returning the in memory bitmap 
    /// image.
    /// </summary>
    /// <param name="grayscale">Grayscale or Colored</param>
    /// <returns>A bitmap object containing the heatmap</returns>
    public Bitmap GenerateHeatMap(Boolean grayscale = false)
    {
      // Create a new in mempory image
      Bitmap image = InitialiseImage();

      // Create a grayscale intensity mask
      image = CreateIntensityMap(image);

      // Colorise the bitmap
      if (!grayscale)
      {
        image = Colorise(image, 255);
      }

      // Force the image to be transparent
      image.MakeTransparent();

      return image;
    }

    /// <summary>
    /// This method will generate a heat map and output the image to the 
    /// specified output location.
    /// </summary>
    /// <param name="file">Output file name</param>
    public void GenerateHeatMap(String file)
    {
      Bitmap image = GenerateHeatMap();
      image.Save(file);
    }

    /// <summary>
    /// This method will convert a list of coordinates to a list of screen point 
    /// objects. Each coordinate will be converted to a screen point object that 
    /// is relative to the minimum and maximum coordinate values found within 
    /// the given coordinate list.
    /// </summary>
    /// <param name="coordinates">A list of coordinates to be converted</param>
    /// <returns>A list of HeatPoint objects</returns>
    public List<HeatPoint> GenerateHeatPoints(List<Coordinate> coordinates)
    {
      // List of screen point objects
      List<HeatPoint> points = new List<HeatPoint>();

      // Get the upper and lower ranges
      Hashtable ranges = Ranges();
      Coordinate min = (Coordinate)ranges["min"];
      Coordinate max = (Coordinate)ranges["max"];

      // Loop over each Coordinate
      foreach (Coordinate c in Coordinates)
      {
        // Create a new relative screen point
        Point p = Translate(c, min, max);

        // Add to the heat points
        points.Add(new HeatPoint(p.X, p.Y, (byte)60));
      }

      return points;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method will create a new image that is of Size (see member variable) 
    /// plus Border number of pixels (see member varaible) added to each side of 
    /// the image. The border has been added to conver the size of the radius
    /// </summary>
    /// <returns>A new blank bitmap image</returns>
    private Bitmap InitialiseImage()
    {
      // Ensure that there are HeatPoints
      if (HeatPoints.Count == 0)
      {
        throw new ArgumentException("HeatPoints is empty");
      }

      // Select all the X values
      var X = (from hp in HeatPoints select hp.X);

      // Select all the Y values
      var Y = (from hp in HeatPoints select hp.Y);

      // Add a border so that all points can be seen
      var minX = X.Min() - Radius;
      var minY = Y.Min() - Radius;

      var maxX = X.Max() + Radius;
      var maxY = Y.Max() + Radius;

      var newX = maxX - minX;
      var newY = maxY - minY;

      return new Bitmap(newX, newY);
    }
    
    /// <summary>
    /// This method returns the minimum and maximum coordinate values within the
    /// data set. The minimum longitude and latitude values form the lower 
    /// coordinate value and the maximum long longitude and latitude values form 
    /// the upper coordinate value.
    /// </summary>
    /// <returns>A hashtable containing the minimum and maximum value</returns>
    private Hashtable Ranges()
    {
      // Get all the Longitude values from the data set.
      var longitudes = (from c in Coordinates select c.Longitude);

      // Get all the Latitude values from the data set.
      var latitudes = (from c in Coordinates select c.Latitude);

      // Create a new Hashtable to store the min/max values.
      Hashtable table = new Hashtable();
      table["min"] = new Coordinate(longitudes.Min(), latitudes.Min());
      table["max"] = new Coordinate(longitudes.Max(), latitudes.Max());

      return table;
    }

    /// <summary>
    /// This method will translate a coordinate value (Longitude/Latitude) into 
    /// a pixel, which will be offset based upon the minimum and maximum values.
    /// </summary>
    /// <param name="c">The coordinate to be converted.</param>
    /// <param name="min">The minimum coordinate value in the set.</param>
    /// <param name="max">The maximum coordinate value in the set.</param>
    /// <returns></returns>
    private Point Translate(Coordinate c, Coordinate min, Coordinate max)
    {
      // Get the long / lat values (Sanity purpoes only)
      double x = c.Longitude;
      double y = c.Latitude;

      // normalize points into range (0 - 1)
      x = (x - min.Longitude) / (max.Longitude - min.Longitude);
      y = (y - min.Latitude) / (max.Latitude - min.Latitude);

      // Map onto the image size
      int finalX = (int)(x * Size.Width) + Radius;
      int finalY = (int)((1 - y) * Size.Height) + Radius;

      // Create a new Point
      return new Point(finalX, finalY);
    }

    /// <summary>
    /// This method will generate an intensity mask drawing each HeatPoint upon 
    /// the bitmap with the given Radius (see member variable).
    /// </summary>
    /// <param name="bSurface">A new bitmap image</param>
    /// <returns></returns>
    private Bitmap CreateIntensityMap(Bitmap bSurface)
    {
      // Create new graphics surface from memory bitmap
      Graphics surface = Graphics.FromImage(bSurface);

      // Set background color to white so that pixels can be correctly colorized
      surface.Clear(Color.White);

      // Traverse heat point data and draw masks for each heat point
      foreach (HeatPoint point in HeatPoints)
      {
        // Render current heat point on draw surface
        DrawHeatPoint(surface, point);
      }

      return bSurface;
    }

    /// <summary>
    /// This method is used to draw an actual radial gradient "spot" on the 
    /// drawing surface. The method is able to draw spots of varying size and 
    /// density.
    /// </summary>
    /// <param name="Canvas">The current canvas graphics object</param>
    /// <param name="HeatPoint">The point to be drawn onto the canvas</param>
    private void DrawHeatPoint(Graphics Canvas, HeatPoint HeatPoint)
    {
      // Create points generic list of points to hold circumference points
      List<Point> CircumferencePointsList = new List<Point>();

      // Create an empty point to predefine the point struct used in the circumference loop
      Point CircumferencePoint;

      // Create an empty array that will be populated with points from the generic list
      Point[] CircumferencePointsArray;

      // Calculate ratio to scale byte intensity range from 0-255 to 0-1
      float fRatio = 1F / Byte.MaxValue;

      // Precalulate half of byte max value
      byte bHalf = Byte.MaxValue / 2;

      // Flip intensity on it's center value from low-high to high-low
      int iIntensity = (byte)(HeatPoint.Intensity - ((HeatPoint.Intensity - bHalf) * 2));

      // Store scaled and flipped intensity value for use with gradient center location
      float fIntensity = iIntensity * fRatio;

      // Loop through all angles of a circle
      for (double i = 0; i <= 360; i += 10)
      {
        // Replace last iteration point with new empty point structure
        CircumferencePoint = new Point();

        // Plot new point on the circumference of a circle of the defined radius
        // Using the point coordinates, radius, and angle
        // Calculate the position of this iterations point on the circle
        double x = HeatPoint.X + Radius * Math.Cos(Coordinate.DegreesToRadians(i));
        double y = HeatPoint.Y + Radius * Math.Sin(Coordinate.DegreesToRadians(i));

        CircumferencePoint.X = Convert.ToInt32(x);
        CircumferencePoint.Y = Convert.ToInt32(y);

        // Add newly plotted circumference point to generic point list
        CircumferencePointsList.Add(CircumferencePoint);
      }

      // Populate empty points system array from generic points array list
      // Do this to satisfy the data type of the PathGradientBrush and FillPolygon methods
      CircumferencePointsArray = CircumferencePointsList.ToArray();

      // Create new PathGradientBrush to create a radial gradient using the circumference points
      PathGradientBrush GradientShaper = new PathGradientBrush(CircumferencePointsArray);

      // Create new color blend to tell the PathGradientBrush what colors to use and where to put them
      ColorBlend GradientSpecifications = new ColorBlend(3);

      // Define positions of gradient colors, use intesity to adjust the middle color to
      // show more mask or less mask
      GradientSpecifications.Positions = new float[3] { 0, fIntensity, 1 };

      // Define gradient colors and their alpha values, adjust alpha of gradient colors to match intensity
      GradientSpecifications.Colors = new Color[3]
        {
            Color.FromArgb(0, Color.White),
            Color.FromArgb(60, Color.Black),
            Color.FromArgb(HeatPoint.Intensity, Color.Black)
        };

      // Pass off color blend to PathGradientBrush to instruct it how to generate the gradient
      GradientShaper.InterpolationColors = GradientSpecifications;

      // Draw polygon (circle) using our point array and gradient brush
      Canvas.FillPolygon(GradientShaper, CircumferencePointsArray);
    }
    
    /// <summary>
    /// This method will convert the given bitmap grayscale mask into a colorised 
    /// heat map.
    /// </summary>
    /// <param name="Mask">The original greyscale bitmap</param>
    /// <param name="Alpha">The number of colors (typically 255)</param>
    /// <returns></returns>
    public Bitmap Colorise(Bitmap mask, byte alpha)
    {
      // Create new bitmap to act as a work surface for the colorization process
      Bitmap Output = new Bitmap(mask.Width, mask.Height, PixelFormat.Format32bppArgb);

      // Create a graphics object from our memory bitmap so we can draw on it and clear it's drawing surface
      Graphics Surface = Graphics.FromImage(Output);
      Surface.Clear(Color.Transparent);

      // Build an array of color mappings to remap our greyscale mask to full color
      // Accept an alpha byte to specify the transparancy of the output image
      ColorMap[] Colors = CreatePaletteIndex(alpha);

      // Create new image attributes class to handle the color remappings
      // Inject our color map array to instruct the image attributes class how to do the colorization
      ImageAttributes Remapper = new ImageAttributes();
      Remapper.SetRemapTable(Colors);

      // Draw our mask onto our memory bitmap work surface using the new color mapping scheme
      Rectangle rectangle = new Rectangle(0, 0, mask.Width, mask.Height);
      Surface.DrawImage(mask, rectangle, 0, 0, mask.Width, mask.Height, GraphicsUnit.Pixel, Remapper);

      // Send back newly colorized memory bitmap
      return Output;
    }

    /// <summary>
    /// This method will return a pallete index array to use to map a grayscale 
    /// color to an ARGB value.
    /// </summary>
    /// <param name="Alpha">The alpha value</param>
    /// <returns>A ColorMap array</returns>
    private ColorMap[] CreatePaletteIndex(byte alpha)
    {
      ColorMap[] OutputMap = new ColorMap[256];

      // Get the desired colour scheme
      List<Color> colours = ColorScheme.GetPalette(ColorScheme.Scheme.Classic);

      // Loop through each pixel and create a new color mapping
      for (int X = 0; X <= 255; X++)
      {
        OutputMap[X] = new ColorMap();
        OutputMap[X].OldColor = Color.FromArgb(X, X, X);
        OutputMap[X].NewColor = Color.FromArgb(alpha, colours[X]);
      }

      return OutputMap;
    }

    #endregion
  }
}
