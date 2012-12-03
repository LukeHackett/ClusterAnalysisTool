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
using System.Drawing;

namespace Analysis.Heatmap
{
  /// <summary>
  /// This static class provides several colour schemes that can be used when 
  /// colorising a heatmap. The color schemes have been based upon GHeat's color 
  /// scheme. The currently available color schemes are:
  ///  # Classic
  ///  # Fire
  ///  # OMG
  ///  # PBJ
  ///  # Pgaitch
  ///  
  /// Resources:
  /// http://code.google.com/p/gheat/
  /// </summary>
  public static class ColorScheme
  {
    #region Properties

    /// <summary>
    /// Available palette colour schemes
    /// </summary>
    public enum Scheme { Classic = 1, Fire, Omg, Pbj, Pgaitch }

    #endregion

    #region Public Static Methods

    /// <summary>
    /// This method will return the palette scheme for the given scheme. This 
    /// method is effectively a wrapper around all the other scheme methods.
    /// </summary>
    /// <param name="scheme">The name of the scheme to return</param>
    /// <returns>A list of color objects for the given scheme</returns>
    public static List<Color> GetPalette(Scheme scheme)
    {
      List<Color> colorScheme = null;

      switch (scheme.ToString())
      {
        // Classic Palette Scheme
        case "Classic":
          colorScheme = GetClassicPalette();
          break;

        // Fire Palette Scheme
        case "Fire":
          colorScheme = GetFirePalette();
          break;

        // OMG Palette Scheme
        case "omg":
          colorScheme = GetOmgPalette();
          break;

        // PBJ Palette Scheme
        case "Pbj":
          colorScheme = GetPbjPalette();
          break;

        // Pgaitch Palette Scheme
        case "Pgaitch":
          colorScheme = GetPgaitchPalette();
          break;

        default:
          throw new ArgumentException("Unknown Scheme", "scheme");
      }

      return colorScheme;
    }

    /// <summary>
    /// This method will return a list of colors that are associated with the 
    /// "Classic" palette. These colors will create a classic heatmap look and 
    /// feel.
    /// </summary>
    /// <returns>A list of "classic" color objects</returns>
    public static List<Color> GetClassicPalette()
    {
      List<Color> colors = new List<Color>();

      colors.Add(Color.FromArgb(255, 237, 237));
      colors.Add(Color.FromArgb(255, 237, 237));
      colors.Add(Color.FromArgb(255, 237, 237));
      colors.Add(Color.FromArgb(255, 237, 237));
      colors.Add(Color.FromArgb(255, 237, 237));
      colors.Add(Color.FromArgb(255, 237, 237));
      colors.Add(Color.FromArgb(255, 237, 237));
      colors.Add(Color.FromArgb(255, 237, 237));
      colors.Add(Color.FromArgb(255, 237, 237));
      colors.Add(Color.FromArgb(255, 224, 224));
      colors.Add(Color.FromArgb(255, 209, 209));
      colors.Add(Color.FromArgb(255, 193, 193));
      colors.Add(Color.FromArgb(255, 176, 176));
      colors.Add(Color.FromArgb(255, 159, 159));
      colors.Add(Color.FromArgb(255, 142, 142));
      colors.Add(Color.FromArgb(255, 126, 126));
      colors.Add(Color.FromArgb(255, 110, 110));
      colors.Add(Color.FromArgb(255, 94, 94));
      colors.Add(Color.FromArgb(255, 81, 81));
      colors.Add(Color.FromArgb(255, 67, 67));
      colors.Add(Color.FromArgb(255, 56, 56));
      colors.Add(Color.FromArgb(255, 46, 46));
      colors.Add(Color.FromArgb(255, 37, 37));
      colors.Add(Color.FromArgb(255, 29, 29));
      colors.Add(Color.FromArgb(255, 23, 23));
      colors.Add(Color.FromArgb(255, 18, 18));
      colors.Add(Color.FromArgb(255, 14, 14));
      colors.Add(Color.FromArgb(255, 11, 11));
      colors.Add(Color.FromArgb(255, 8, 8));
      colors.Add(Color.FromArgb(255, 6, 6));
      colors.Add(Color.FromArgb(255, 5, 5));
      colors.Add(Color.FromArgb(255, 3, 3));
      colors.Add(Color.FromArgb(255, 2, 2));
      colors.Add(Color.FromArgb(255, 2, 2));
      colors.Add(Color.FromArgb(255, 1, 1));
      colors.Add(Color.FromArgb(255, 1, 1));
      colors.Add(Color.FromArgb(255, 0, 0));
      colors.Add(Color.FromArgb(255, 0, 0));
      colors.Add(Color.FromArgb(255, 0, 0));
      colors.Add(Color.FromArgb(255, 0, 0));
      colors.Add(Color.FromArgb(255, 0, 0));
      colors.Add(Color.FromArgb(255, 0, 0));
      colors.Add(Color.FromArgb(255, 0, 0));
      colors.Add(Color.FromArgb(255, 0, 0));
      colors.Add(Color.FromArgb(255, 1, 0));
      colors.Add(Color.FromArgb(255, 4, 0));
      colors.Add(Color.FromArgb(255, 6, 0));
      colors.Add(Color.FromArgb(255, 10, 0));
      colors.Add(Color.FromArgb(255, 14, 0));
      colors.Add(Color.FromArgb(255, 18, 0));
      colors.Add(Color.FromArgb(255, 22, 0));
      colors.Add(Color.FromArgb(255, 26, 0));
      colors.Add(Color.FromArgb(255, 31, 0));
      colors.Add(Color.FromArgb(255, 36, 0));
      colors.Add(Color.FromArgb(255, 41, 0));
      colors.Add(Color.FromArgb(255, 45, 0));
      colors.Add(Color.FromArgb(255, 51, 0));
      colors.Add(Color.FromArgb(255, 57, 0));
      colors.Add(Color.FromArgb(255, 62, 0));
      colors.Add(Color.FromArgb(255, 68, 0));
      colors.Add(Color.FromArgb(255, 74, 0));
      colors.Add(Color.FromArgb(255, 81, 0));
      colors.Add(Color.FromArgb(255, 86, 0));
      colors.Add(Color.FromArgb(255, 93, 0));
      colors.Add(Color.FromArgb(255, 99, 0));
      colors.Add(Color.FromArgb(255, 105, 0));
      colors.Add(Color.FromArgb(255, 111, 0));
      colors.Add(Color.FromArgb(255, 118, 0));
      colors.Add(Color.FromArgb(255, 124, 0));
      colors.Add(Color.FromArgb(255, 131, 0));
      colors.Add(Color.FromArgb(255, 137, 0));
      colors.Add(Color.FromArgb(255, 144, 0));
      colors.Add(Color.FromArgb(255, 150, 0));
      colors.Add(Color.FromArgb(255, 156, 0));
      colors.Add(Color.FromArgb(255, 163, 0));
      colors.Add(Color.FromArgb(255, 169, 0));
      colors.Add(Color.FromArgb(255, 175, 0));
      colors.Add(Color.FromArgb(255, 181, 0));
      colors.Add(Color.FromArgb(255, 187, 0));
      colors.Add(Color.FromArgb(255, 192, 0));
      colors.Add(Color.FromArgb(255, 198, 0));
      colors.Add(Color.FromArgb(255, 203, 0));
      colors.Add(Color.FromArgb(255, 208, 0));
      colors.Add(Color.FromArgb(255, 213, 0));
      colors.Add(Color.FromArgb(255, 218, 0));
      colors.Add(Color.FromArgb(255, 222, 0));
      colors.Add(Color.FromArgb(255, 227, 0));
      colors.Add(Color.FromArgb(255, 232, 0));
      colors.Add(Color.FromArgb(255, 235, 0));
      colors.Add(Color.FromArgb(255, 238, 0));
      colors.Add(Color.FromArgb(255, 242, 0));
      colors.Add(Color.FromArgb(255, 245, 0));
      colors.Add(Color.FromArgb(255, 247, 0));
      colors.Add(Color.FromArgb(255, 250, 0));
      colors.Add(Color.FromArgb(255, 251, 0));
      colors.Add(Color.FromArgb(253, 252, 0));
      colors.Add(Color.FromArgb(250, 252, 1));
      colors.Add(Color.FromArgb(248, 252, 2));
      colors.Add(Color.FromArgb(244, 252, 2));
      colors.Add(Color.FromArgb(241, 252, 3));
      colors.Add(Color.FromArgb(237, 252, 3));
      colors.Add(Color.FromArgb(233, 252, 3));
      colors.Add(Color.FromArgb(229, 252, 4));
      colors.Add(Color.FromArgb(225, 252, 4));
      colors.Add(Color.FromArgb(220, 252, 5));
      colors.Add(Color.FromArgb(216, 252, 5));
      colors.Add(Color.FromArgb(211, 252, 6));
      colors.Add(Color.FromArgb(206, 252, 7));
      colors.Add(Color.FromArgb(201, 252, 7));
      colors.Add(Color.FromArgb(197, 252, 8));
      colors.Add(Color.FromArgb(191, 251, 8));
      colors.Add(Color.FromArgb(185, 249, 9));
      colors.Add(Color.FromArgb(180, 247, 9));
      colors.Add(Color.FromArgb(174, 246, 10));
      colors.Add(Color.FromArgb(169, 244, 11));
      colors.Add(Color.FromArgb(164, 242, 11));
      colors.Add(Color.FromArgb(158, 240, 12));
      colors.Add(Color.FromArgb(151, 238, 13));
      colors.Add(Color.FromArgb(146, 236, 14));
      colors.Add(Color.FromArgb(140, 233, 14));
      colors.Add(Color.FromArgb(134, 231, 15));
      colors.Add(Color.FromArgb(128, 228, 16));
      colors.Add(Color.FromArgb(122, 226, 17));
      colors.Add(Color.FromArgb(116, 223, 18));
      colors.Add(Color.FromArgb(110, 221, 19));
      colors.Add(Color.FromArgb(105, 218, 20));
      colors.Add(Color.FromArgb(99, 216, 21));
      colors.Add(Color.FromArgb(93, 214, 22));
      colors.Add(Color.FromArgb(88, 211, 23));
      colors.Add(Color.FromArgb(82, 209, 24));
      colors.Add(Color.FromArgb(76, 207, 25));
      colors.Add(Color.FromArgb(71, 204, 26));
      colors.Add(Color.FromArgb(66, 202, 28));
      colors.Add(Color.FromArgb(60, 200, 30));
      colors.Add(Color.FromArgb(55, 198, 31));
      colors.Add(Color.FromArgb(50, 196, 33));
      colors.Add(Color.FromArgb(45, 194, 34));
      colors.Add(Color.FromArgb(40, 191, 35));
      colors.Add(Color.FromArgb(36, 190, 37));
      colors.Add(Color.FromArgb(31, 188, 39));
      colors.Add(Color.FromArgb(27, 187, 40));
      colors.Add(Color.FromArgb(23, 185, 43));
      colors.Add(Color.FromArgb(19, 184, 44));
      colors.Add(Color.FromArgb(15, 183, 46));
      colors.Add(Color.FromArgb(12, 182, 48));
      colors.Add(Color.FromArgb(9, 181, 51));
      colors.Add(Color.FromArgb(6, 181, 53));
      colors.Add(Color.FromArgb(3, 180, 55));
      colors.Add(Color.FromArgb(1, 180, 57));
      colors.Add(Color.FromArgb(0, 180, 60));
      colors.Add(Color.FromArgb(0, 180, 62));
      colors.Add(Color.FromArgb(0, 180, 65));
      colors.Add(Color.FromArgb(0, 181, 68));
      colors.Add(Color.FromArgb(0, 182, 70));
      colors.Add(Color.FromArgb(0, 182, 74));
      colors.Add(Color.FromArgb(0, 183, 77));
      colors.Add(Color.FromArgb(0, 184, 80));
      colors.Add(Color.FromArgb(0, 184, 84));
      colors.Add(Color.FromArgb(0, 186, 88));
      colors.Add(Color.FromArgb(0, 187, 92));
      colors.Add(Color.FromArgb(0, 188, 95));
      colors.Add(Color.FromArgb(0, 190, 99));
      colors.Add(Color.FromArgb(0, 191, 104));
      colors.Add(Color.FromArgb(0, 193, 108));
      colors.Add(Color.FromArgb(0, 194, 112));
      colors.Add(Color.FromArgb(0, 196, 116));
      colors.Add(Color.FromArgb(0, 198, 120));
      colors.Add(Color.FromArgb(0, 200, 125));
      colors.Add(Color.FromArgb(0, 201, 129));
      colors.Add(Color.FromArgb(0, 203, 134));
      colors.Add(Color.FromArgb(0, 205, 138));
      colors.Add(Color.FromArgb(0, 207, 143));
      colors.Add(Color.FromArgb(0, 209, 147));
      colors.Add(Color.FromArgb(0, 211, 151));
      colors.Add(Color.FromArgb(0, 213, 156));
      colors.Add(Color.FromArgb(0, 215, 160));
      colors.Add(Color.FromArgb(0, 216, 165));
      colors.Add(Color.FromArgb(0, 219, 171));
      colors.Add(Color.FromArgb(0, 222, 178));
      colors.Add(Color.FromArgb(0, 224, 184));
      colors.Add(Color.FromArgb(0, 227, 190));
      colors.Add(Color.FromArgb(0, 229, 197));
      colors.Add(Color.FromArgb(0, 231, 203));
      colors.Add(Color.FromArgb(0, 233, 209));
      colors.Add(Color.FromArgb(0, 234, 214));
      colors.Add(Color.FromArgb(0, 234, 220));
      colors.Add(Color.FromArgb(0, 234, 225));
      colors.Add(Color.FromArgb(0, 234, 230));
      colors.Add(Color.FromArgb(0, 234, 234));
      colors.Add(Color.FromArgb(0, 234, 238));
      colors.Add(Color.FromArgb(0, 234, 242));
      colors.Add(Color.FromArgb(0, 234, 246));
      colors.Add(Color.FromArgb(0, 234, 248));
      colors.Add(Color.FromArgb(0, 234, 251));
      colors.Add(Color.FromArgb(0, 234, 254));
      colors.Add(Color.FromArgb(0, 234, 255));
      colors.Add(Color.FromArgb(0, 232, 255));
      colors.Add(Color.FromArgb(0, 228, 255));
      colors.Add(Color.FromArgb(0, 224, 255));
      colors.Add(Color.FromArgb(0, 219, 255));
      colors.Add(Color.FromArgb(0, 214, 254));
      colors.Add(Color.FromArgb(0, 208, 252));
      colors.Add(Color.FromArgb(0, 202, 250));
      colors.Add(Color.FromArgb(0, 195, 247));
      colors.Add(Color.FromArgb(0, 188, 244));
      colors.Add(Color.FromArgb(0, 180, 240));
      colors.Add(Color.FromArgb(0, 173, 236));
      colors.Add(Color.FromArgb(0, 164, 232));
      colors.Add(Color.FromArgb(0, 156, 228));
      colors.Add(Color.FromArgb(0, 147, 222));
      colors.Add(Color.FromArgb(0, 139, 218));
      colors.Add(Color.FromArgb(0, 130, 213));
      colors.Add(Color.FromArgb(0, 122, 208));
      colors.Add(Color.FromArgb(0, 117, 205));
      colors.Add(Color.FromArgb(0, 112, 203));
      colors.Add(Color.FromArgb(0, 107, 199));
      colors.Add(Color.FromArgb(0, 99, 196));
      colors.Add(Color.FromArgb(0, 93, 193));
      colors.Add(Color.FromArgb(0, 86, 189));
      colors.Add(Color.FromArgb(0, 78, 184));
      colors.Add(Color.FromArgb(0, 71, 180));
      colors.Add(Color.FromArgb(0, 65, 175));
      colors.Add(Color.FromArgb(0, 58, 171));
      colors.Add(Color.FromArgb(0, 52, 167));
      colors.Add(Color.FromArgb(0, 46, 162));
      colors.Add(Color.FromArgb(0, 40, 157));
      colors.Add(Color.FromArgb(0, 35, 152));
      colors.Add(Color.FromArgb(0, 30, 147));
      colors.Add(Color.FromArgb(0, 26, 142));
      colors.Add(Color.FromArgb(0, 22, 136));
      colors.Add(Color.FromArgb(0, 18, 131));
      colors.Add(Color.FromArgb(0, 15, 126));
      colors.Add(Color.FromArgb(0, 12, 120));
      colors.Add(Color.FromArgb(0, 9, 115));
      colors.Add(Color.FromArgb(1, 8, 110));
      colors.Add(Color.FromArgb(1, 6, 106));
      colors.Add(Color.FromArgb(1, 5, 101));
      colors.Add(Color.FromArgb(2, 4, 97));
      colors.Add(Color.FromArgb(3, 4, 92));
      colors.Add(Color.FromArgb(4, 5, 89));
      colors.Add(Color.FromArgb(5, 5, 85));
      colors.Add(Color.FromArgb(6, 6, 82));
      colors.Add(Color.FromArgb(7, 7, 79));
      colors.Add(Color.FromArgb(8, 8, 77));
      colors.Add(Color.FromArgb(10, 10, 77));
      colors.Add(Color.FromArgb(12, 12, 77));
      colors.Add(Color.FromArgb(14, 14, 76));
      colors.Add(Color.FromArgb(16, 16, 74));
      colors.Add(Color.FromArgb(19, 19, 73));
      colors.Add(Color.FromArgb(21, 21, 72));
      colors.Add(Color.FromArgb(24, 24, 71));
      colors.Add(Color.FromArgb(26, 26, 69));
      colors.Add(Color.FromArgb(29, 29, 70));
      colors.Add(Color.FromArgb(32, 32, 69));
      colors.Add(Color.FromArgb(35, 35, 68));
      colors.Add(Color.FromArgb(37, 37, 67));

      return colors;
    }

    /// <summary>
    /// This method will return a list of colors that are associated with the 
    /// "Fire" palette. These colors will create a fire heatmap look and 
    /// feel.
    /// </summary>
    /// <returns>A list of "fire" color objects</returns>
    public static List<Color> GetFirePalette()
    {
      List<Color> colors = new List<Color>();

      colors.Add(Color.FromArgb(255, 255, 255));
      colors.Add(Color.FromArgb(255, 255, 253));
      colors.Add(Color.FromArgb(255, 255, 250));
      colors.Add(Color.FromArgb(255, 255, 247));
      colors.Add(Color.FromArgb(255, 255, 244));
      colors.Add(Color.FromArgb(255, 255, 241));
      colors.Add(Color.FromArgb(255, 255, 238));
      colors.Add(Color.FromArgb(255, 255, 234));
      colors.Add(Color.FromArgb(255, 255, 231));
      colors.Add(Color.FromArgb(255, 255, 227));
      colors.Add(Color.FromArgb(255, 255, 223));
      colors.Add(Color.FromArgb(255, 255, 219));
      colors.Add(Color.FromArgb(255, 255, 214));
      colors.Add(Color.FromArgb(255, 255, 211));
      colors.Add(Color.FromArgb(255, 255, 206));
      colors.Add(Color.FromArgb(255, 255, 202));
      colors.Add(Color.FromArgb(255, 255, 197));
      colors.Add(Color.FromArgb(255, 255, 192));
      colors.Add(Color.FromArgb(255, 255, 187));
      colors.Add(Color.FromArgb(255, 255, 183));
      colors.Add(Color.FromArgb(255, 255, 178));
      colors.Add(Color.FromArgb(255, 255, 172));
      colors.Add(Color.FromArgb(255, 255, 167));
      colors.Add(Color.FromArgb(255, 255, 163));
      colors.Add(Color.FromArgb(255, 255, 157));
      colors.Add(Color.FromArgb(255, 255, 152));
      colors.Add(Color.FromArgb(255, 255, 147));
      colors.Add(Color.FromArgb(255, 255, 142));
      colors.Add(Color.FromArgb(255, 255, 136));
      colors.Add(Color.FromArgb(255, 255, 132));
      colors.Add(Color.FromArgb(255, 255, 126));
      colors.Add(Color.FromArgb(255, 255, 121));
      colors.Add(Color.FromArgb(255, 255, 116));
      colors.Add(Color.FromArgb(255, 255, 111));
      colors.Add(Color.FromArgb(255, 255, 106));
      colors.Add(Color.FromArgb(255, 255, 102));
      colors.Add(Color.FromArgb(255, 255, 97));
      colors.Add(Color.FromArgb(255, 255, 91));
      colors.Add(Color.FromArgb(255, 255, 87));
      colors.Add(Color.FromArgb(255, 255, 82));
      colors.Add(Color.FromArgb(255, 255, 78));
      colors.Add(Color.FromArgb(255, 255, 74));
      colors.Add(Color.FromArgb(255, 255, 70));
      colors.Add(Color.FromArgb(255, 255, 65));
      colors.Add(Color.FromArgb(255, 255, 61));
      colors.Add(Color.FromArgb(255, 255, 57));
      colors.Add(Color.FromArgb(255, 255, 53));
      colors.Add(Color.FromArgb(255, 255, 50));
      colors.Add(Color.FromArgb(255, 255, 46));
      colors.Add(Color.FromArgb(255, 255, 43));
      colors.Add(Color.FromArgb(255, 255, 39));
      colors.Add(Color.FromArgb(255, 255, 38));
      colors.Add(Color.FromArgb(255, 255, 34));
      colors.Add(Color.FromArgb(255, 255, 31));
      colors.Add(Color.FromArgb(255, 255, 29));
      colors.Add(Color.FromArgb(255, 255, 26));
      colors.Add(Color.FromArgb(255, 255, 25));
      colors.Add(Color.FromArgb(255, 254, 23));
      colors.Add(Color.FromArgb(255, 251, 22));
      colors.Add(Color.FromArgb(255, 250, 22));
      colors.Add(Color.FromArgb(255, 247, 23));
      colors.Add(Color.FromArgb(255, 245, 23));
      colors.Add(Color.FromArgb(255, 242, 24));
      colors.Add(Color.FromArgb(255, 239, 24));
      colors.Add(Color.FromArgb(255, 236, 25));
      colors.Add(Color.FromArgb(255, 232, 25));
      colors.Add(Color.FromArgb(255, 229, 26));
      colors.Add(Color.FromArgb(255, 226, 26));
      colors.Add(Color.FromArgb(255, 222, 27));
      colors.Add(Color.FromArgb(255, 218, 27));
      colors.Add(Color.FromArgb(255, 215, 28));
      colors.Add(Color.FromArgb(255, 210, 28));
      colors.Add(Color.FromArgb(255, 207, 29));
      colors.Add(Color.FromArgb(255, 203, 29));
      colors.Add(Color.FromArgb(255, 199, 30));
      colors.Add(Color.FromArgb(255, 194, 30));
      colors.Add(Color.FromArgb(255, 190, 31));
      colors.Add(Color.FromArgb(255, 186, 31));
      colors.Add(Color.FromArgb(255, 182, 32));
      colors.Add(Color.FromArgb(255, 176, 32));
      colors.Add(Color.FromArgb(255, 172, 33));
      colors.Add(Color.FromArgb(255, 168, 34));
      colors.Add(Color.FromArgb(255, 163, 34));
      colors.Add(Color.FromArgb(255, 159, 35));
      colors.Add(Color.FromArgb(255, 154, 35));
      colors.Add(Color.FromArgb(255, 150, 36));
      colors.Add(Color.FromArgb(255, 145, 36));
      colors.Add(Color.FromArgb(255, 141, 37));
      colors.Add(Color.FromArgb(255, 136, 37));
      colors.Add(Color.FromArgb(255, 132, 38));
      colors.Add(Color.FromArgb(255, 128, 39));
      colors.Add(Color.FromArgb(255, 124, 39));
      colors.Add(Color.FromArgb(255, 119, 40));
      colors.Add(Color.FromArgb(255, 115, 40));
      colors.Add(Color.FromArgb(255, 111, 41));
      colors.Add(Color.FromArgb(255, 107, 41));
      colors.Add(Color.FromArgb(255, 103, 42));
      colors.Add(Color.FromArgb(255, 99, 42));
      colors.Add(Color.FromArgb(255, 95, 43));
      colors.Add(Color.FromArgb(255, 92, 44));
      colors.Add(Color.FromArgb(255, 89, 44));
      colors.Add(Color.FromArgb(255, 85, 45));
      colors.Add(Color.FromArgb(255, 81, 45));
      colors.Add(Color.FromArgb(255, 79, 46));
      colors.Add(Color.FromArgb(255, 76, 47));
      colors.Add(Color.FromArgb(255, 72, 47));
      colors.Add(Color.FromArgb(255, 70, 48));
      colors.Add(Color.FromArgb(255, 67, 48));
      colors.Add(Color.FromArgb(255, 65, 49));
      colors.Add(Color.FromArgb(255, 63, 50));
      colors.Add(Color.FromArgb(255, 60, 50));
      colors.Add(Color.FromArgb(255, 59, 51));
      colors.Add(Color.FromArgb(255, 57, 51));
      colors.Add(Color.FromArgb(255, 55, 52));
      colors.Add(Color.FromArgb(255, 55, 53));
      colors.Add(Color.FromArgb(255, 53, 53));
      colors.Add(Color.FromArgb(253, 54, 54));
      colors.Add(Color.FromArgb(253, 54, 54));
      colors.Add(Color.FromArgb(251, 55, 55));
      colors.Add(Color.FromArgb(250, 56, 56));
      colors.Add(Color.FromArgb(248, 56, 56));
      colors.Add(Color.FromArgb(247, 57, 57));
      colors.Add(Color.FromArgb(246, 57, 57));
      colors.Add(Color.FromArgb(244, 58, 58));
      colors.Add(Color.FromArgb(242, 59, 59));
      colors.Add(Color.FromArgb(240, 59, 59));
      colors.Add(Color.FromArgb(239, 60, 60));
      colors.Add(Color.FromArgb(238, 61, 61));
      colors.Add(Color.FromArgb(235, 61, 61));
      colors.Add(Color.FromArgb(234, 62, 62));
      colors.Add(Color.FromArgb(232, 62, 62));
      colors.Add(Color.FromArgb(229, 63, 63));
      colors.Add(Color.FromArgb(228, 64, 64));
      colors.Add(Color.FromArgb(226, 64, 64));
      colors.Add(Color.FromArgb(224, 65, 65));
      colors.Add(Color.FromArgb(222, 66, 66));
      colors.Add(Color.FromArgb(219, 66, 66));
      colors.Add(Color.FromArgb(218, 67, 67));
      colors.Add(Color.FromArgb(216, 67, 67));
      colors.Add(Color.FromArgb(213, 68, 68));
      colors.Add(Color.FromArgb(211, 69, 69));
      colors.Add(Color.FromArgb(209, 69, 69));
      colors.Add(Color.FromArgb(207, 70, 70));
      colors.Add(Color.FromArgb(205, 71, 71));
      colors.Add(Color.FromArgb(203, 71, 71));
      colors.Add(Color.FromArgb(200, 72, 72));
      colors.Add(Color.FromArgb(199, 73, 73));
      colors.Add(Color.FromArgb(196, 73, 73));
      colors.Add(Color.FromArgb(194, 74, 74));
      colors.Add(Color.FromArgb(192, 74, 74));
      colors.Add(Color.FromArgb(190, 75, 75));
      colors.Add(Color.FromArgb(188, 76, 76));
      colors.Add(Color.FromArgb(186, 76, 76));
      colors.Add(Color.FromArgb(183, 77, 77));
      colors.Add(Color.FromArgb(181, 78, 78));
      colors.Add(Color.FromArgb(179, 78, 78));
      colors.Add(Color.FromArgb(177, 79, 79));
      colors.Add(Color.FromArgb(175, 80, 80));
      colors.Add(Color.FromArgb(173, 80, 80));
      colors.Add(Color.FromArgb(170, 81, 81));
      colors.Add(Color.FromArgb(169, 82, 82));
      colors.Add(Color.FromArgb(166, 82, 82));
      colors.Add(Color.FromArgb(165, 83, 83));
      colors.Add(Color.FromArgb(162, 83, 83));
      colors.Add(Color.FromArgb(160, 84, 84));
      colors.Add(Color.FromArgb(158, 85, 85));
      colors.Add(Color.FromArgb(156, 85, 85));
      colors.Add(Color.FromArgb(154, 86, 86));
      colors.Add(Color.FromArgb(153, 87, 87));
      colors.Add(Color.FromArgb(150, 87, 87));
      colors.Add(Color.FromArgb(149, 88, 88));
      colors.Add(Color.FromArgb(147, 89, 89));
      colors.Add(Color.FromArgb(146, 90, 90));
      colors.Add(Color.FromArgb(144, 91, 91));
      colors.Add(Color.FromArgb(142, 92, 92));
      colors.Add(Color.FromArgb(142, 94, 94));
      colors.Add(Color.FromArgb(141, 95, 95));
      colors.Add(Color.FromArgb(140, 96, 96));
      colors.Add(Color.FromArgb(139, 98, 98));
      colors.Add(Color.FromArgb(138, 99, 99));
      colors.Add(Color.FromArgb(136, 100, 100));
      colors.Add(Color.FromArgb(135, 101, 101));
      colors.Add(Color.FromArgb(135, 103, 103));
      colors.Add(Color.FromArgb(134, 104, 104));
      colors.Add(Color.FromArgb(133, 105, 105));
      colors.Add(Color.FromArgb(133, 107, 107));
      colors.Add(Color.FromArgb(132, 108, 108));
      colors.Add(Color.FromArgb(131, 109, 109));
      colors.Add(Color.FromArgb(132, 111, 111));
      colors.Add(Color.FromArgb(131, 112, 112));
      colors.Add(Color.FromArgb(130, 113, 113));
      colors.Add(Color.FromArgb(130, 114, 114));
      colors.Add(Color.FromArgb(130, 116, 116));
      colors.Add(Color.FromArgb(130, 117, 117));
      colors.Add(Color.FromArgb(130, 118, 118));
      colors.Add(Color.FromArgb(129, 119, 119));
      colors.Add(Color.FromArgb(130, 121, 121));
      colors.Add(Color.FromArgb(130, 122, 122));
      colors.Add(Color.FromArgb(130, 123, 123));
      colors.Add(Color.FromArgb(130, 124, 124));
      colors.Add(Color.FromArgb(131, 126, 126));
      colors.Add(Color.FromArgb(131, 127, 127));
      colors.Add(Color.FromArgb(130, 128, 128));
      colors.Add(Color.FromArgb(131, 129, 129));
      colors.Add(Color.FromArgb(132, 131, 131));
      colors.Add(Color.FromArgb(132, 132, 132));
      colors.Add(Color.FromArgb(133, 133, 133));
      colors.Add(Color.FromArgb(134, 134, 134));
      colors.Add(Color.FromArgb(135, 135, 135));
      colors.Add(Color.FromArgb(136, 136, 136));
      colors.Add(Color.FromArgb(138, 138, 138));
      colors.Add(Color.FromArgb(139, 139, 139));
      colors.Add(Color.FromArgb(140, 140, 140));
      colors.Add(Color.FromArgb(141, 141, 141));
      colors.Add(Color.FromArgb(142, 142, 142));
      colors.Add(Color.FromArgb(143, 143, 143));
      colors.Add(Color.FromArgb(144, 144, 144));
      colors.Add(Color.FromArgb(145, 145, 145));
      colors.Add(Color.FromArgb(147, 147, 147));
      colors.Add(Color.FromArgb(148, 148, 148));
      colors.Add(Color.FromArgb(149, 149, 149));
      colors.Add(Color.FromArgb(150, 150, 150));
      colors.Add(Color.FromArgb(151, 151, 151));
      colors.Add(Color.FromArgb(152, 152, 152));
      colors.Add(Color.FromArgb(153, 153, 153));
      colors.Add(Color.FromArgb(154, 154, 154));
      colors.Add(Color.FromArgb(155, 155, 155));
      colors.Add(Color.FromArgb(156, 156, 156));
      colors.Add(Color.FromArgb(157, 157, 157));
      colors.Add(Color.FromArgb(158, 158, 158));
      colors.Add(Color.FromArgb(159, 159, 159));
      colors.Add(Color.FromArgb(160, 160, 160));
      colors.Add(Color.FromArgb(160, 160, 160));
      colors.Add(Color.FromArgb(161, 161, 161));
      colors.Add(Color.FromArgb(162, 162, 162));
      colors.Add(Color.FromArgb(163, 163, 163));
      colors.Add(Color.FromArgb(164, 164, 164));
      colors.Add(Color.FromArgb(165, 165, 165));
      colors.Add(Color.FromArgb(166, 166, 166));
      colors.Add(Color.FromArgb(167, 167, 167));
      colors.Add(Color.FromArgb(167, 167, 167));
      colors.Add(Color.FromArgb(168, 168, 168));
      colors.Add(Color.FromArgb(169, 169, 169));
      colors.Add(Color.FromArgb(170, 170, 170));
      colors.Add(Color.FromArgb(170, 170, 170));
      colors.Add(Color.FromArgb(171, 171, 171));
      colors.Add(Color.FromArgb(172, 172, 172));
      colors.Add(Color.FromArgb(173, 173, 173));
      colors.Add(Color.FromArgb(173, 173, 173));
      colors.Add(Color.FromArgb(174, 174, 174));
      colors.Add(Color.FromArgb(175, 175, 175));
      colors.Add(Color.FromArgb(175, 175, 175));
      colors.Add(Color.FromArgb(176, 176, 176));
      colors.Add(Color.FromArgb(176, 176, 176));
      colors.Add(Color.FromArgb(177, 177, 177));
      colors.Add(Color.FromArgb(177, 177, 177));

      return colors;
    }

    /// <summary>
    /// This method will return a list of colors that are associated with the 
    /// "OMG" palette. These colors will create an OMG heatmap look and feel.
    /// </summary>
    /// <returns>A list of "omg" color objects</returns>
    public static List<Color> GetOmgPalette()
    {
      List<Color> colors = new List<Color>();

      colors.Add(Color.FromArgb(255, 255, 255));
      colors.Add(Color.FromArgb(255, 254, 254));
      colors.Add(Color.FromArgb(255, 253, 253));
      colors.Add(Color.FromArgb(255, 251, 251));
      colors.Add(Color.FromArgb(255, 250, 250));
      colors.Add(Color.FromArgb(255, 249, 249));
      colors.Add(Color.FromArgb(255, 247, 247));
      colors.Add(Color.FromArgb(255, 246, 246));
      colors.Add(Color.FromArgb(255, 244, 244));
      colors.Add(Color.FromArgb(255, 242, 242));
      colors.Add(Color.FromArgb(255, 241, 241));
      colors.Add(Color.FromArgb(255, 239, 239));
      colors.Add(Color.FromArgb(255, 237, 237));
      colors.Add(Color.FromArgb(255, 235, 235));
      colors.Add(Color.FromArgb(255, 233, 233));
      colors.Add(Color.FromArgb(255, 231, 231));
      colors.Add(Color.FromArgb(255, 229, 229));
      colors.Add(Color.FromArgb(255, 227, 227));
      colors.Add(Color.FromArgb(255, 226, 226));
      colors.Add(Color.FromArgb(255, 224, 224));
      colors.Add(Color.FromArgb(255, 222, 222));
      colors.Add(Color.FromArgb(255, 220, 220));
      colors.Add(Color.FromArgb(255, 217, 217));
      colors.Add(Color.FromArgb(255, 215, 215));
      colors.Add(Color.FromArgb(255, 213, 213));
      colors.Add(Color.FromArgb(255, 210, 210));
      colors.Add(Color.FromArgb(255, 208, 208));
      colors.Add(Color.FromArgb(255, 206, 206));
      colors.Add(Color.FromArgb(255, 204, 204));
      colors.Add(Color.FromArgb(255, 202, 202));
      colors.Add(Color.FromArgb(255, 199, 199));
      colors.Add(Color.FromArgb(255, 197, 197));
      colors.Add(Color.FromArgb(255, 194, 194));
      colors.Add(Color.FromArgb(255, 192, 192));
      colors.Add(Color.FromArgb(255, 189, 189));
      colors.Add(Color.FromArgb(255, 188, 188));
      colors.Add(Color.FromArgb(255, 185, 185));
      colors.Add(Color.FromArgb(255, 183, 183));
      colors.Add(Color.FromArgb(255, 180, 180));
      colors.Add(Color.FromArgb(255, 178, 178));
      colors.Add(Color.FromArgb(255, 176, 176));
      colors.Add(Color.FromArgb(255, 173, 173));
      colors.Add(Color.FromArgb(255, 171, 171));
      colors.Add(Color.FromArgb(255, 169, 169));
      colors.Add(Color.FromArgb(255, 167, 167));
      colors.Add(Color.FromArgb(255, 164, 164));
      colors.Add(Color.FromArgb(255, 162, 162));
      colors.Add(Color.FromArgb(255, 160, 160));
      colors.Add(Color.FromArgb(255, 158, 158));
      colors.Add(Color.FromArgb(255, 155, 155));
      colors.Add(Color.FromArgb(255, 153, 153));
      colors.Add(Color.FromArgb(255, 151, 151));
      colors.Add(Color.FromArgb(255, 149, 149));
      colors.Add(Color.FromArgb(255, 147, 147));
      colors.Add(Color.FromArgb(255, 145, 145));
      colors.Add(Color.FromArgb(255, 143, 143));
      colors.Add(Color.FromArgb(255, 141, 141));
      colors.Add(Color.FromArgb(255, 139, 139));
      colors.Add(Color.FromArgb(255, 137, 137));
      colors.Add(Color.FromArgb(255, 136, 136));
      colors.Add(Color.FromArgb(255, 134, 134));
      colors.Add(Color.FromArgb(255, 132, 132));
      colors.Add(Color.FromArgb(255, 131, 131));
      colors.Add(Color.FromArgb(255, 129, 129));
      colors.Add(Color.FromArgb(255, 128, 128));
      colors.Add(Color.FromArgb(255, 127, 127));
      colors.Add(Color.FromArgb(255, 127, 127));
      colors.Add(Color.FromArgb(255, 126, 126));
      colors.Add(Color.FromArgb(255, 125, 125));
      colors.Add(Color.FromArgb(255, 125, 125));
      colors.Add(Color.FromArgb(255, 124, 124));
      colors.Add(Color.FromArgb(255, 123, 122));
      colors.Add(Color.FromArgb(255, 123, 122));
      colors.Add(Color.FromArgb(255, 122, 121));
      colors.Add(Color.FromArgb(255, 122, 121));
      colors.Add(Color.FromArgb(255, 121, 120));
      colors.Add(Color.FromArgb(255, 120, 119));
      colors.Add(Color.FromArgb(255, 119, 118));
      colors.Add(Color.FromArgb(255, 119, 118));
      colors.Add(Color.FromArgb(255, 118, 116));
      colors.Add(Color.FromArgb(255, 117, 116));
      colors.Add(Color.FromArgb(255, 117, 115));
      colors.Add(Color.FromArgb(255, 115, 114));
      colors.Add(Color.FromArgb(255, 115, 114));
      colors.Add(Color.FromArgb(255, 114, 113));
      colors.Add(Color.FromArgb(255, 114, 112));
      colors.Add(Color.FromArgb(255, 113, 111));
      colors.Add(Color.FromArgb(255, 113, 111));
      colors.Add(Color.FromArgb(255, 112, 110));
      colors.Add(Color.FromArgb(255, 111, 108));
      colors.Add(Color.FromArgb(255, 111, 108));
      colors.Add(Color.FromArgb(255, 110, 107));
      colors.Add(Color.FromArgb(255, 110, 107));
      colors.Add(Color.FromArgb(255, 109, 105));
      colors.Add(Color.FromArgb(255, 109, 105));
      colors.Add(Color.FromArgb(255, 108, 104));
      colors.Add(Color.FromArgb(255, 107, 104));
      colors.Add(Color.FromArgb(255, 107, 102));
      colors.Add(Color.FromArgb(255, 106, 102));
      colors.Add(Color.FromArgb(255, 106, 101));
      colors.Add(Color.FromArgb(255, 105, 101));
      colors.Add(Color.FromArgb(255, 104, 99));
      colors.Add(Color.FromArgb(255, 104, 99));
      colors.Add(Color.FromArgb(255, 103, 98));
      colors.Add(Color.FromArgb(255, 103, 98));
      colors.Add(Color.FromArgb(255, 102, 97));
      colors.Add(Color.FromArgb(255, 102, 96));
      colors.Add(Color.FromArgb(255, 101, 96));
      colors.Add(Color.FromArgb(255, 101, 96));
      colors.Add(Color.FromArgb(255, 100, 94));
      colors.Add(Color.FromArgb(255, 100, 94));
      colors.Add(Color.FromArgb(255, 99, 93));
      colors.Add(Color.FromArgb(255, 99, 92));
      colors.Add(Color.FromArgb(255, 98, 91));
      colors.Add(Color.FromArgb(255, 98, 91));
      colors.Add(Color.FromArgb(255, 97, 90));
      colors.Add(Color.FromArgb(255, 97, 89));
      colors.Add(Color.FromArgb(255, 96, 89));
      colors.Add(Color.FromArgb(255, 96, 89));
      colors.Add(Color.FromArgb(255, 95, 88));
      colors.Add(Color.FromArgb(255, 95, 88));
      colors.Add(Color.FromArgb(255, 94, 86));
      colors.Add(Color.FromArgb(255, 93, 86));
      colors.Add(Color.FromArgb(255, 93, 85));
      colors.Add(Color.FromArgb(255, 93, 85));
      colors.Add(Color.FromArgb(255, 92, 85));
      colors.Add(Color.FromArgb(255, 92, 84));
      colors.Add(Color.FromArgb(255, 91, 83));
      colors.Add(Color.FromArgb(255, 91, 83));
      colors.Add(Color.FromArgb(255, 90, 82));
      colors.Add(Color.FromArgb(255, 90, 82));
      colors.Add(Color.FromArgb(255, 89, 81));
      colors.Add(Color.FromArgb(255, 89, 82));
      colors.Add(Color.FromArgb(255, 89, 80));
      colors.Add(Color.FromArgb(255, 89, 80));
      colors.Add(Color.FromArgb(255, 89, 79));
      colors.Add(Color.FromArgb(255, 89, 79));
      colors.Add(Color.FromArgb(255, 88, 79));
      colors.Add(Color.FromArgb(255, 88, 79));
      colors.Add(Color.FromArgb(255, 87, 78));
      colors.Add(Color.FromArgb(255, 87, 78));
      colors.Add(Color.FromArgb(255, 87, 78));
      colors.Add(Color.FromArgb(255, 87, 77));
      colors.Add(Color.FromArgb(255, 87, 77));
      colors.Add(Color.FromArgb(255, 86, 77));
      colors.Add(Color.FromArgb(255, 86, 77));
      colors.Add(Color.FromArgb(255, 85, 76));
      colors.Add(Color.FromArgb(255, 85, 76));
      colors.Add(Color.FromArgb(255, 85, 75));
      colors.Add(Color.FromArgb(255, 85, 76));
      colors.Add(Color.FromArgb(255, 85, 75));
      colors.Add(Color.FromArgb(255, 85, 76));
      colors.Add(Color.FromArgb(255, 84, 75));
      colors.Add(Color.FromArgb(255, 84, 75));
      colors.Add(Color.FromArgb(255, 84, 75));
      colors.Add(Color.FromArgb(255, 84, 75));
      colors.Add(Color.FromArgb(255, 85, 75));
      colors.Add(Color.FromArgb(255, 84, 75));
      colors.Add(Color.FromArgb(255, 84, 75));
      colors.Add(Color.FromArgb(255, 83, 74));
      colors.Add(Color.FromArgb(255, 83, 75));
      colors.Add(Color.FromArgb(255, 83, 75));
      colors.Add(Color.FromArgb(255, 84, 75));
      colors.Add(Color.FromArgb(255, 83, 75));
      colors.Add(Color.FromArgb(255, 83, 75));
      colors.Add(Color.FromArgb(255, 83, 75));
      colors.Add(Color.FromArgb(255, 83, 75));
      colors.Add(Color.FromArgb(255, 83, 76));
      colors.Add(Color.FromArgb(255, 83, 76));
      colors.Add(Color.FromArgb(255, 83, 76));
      colors.Add(Color.FromArgb(255, 83, 76));
      colors.Add(Color.FromArgb(255, 83, 76));
      colors.Add(Color.FromArgb(255, 83, 76));
      colors.Add(Color.FromArgb(255, 83, 76));
      colors.Add(Color.FromArgb(255, 83, 76));
      colors.Add(Color.FromArgb(255, 83, 77));
      colors.Add(Color.FromArgb(255, 84, 78));
      colors.Add(Color.FromArgb(255, 83, 78));
      colors.Add(Color.FromArgb(255, 84, 79));
      colors.Add(Color.FromArgb(255, 84, 78));
      colors.Add(Color.FromArgb(255, 84, 79));
      colors.Add(Color.FromArgb(255, 83, 79));
      colors.Add(Color.FromArgb(255, 84, 80));
      colors.Add(Color.FromArgb(255, 83, 80));
      colors.Add(Color.FromArgb(255, 84, 81));
      colors.Add(Color.FromArgb(255, 85, 82));
      colors.Add(Color.FromArgb(255, 85, 82));
      colors.Add(Color.FromArgb(255, 85, 83));
      colors.Add(Color.FromArgb(255, 85, 83));
      colors.Add(Color.FromArgb(255, 85, 84));
      colors.Add(Color.FromArgb(255, 85, 84));
      colors.Add(Color.FromArgb(255, 86, 85));
      colors.Add(Color.FromArgb(255, 86, 85));
      colors.Add(Color.FromArgb(255, 87, 87));
      colors.Add(Color.FromArgb(254, 89, 89));
      colors.Add(Color.FromArgb(254, 91, 92));
      colors.Add(Color.FromArgb(253, 92, 93));
      colors.Add(Color.FromArgb(252, 94, 96));
      colors.Add(Color.FromArgb(251, 96, 98));
      colors.Add(Color.FromArgb(251, 97, 100));
      colors.Add(Color.FromArgb(249, 99, 103));
      colors.Add(Color.FromArgb(249, 100, 105));
      colors.Add(Color.FromArgb(248, 102, 108));
      colors.Add(Color.FromArgb(247, 104, 111));
      colors.Add(Color.FromArgb(246, 105, 113));
      colors.Add(Color.FromArgb(245, 107, 116));
      colors.Add(Color.FromArgb(244, 109, 119));
      colors.Add(Color.FromArgb(243, 110, 122));
      colors.Add(Color.FromArgb(242, 112, 125));
      colors.Add(Color.FromArgb(241, 113, 127));
      colors.Add(Color.FromArgb(240, 115, 130));
      colors.Add(Color.FromArgb(239, 117, 134));
      colors.Add(Color.FromArgb(238, 118, 136));
      colors.Add(Color.FromArgb(237, 120, 140));
      colors.Add(Color.FromArgb(236, 121, 142));
      colors.Add(Color.FromArgb(235, 123, 145));
      colors.Add(Color.FromArgb(234, 124, 148));
      colors.Add(Color.FromArgb(233, 126, 151));
      colors.Add(Color.FromArgb(232, 127, 154));
      colors.Add(Color.FromArgb(232, 129, 157));
      colors.Add(Color.FromArgb(230, 130, 159));
      colors.Add(Color.FromArgb(230, 132, 162));
      colors.Add(Color.FromArgb(229, 133, 165));
      colors.Add(Color.FromArgb(228, 135, 168));
      colors.Add(Color.FromArgb(227, 136, 170));
      colors.Add(Color.FromArgb(227, 138, 173));
      colors.Add(Color.FromArgb(226, 139, 176));
      colors.Add(Color.FromArgb(225, 140, 178));
      colors.Add(Color.FromArgb(224, 142, 181));
      colors.Add(Color.FromArgb(223, 143, 183));
      colors.Add(Color.FromArgb(223, 144, 185));
      colors.Add(Color.FromArgb(223, 146, 188));
      colors.Add(Color.FromArgb(222, 147, 190));
      colors.Add(Color.FromArgb(221, 148, 192));
      colors.Add(Color.FromArgb(221, 150, 195));
      colors.Add(Color.FromArgb(220, 151, 197));
      colors.Add(Color.FromArgb(219, 152, 199));
      colors.Add(Color.FromArgb(219, 153, 201));
      colors.Add(Color.FromArgb(219, 154, 202));
      colors.Add(Color.FromArgb(219, 156, 205));
      colors.Add(Color.FromArgb(218, 157, 207));
      colors.Add(Color.FromArgb(217, 158, 208));
      colors.Add(Color.FromArgb(217, 159, 210));
      colors.Add(Color.FromArgb(217, 160, 211));
      colors.Add(Color.FromArgb(217, 161, 213));
      colors.Add(Color.FromArgb(216, 162, 214));
      colors.Add(Color.FromArgb(216, 163, 216));
      colors.Add(Color.FromArgb(216, 164, 217));
      colors.Add(Color.FromArgb(215, 165, 218));
      colors.Add(Color.FromArgb(216, 166, 219));
      colors.Add(Color.FromArgb(215, 166, 220));
      colors.Add(Color.FromArgb(215, 167, 222));
      colors.Add(Color.FromArgb(215, 168, 223));
      colors.Add(Color.FromArgb(215, 169, 223));
      colors.Add(Color.FromArgb(215, 170, 224));
      colors.Add(Color.FromArgb(215, 170, 225));

      return colors;
    }

    /// <summary>
    /// This method will return a list of colors that are associated with the 
    /// "Fire" palette. These colors will create a pbj heatmap look and feel.
    /// </summary>
    /// <returns>A list of "pbj" color objects</returns>
    public static List<Color> GetPbjPalette()
    {
      List<Color> colors = new List<Color>();

      colors.Add(Color.FromArgb(41, 10, 89));
      colors.Add(Color.FromArgb(41, 10, 89));
      colors.Add(Color.FromArgb(42, 10, 89));
      colors.Add(Color.FromArgb(42, 10, 89));
      colors.Add(Color.FromArgb(42, 10, 88));
      colors.Add(Color.FromArgb(43, 10, 88));
      colors.Add(Color.FromArgb(43, 9, 88));
      colors.Add(Color.FromArgb(43, 9, 88));
      colors.Add(Color.FromArgb(44, 9, 88));
      colors.Add(Color.FromArgb(44, 9, 88));
      colors.Add(Color.FromArgb(45, 10, 89));
      colors.Add(Color.FromArgb(46, 10, 88));
      colors.Add(Color.FromArgb(46, 9, 88));
      colors.Add(Color.FromArgb(47, 9, 88));
      colors.Add(Color.FromArgb(47, 9, 88));
      colors.Add(Color.FromArgb(47, 9, 88));
      colors.Add(Color.FromArgb(48, 8, 88));
      colors.Add(Color.FromArgb(48, 8, 87));
      colors.Add(Color.FromArgb(49, 8, 87));
      colors.Add(Color.FromArgb(49, 8, 87));
      colors.Add(Color.FromArgb(49, 7, 87));
      colors.Add(Color.FromArgb(50, 7, 87));
      colors.Add(Color.FromArgb(50, 7, 87));
      colors.Add(Color.FromArgb(51, 7, 86));
      colors.Add(Color.FromArgb(51, 6, 86));
      colors.Add(Color.FromArgb(53, 7, 86));
      colors.Add(Color.FromArgb(53, 7, 86));
      colors.Add(Color.FromArgb(54, 7, 86));
      colors.Add(Color.FromArgb(54, 6, 85));
      colors.Add(Color.FromArgb(55, 6, 85));
      colors.Add(Color.FromArgb(55, 6, 85));
      colors.Add(Color.FromArgb(56, 5, 85));
      colors.Add(Color.FromArgb(56, 5, 85));
      colors.Add(Color.FromArgb(57, 5, 84));
      colors.Add(Color.FromArgb(57, 5, 84));
      colors.Add(Color.FromArgb(58, 4, 84));
      colors.Add(Color.FromArgb(59, 4, 84));
      colors.Add(Color.FromArgb(59, 5, 84));
      colors.Add(Color.FromArgb(60, 4, 84));
      colors.Add(Color.FromArgb(60, 4, 84));
      colors.Add(Color.FromArgb(61, 4, 84));
      colors.Add(Color.FromArgb(61, 4, 83));
      colors.Add(Color.FromArgb(62, 3, 83));
      colors.Add(Color.FromArgb(63, 3, 83));
      colors.Add(Color.FromArgb(63, 3, 83));
      colors.Add(Color.FromArgb(64, 3, 82));
      colors.Add(Color.FromArgb(64, 3, 82));
      colors.Add(Color.FromArgb(65, 3, 82));
      colors.Add(Color.FromArgb(66, 3, 82));
      colors.Add(Color.FromArgb(67, 4, 82));
      colors.Add(Color.FromArgb(68, 4, 82));
      colors.Add(Color.FromArgb(69, 4, 82));
      colors.Add(Color.FromArgb(69, 4, 81));
      colors.Add(Color.FromArgb(70, 4, 81));
      colors.Add(Color.FromArgb(71, 4, 81));
      colors.Add(Color.FromArgb(71, 4, 80));
      colors.Add(Color.FromArgb(72, 4, 80));
      colors.Add(Color.FromArgb(73, 4, 80));
      colors.Add(Color.FromArgb(73, 4, 79));
      colors.Add(Color.FromArgb(75, 5, 80));
      colors.Add(Color.FromArgb(76, 5, 80));
      colors.Add(Color.FromArgb(77, 5, 79));
      colors.Add(Color.FromArgb(77, 5, 79));
      colors.Add(Color.FromArgb(78, 5, 79));
      colors.Add(Color.FromArgb(79, 5, 78));
      colors.Add(Color.FromArgb(80, 5, 78));
      colors.Add(Color.FromArgb(80, 5, 78));
      colors.Add(Color.FromArgb(80, 5, 77));
      colors.Add(Color.FromArgb(81, 5, 77));
      colors.Add(Color.FromArgb(83, 6, 76));
      colors.Add(Color.FromArgb(83, 6, 76));
      colors.Add(Color.FromArgb(84, 6, 76));
      colors.Add(Color.FromArgb(85, 6, 75));
      colors.Add(Color.FromArgb(86, 6, 75));
      colors.Add(Color.FromArgb(87, 6, 74));
      colors.Add(Color.FromArgb(88, 6, 74));
      colors.Add(Color.FromArgb(88, 6, 73));
      colors.Add(Color.FromArgb(89, 6, 73));
      colors.Add(Color.FromArgb(91, 7, 73));
      colors.Add(Color.FromArgb(92, 7, 73));
      colors.Add(Color.FromArgb(93, 7, 72));
      colors.Add(Color.FromArgb(94, 7, 72));
      colors.Add(Color.FromArgb(94, 7, 71));
      colors.Add(Color.FromArgb(95, 7, 71));
      colors.Add(Color.FromArgb(96, 7, 70));
      colors.Add(Color.FromArgb(96, 7, 70));
      colors.Add(Color.FromArgb(97, 7, 69));
      colors.Add(Color.FromArgb(99, 9, 70));
      colors.Add(Color.FromArgb(100, 9, 69));
      colors.Add(Color.FromArgb(101, 10, 69));
      colors.Add(Color.FromArgb(102, 10, 68));
      colors.Add(Color.FromArgb(103, 11, 67));
      colors.Add(Color.FromArgb(104, 11, 67));
      colors.Add(Color.FromArgb(105, 12, 66));
      colors.Add(Color.FromArgb(106, 13, 66));
      colors.Add(Color.FromArgb(107, 14, 66));
      colors.Add(Color.FromArgb(108, 15, 65));
      colors.Add(Color.FromArgb(109, 16, 64));
      colors.Add(Color.FromArgb(110, 16, 64));
      colors.Add(Color.FromArgb(111, 17, 63));
      colors.Add(Color.FromArgb(112, 18, 62));
      colors.Add(Color.FromArgb(113, 18, 61));
      colors.Add(Color.FromArgb(114, 19, 61));
      colors.Add(Color.FromArgb(115, 20, 60));
      colors.Add(Color.FromArgb(118, 22, 60));
      colors.Add(Color.FromArgb(119, 22, 59));
      colors.Add(Color.FromArgb(120, 22, 58));
      colors.Add(Color.FromArgb(120, 23, 58));
      colors.Add(Color.FromArgb(121, 24, 57));
      colors.Add(Color.FromArgb(122, 25, 56));
      colors.Add(Color.FromArgb(124, 26, 55));
      colors.Add(Color.FromArgb(125, 27, 54));
      colors.Add(Color.FromArgb(127, 29, 54));
      colors.Add(Color.FromArgb(128, 30, 54));
      colors.Add(Color.FromArgb(130, 31, 53));
      colors.Add(Color.FromArgb(131, 32, 52));
      colors.Add(Color.FromArgb(132, 33, 51));
      colors.Add(Color.FromArgb(133, 34, 50));
      colors.Add(Color.FromArgb(134, 35, 49));
      colors.Add(Color.FromArgb(135, 36, 48));
      colors.Add(Color.FromArgb(137, 38, 48));
      colors.Add(Color.FromArgb(138, 39, 47));
      colors.Add(Color.FromArgb(140, 40, 46));
      colors.Add(Color.FromArgb(141, 41, 46));
      colors.Add(Color.FromArgb(142, 42, 45));
      colors.Add(Color.FromArgb(143, 42, 44));
      colors.Add(Color.FromArgb(144, 43, 43));
      colors.Add(Color.FromArgb(145, 44, 42));
      colors.Add(Color.FromArgb(146, 45, 42));
      colors.Add(Color.FromArgb(149, 47, 41));
      colors.Add(Color.FromArgb(150, 48, 41));
      colors.Add(Color.FromArgb(151, 49, 40));
      colors.Add(Color.FromArgb(152, 50, 39));
      colors.Add(Color.FromArgb(153, 51, 38));
      colors.Add(Color.FromArgb(154, 52, 38));
      colors.Add(Color.FromArgb(155, 53, 37));
      colors.Add(Color.FromArgb(157, 55, 36));
      colors.Add(Color.FromArgb(159, 57, 36));
      colors.Add(Color.FromArgb(160, 57, 35));
      colors.Add(Color.FromArgb(160, 58, 34));
      colors.Add(Color.FromArgb(162, 59, 33));
      colors.Add(Color.FromArgb(163, 60, 33));
      colors.Add(Color.FromArgb(164, 61, 32));
      colors.Add(Color.FromArgb(165, 62, 31));
      colors.Add(Color.FromArgb(167, 63, 30));
      colors.Add(Color.FromArgb(168, 65, 30));
      colors.Add(Color.FromArgb(169, 66, 29));
      colors.Add(Color.FromArgb(170, 67, 29));
      colors.Add(Color.FromArgb(172, 68, 28));
      colors.Add(Color.FromArgb(173, 69, 27));
      colors.Add(Color.FromArgb(174, 70, 26));
      colors.Add(Color.FromArgb(175, 71, 26));
      colors.Add(Color.FromArgb(176, 71, 25));
      colors.Add(Color.FromArgb(178, 73, 25));
      colors.Add(Color.FromArgb(179, 74, 24));
      colors.Add(Color.FromArgb(180, 75, 24));
      colors.Add(Color.FromArgb(181, 76, 23));
      colors.Add(Color.FromArgb(182, 77, 23));
      colors.Add(Color.FromArgb(183, 78, 23));
      colors.Add(Color.FromArgb(184, 79, 22));
      colors.Add(Color.FromArgb(186, 80, 22));
      colors.Add(Color.FromArgb(187, 81, 21));
      colors.Add(Color.FromArgb(188, 82, 21));
      colors.Add(Color.FromArgb(189, 83, 21));
      colors.Add(Color.FromArgb(190, 83, 20));
      colors.Add(Color.FromArgb(191, 84, 20));
      colors.Add(Color.FromArgb(192, 85, 19));
      colors.Add(Color.FromArgb(192, 86, 19));
      colors.Add(Color.FromArgb(193, 87, 18));
      colors.Add(Color.FromArgb(194, 87, 18));
      colors.Add(Color.FromArgb(196, 89, 18));
      colors.Add(Color.FromArgb(196, 90, 18));
      colors.Add(Color.FromArgb(197, 90, 18));
      colors.Add(Color.FromArgb(198, 90, 18));
      colors.Add(Color.FromArgb(199, 91, 18));
      colors.Add(Color.FromArgb(200, 92, 18));
      colors.Add(Color.FromArgb(201, 93, 18));
      colors.Add(Color.FromArgb(202, 93, 18));
      colors.Add(Color.FromArgb(203, 94, 18));
      colors.Add(Color.FromArgb(204, 96, 19));
      colors.Add(Color.FromArgb(204, 96, 19));
      colors.Add(Color.FromArgb(205, 97, 19));
      colors.Add(Color.FromArgb(206, 98, 19));
      colors.Add(Color.FromArgb(207, 99, 19));
      colors.Add(Color.FromArgb(208, 99, 19));
      colors.Add(Color.FromArgb(209, 100, 19));
      colors.Add(Color.FromArgb(210, 100, 19));
      colors.Add(Color.FromArgb(211, 100, 19));
      colors.Add(Color.FromArgb(212, 102, 20));
      colors.Add(Color.FromArgb(213, 103, 20));
      colors.Add(Color.FromArgb(214, 103, 20));
      colors.Add(Color.FromArgb(214, 104, 20));
      colors.Add(Color.FromArgb(215, 105, 20));
      colors.Add(Color.FromArgb(215, 105, 20));
      colors.Add(Color.FromArgb(216, 106, 20));
      colors.Add(Color.FromArgb(217, 107, 20));
      colors.Add(Color.FromArgb(218, 107, 20));
      colors.Add(Color.FromArgb(219, 108, 20));
      colors.Add(Color.FromArgb(220, 109, 21));
      colors.Add(Color.FromArgb(221, 109, 21));
      colors.Add(Color.FromArgb(222, 110, 21));
      colors.Add(Color.FromArgb(222, 111, 21));
      colors.Add(Color.FromArgb(223, 111, 21));
      colors.Add(Color.FromArgb(224, 112, 21));
      colors.Add(Color.FromArgb(225, 113, 21));
      colors.Add(Color.FromArgb(226, 113, 21));
      colors.Add(Color.FromArgb(227, 114, 21));
      colors.Add(Color.FromArgb(227, 114, 21));
      colors.Add(Color.FromArgb(228, 115, 22));
      colors.Add(Color.FromArgb(229, 116, 22));
      colors.Add(Color.FromArgb(229, 116, 22));
      colors.Add(Color.FromArgb(230, 117, 22));
      colors.Add(Color.FromArgb(231, 117, 22));
      colors.Add(Color.FromArgb(231, 118, 22));
      colors.Add(Color.FromArgb(232, 119, 22));
      colors.Add(Color.FromArgb(233, 119, 22));
      colors.Add(Color.FromArgb(234, 120, 22));
      colors.Add(Color.FromArgb(234, 120, 22));
      colors.Add(Color.FromArgb(235, 121, 22));
      colors.Add(Color.FromArgb(236, 121, 22));
      colors.Add(Color.FromArgb(237, 122, 23));
      colors.Add(Color.FromArgb(237, 122, 23));
      colors.Add(Color.FromArgb(238, 123, 23));
      colors.Add(Color.FromArgb(239, 124, 23));
      colors.Add(Color.FromArgb(239, 124, 23));
      colors.Add(Color.FromArgb(240, 125, 23));
      colors.Add(Color.FromArgb(240, 125, 23));
      colors.Add(Color.FromArgb(241, 126, 23));
      colors.Add(Color.FromArgb(241, 126, 23));
      colors.Add(Color.FromArgb(242, 127, 23));
      colors.Add(Color.FromArgb(243, 127, 23));
      colors.Add(Color.FromArgb(243, 128, 23));
      colors.Add(Color.FromArgb(244, 128, 24));
      colors.Add(Color.FromArgb(244, 128, 24));
      colors.Add(Color.FromArgb(245, 129, 24));
      colors.Add(Color.FromArgb(246, 129, 24));
      colors.Add(Color.FromArgb(246, 130, 24));
      colors.Add(Color.FromArgb(247, 130, 24));
      colors.Add(Color.FromArgb(247, 131, 24));
      colors.Add(Color.FromArgb(248, 131, 24));
      colors.Add(Color.FromArgb(249, 131, 24));
      colors.Add(Color.FromArgb(249, 132, 24));
      colors.Add(Color.FromArgb(250, 132, 24));
      colors.Add(Color.FromArgb(250, 133, 24));
      colors.Add(Color.FromArgb(250, 133, 24));
      colors.Add(Color.FromArgb(250, 133, 24));
      colors.Add(Color.FromArgb(251, 134, 24));
      colors.Add(Color.FromArgb(251, 134, 25));
      colors.Add(Color.FromArgb(252, 135, 25));
      colors.Add(Color.FromArgb(252, 135, 25));
      colors.Add(Color.FromArgb(253, 135, 25));
      colors.Add(Color.FromArgb(253, 136, 25));
      colors.Add(Color.FromArgb(253, 136, 25));
      colors.Add(Color.FromArgb(254, 136, 25));
      colors.Add(Color.FromArgb(254, 136, 25));
      colors.Add(Color.FromArgb(255, 137, 25));

      return colors;
    }

    /// <summary>
    /// This method will return a list of colors that are associated with the 
    /// "Fire" palette. These colors will create a pgaitch heatmap look and feel.
    /// </summary>
    /// <returns>A list of "pgaitch" color objects</returns>
    public static List<Color> GetPgaitchPalette()
    {
      List<Color> colors = new List<Color>();

      colors.Add(Color.FromArgb(255, 254, 165));
      colors.Add(Color.FromArgb(255, 254, 164));
      colors.Add(Color.FromArgb(255, 253, 163));
      colors.Add(Color.FromArgb(255, 253, 162));
      colors.Add(Color.FromArgb(255, 253, 161));
      colors.Add(Color.FromArgb(255, 252, 160));
      colors.Add(Color.FromArgb(255, 252, 159));
      colors.Add(Color.FromArgb(255, 252, 157));
      colors.Add(Color.FromArgb(255, 251, 156));
      colors.Add(Color.FromArgb(255, 251, 155));
      colors.Add(Color.FromArgb(255, 251, 153));
      colors.Add(Color.FromArgb(255, 250, 152));
      colors.Add(Color.FromArgb(255, 250, 150));
      colors.Add(Color.FromArgb(255, 250, 149));
      colors.Add(Color.FromArgb(255, 249, 148));
      colors.Add(Color.FromArgb(255, 249, 146));
      colors.Add(Color.FromArgb(255, 249, 145));
      colors.Add(Color.FromArgb(255, 248, 143));
      colors.Add(Color.FromArgb(255, 248, 141));
      colors.Add(Color.FromArgb(255, 248, 139));
      colors.Add(Color.FromArgb(255, 247, 138));
      colors.Add(Color.FromArgb(255, 247, 136));
      colors.Add(Color.FromArgb(255, 246, 134));
      colors.Add(Color.FromArgb(255, 246, 132));
      colors.Add(Color.FromArgb(255, 246, 130));
      colors.Add(Color.FromArgb(255, 245, 129));
      colors.Add(Color.FromArgb(255, 245, 127));
      colors.Add(Color.FromArgb(255, 245, 125));
      colors.Add(Color.FromArgb(255, 244, 123));
      colors.Add(Color.FromArgb(255, 244, 121));
      colors.Add(Color.FromArgb(255, 243, 119));
      colors.Add(Color.FromArgb(255, 243, 117));
      colors.Add(Color.FromArgb(255, 242, 114));
      colors.Add(Color.FromArgb(255, 242, 112));
      colors.Add(Color.FromArgb(255, 241, 111));
      colors.Add(Color.FromArgb(255, 241, 109));
      colors.Add(Color.FromArgb(255, 240, 107));
      colors.Add(Color.FromArgb(255, 240, 105));
      colors.Add(Color.FromArgb(255, 239, 102));
      colors.Add(Color.FromArgb(255, 239, 100));
      colors.Add(Color.FromArgb(255, 238, 99));
      colors.Add(Color.FromArgb(255, 238, 97));
      colors.Add(Color.FromArgb(255, 237, 95));
      colors.Add(Color.FromArgb(255, 237, 92));
      colors.Add(Color.FromArgb(255, 236, 90));
      colors.Add(Color.FromArgb(255, 237, 89));
      colors.Add(Color.FromArgb(255, 236, 87));
      colors.Add(Color.FromArgb(255, 235, 84));
      colors.Add(Color.FromArgb(255, 235, 82));
      colors.Add(Color.FromArgb(255, 234, 80));
      colors.Add(Color.FromArgb(255, 233, 79));
      colors.Add(Color.FromArgb(255, 233, 77));
      colors.Add(Color.FromArgb(255, 232, 74));
      colors.Add(Color.FromArgb(255, 231, 72));
      colors.Add(Color.FromArgb(255, 230, 70));
      colors.Add(Color.FromArgb(255, 230, 69));
      colors.Add(Color.FromArgb(255, 229, 67));
      colors.Add(Color.FromArgb(255, 228, 65));
      colors.Add(Color.FromArgb(255, 227, 63));
      colors.Add(Color.FromArgb(255, 226, 61));
      colors.Add(Color.FromArgb(255, 225, 60));
      colors.Add(Color.FromArgb(255, 225, 58));
      colors.Add(Color.FromArgb(255, 224, 56));
      colors.Add(Color.FromArgb(255, 223, 54));
      colors.Add(Color.FromArgb(255, 222, 52));
      colors.Add(Color.FromArgb(255, 222, 51));
      colors.Add(Color.FromArgb(255, 221, 49));
      colors.Add(Color.FromArgb(255, 220, 47));
      colors.Add(Color.FromArgb(255, 219, 46));
      colors.Add(Color.FromArgb(255, 218, 44));
      colors.Add(Color.FromArgb(255, 216, 43));
      colors.Add(Color.FromArgb(255, 215, 42));
      colors.Add(Color.FromArgb(255, 214, 41));
      colors.Add(Color.FromArgb(255, 213, 39));
      colors.Add(Color.FromArgb(255, 212, 39));
      colors.Add(Color.FromArgb(255, 211, 37));
      colors.Add(Color.FromArgb(255, 209, 36));
      colors.Add(Color.FromArgb(255, 208, 34));
      colors.Add(Color.FromArgb(255, 208, 33));
      colors.Add(Color.FromArgb(255, 206, 33));
      colors.Add(Color.FromArgb(255, 205, 32));
      colors.Add(Color.FromArgb(255, 204, 30));
      colors.Add(Color.FromArgb(255, 202, 29));
      colors.Add(Color.FromArgb(255, 201, 29));
      colors.Add(Color.FromArgb(255, 199, 28));
      colors.Add(Color.FromArgb(254, 199, 28));
      colors.Add(Color.FromArgb(254, 199, 27));
      colors.Add(Color.FromArgb(253, 198, 27));
      colors.Add(Color.FromArgb(252, 197, 27));
      colors.Add(Color.FromArgb(251, 196, 27));
      colors.Add(Color.FromArgb(250, 195, 26));
      colors.Add(Color.FromArgb(249, 195, 26));
      colors.Add(Color.FromArgb(248, 194, 26));
      colors.Add(Color.FromArgb(248, 193, 26));
      colors.Add(Color.FromArgb(247, 192, 26));
      colors.Add(Color.FromArgb(246, 192, 25));
      colors.Add(Color.FromArgb(245, 191, 26));
      colors.Add(Color.FromArgb(244, 190, 26));
      colors.Add(Color.FromArgb(243, 189, 25));
      colors.Add(Color.FromArgb(241, 188, 25));
      colors.Add(Color.FromArgb(240, 187, 25));
      colors.Add(Color.FromArgb(239, 187, 25));
      colors.Add(Color.FromArgb(238, 186, 25));
      colors.Add(Color.FromArgb(236, 185, 25));
      colors.Add(Color.FromArgb(236, 184, 26));
      colors.Add(Color.FromArgb(235, 183, 26));
      colors.Add(Color.FromArgb(233, 182, 25));
      colors.Add(Color.FromArgb(232, 181, 25));
      colors.Add(Color.FromArgb(230, 181, 26));
      colors.Add(Color.FromArgb(229, 180, 26));
      colors.Add(Color.FromArgb(228, 179, 25));
      colors.Add(Color.FromArgb(227, 178, 25));
      colors.Add(Color.FromArgb(226, 177, 26));
      colors.Add(Color.FromArgb(224, 176, 26));
      colors.Add(Color.FromArgb(222, 176, 25));
      colors.Add(Color.FromArgb(221, 175, 25));
      colors.Add(Color.FromArgb(220, 173, 26));
      colors.Add(Color.FromArgb(219, 172, 26));
      colors.Add(Color.FromArgb(217, 171, 25));
      colors.Add(Color.FromArgb(215, 170, 25));
      colors.Add(Color.FromArgb(214, 170, 26));
      colors.Add(Color.FromArgb(212, 169, 26));
      colors.Add(Color.FromArgb(211, 167, 25));
      colors.Add(Color.FromArgb(209, 166, 25));
      colors.Add(Color.FromArgb(208, 166, 26));
      colors.Add(Color.FromArgb(206, 165, 26));
      colors.Add(Color.FromArgb(204, 163, 26));
      colors.Add(Color.FromArgb(203, 162, 26));
      colors.Add(Color.FromArgb(202, 161, 25));
      colors.Add(Color.FromArgb(200, 161, 26));
      colors.Add(Color.FromArgb(198, 159, 26));
      colors.Add(Color.FromArgb(197, 158, 26));
      colors.Add(Color.FromArgb(195, 157, 26));
      colors.Add(Color.FromArgb(193, 157, 27));
      colors.Add(Color.FromArgb(192, 155, 27));
      colors.Add(Color.FromArgb(190, 154, 27));
      colors.Add(Color.FromArgb(189, 153, 27));
      colors.Add(Color.FromArgb(187, 152, 28));
      colors.Add(Color.FromArgb(186, 151, 28));
      colors.Add(Color.FromArgb(184, 150, 28));
      colors.Add(Color.FromArgb(182, 149, 28));
      colors.Add(Color.FromArgb(181, 148, 29));
      colors.Add(Color.FromArgb(179, 147, 29));
      colors.Add(Color.FromArgb(177, 146, 29));
      colors.Add(Color.FromArgb(175, 144, 29));
      colors.Add(Color.FromArgb(174, 144, 30));
      colors.Add(Color.FromArgb(172, 142, 30));
      colors.Add(Color.FromArgb(170, 141, 30));
      colors.Add(Color.FromArgb(169, 140, 30));
      colors.Add(Color.FromArgb(167, 139, 31));
      colors.Add(Color.FromArgb(165, 138, 31));
      colors.Add(Color.FromArgb(164, 137, 31));
      colors.Add(Color.FromArgb(162, 136, 31));
      colors.Add(Color.FromArgb(161, 135, 32));
      colors.Add(Color.FromArgb(159, 134, 32));
      colors.Add(Color.FromArgb(157, 133, 32));
      colors.Add(Color.FromArgb(154, 132, 32));
      colors.Add(Color.FromArgb(153, 131, 33));
      colors.Add(Color.FromArgb(151, 130, 33));
      colors.Add(Color.FromArgb(150, 129, 33));
      colors.Add(Color.FromArgb(148, 127, 33));
      colors.Add(Color.FromArgb(147, 127, 34));
      colors.Add(Color.FromArgb(145, 126, 34));
      colors.Add(Color.FromArgb(143, 124, 34));
      colors.Add(Color.FromArgb(141, 123, 34));
      colors.Add(Color.FromArgb(140, 122, 35));
      colors.Add(Color.FromArgb(139, 121, 35));
      colors.Add(Color.FromArgb(137, 120, 35));
      colors.Add(Color.FromArgb(135, 119, 35));
      colors.Add(Color.FromArgb(134, 118, 36));
      colors.Add(Color.FromArgb(132, 117, 36));
      colors.Add(Color.FromArgb(130, 116, 36));
      colors.Add(Color.FromArgb(129, 115, 36));
      colors.Add(Color.FromArgb(127, 113, 36));
      colors.Add(Color.FromArgb(126, 113, 37));
      colors.Add(Color.FromArgb(124, 112, 37));
      colors.Add(Color.FromArgb(122, 111, 37));
      colors.Add(Color.FromArgb(121, 110, 37));
      colors.Add(Color.FromArgb(120, 109, 38));
      colors.Add(Color.FromArgb(118, 108, 38));
      colors.Add(Color.FromArgb(116, 107, 38));
      colors.Add(Color.FromArgb(115, 105, 38));
      colors.Add(Color.FromArgb(113, 104, 38));
      colors.Add(Color.FromArgb(112, 104, 39));
      colors.Add(Color.FromArgb(110, 103, 39));
      colors.Add(Color.FromArgb(108, 102, 39));
      colors.Add(Color.FromArgb(107, 101, 39));
      colors.Add(Color.FromArgb(106, 100, 40));
      colors.Add(Color.FromArgb(104, 99, 40));
      colors.Add(Color.FromArgb(102, 98, 40));
      colors.Add(Color.FromArgb(101, 96, 40));
      colors.Add(Color.FromArgb(99, 96, 40));
      colors.Add(Color.FromArgb(99, 96, 41));
      colors.Add(Color.FromArgb(97, 94, 41));
      colors.Add(Color.FromArgb(96, 93, 41));
      colors.Add(Color.FromArgb(94, 92, 41));
      colors.Add(Color.FromArgb(92, 91, 41));
      colors.Add(Color.FromArgb(92, 90, 42));
      colors.Add(Color.FromArgb(90, 90, 42));
      colors.Add(Color.FromArgb(89, 89, 42));
      colors.Add(Color.FromArgb(87, 87, 42));
      colors.Add(Color.FromArgb(86, 86, 42));
      colors.Add(Color.FromArgb(85, 86, 43));
      colors.Add(Color.FromArgb(84, 85, 43));
      colors.Add(Color.FromArgb(83, 84, 43));
      colors.Add(Color.FromArgb(81, 83, 43));
      colors.Add(Color.FromArgb(80, 82, 43));
      colors.Add(Color.FromArgb(80, 82, 44));
      colors.Add(Color.FromArgb(78, 80, 44));
      colors.Add(Color.FromArgb(77, 80, 44));
      colors.Add(Color.FromArgb(75, 79, 44));
      colors.Add(Color.FromArgb(75, 78, 44));
      colors.Add(Color.FromArgb(74, 78, 45));
      colors.Add(Color.FromArgb(73, 76, 45));
      colors.Add(Color.FromArgb(71, 75, 45));
      colors.Add(Color.FromArgb(71, 75, 45));
      colors.Add(Color.FromArgb(70, 74, 45));
      colors.Add(Color.FromArgb(69, 74, 46));
      colors.Add(Color.FromArgb(68, 73, 46));
      colors.Add(Color.FromArgb(67, 72, 46));
      colors.Add(Color.FromArgb(66, 71, 46));
      colors.Add(Color.FromArgb(65, 71, 46));
      colors.Add(Color.FromArgb(64, 69, 46));
      colors.Add(Color.FromArgb(64, 69, 47));
      colors.Add(Color.FromArgb(63, 68, 47));
      colors.Add(Color.FromArgb(62, 67, 47));
      colors.Add(Color.FromArgb(61, 67, 47));
      colors.Add(Color.FromArgb(60, 66, 47));
      colors.Add(Color.FromArgb(59, 65, 47));
      colors.Add(Color.FromArgb(59, 65, 48));
      colors.Add(Color.FromArgb(59, 64, 48));
      colors.Add(Color.FromArgb(58, 63, 48));
      colors.Add(Color.FromArgb(57, 63, 48));
      colors.Add(Color.FromArgb(56, 62, 48));
      colors.Add(Color.FromArgb(56, 62, 48));
      colors.Add(Color.FromArgb(55, 61, 48));
      colors.Add(Color.FromArgb(55, 61, 49));
      colors.Add(Color.FromArgb(55, 60, 49));
      colors.Add(Color.FromArgb(55, 60, 49));
      colors.Add(Color.FromArgb(54, 59, 49));
      colors.Add(Color.FromArgb(53, 58, 49));
      colors.Add(Color.FromArgb(53, 57, 49));
      colors.Add(Color.FromArgb(52, 57, 49));
      colors.Add(Color.FromArgb(52, 57, 50));
      colors.Add(Color.FromArgb(52, 56, 50));
      colors.Add(Color.FromArgb(52, 56, 50));
      colors.Add(Color.FromArgb(52, 56, 50));
      colors.Add(Color.FromArgb(52, 55, 50));
      colors.Add(Color.FromArgb(51, 54, 50));
      colors.Add(Color.FromArgb(51, 53, 50));
      colors.Add(Color.FromArgb(51, 53, 50));
      colors.Add(Color.FromArgb(51, 52, 50));
      colors.Add(Color.FromArgb(51, 53, 51));
      colors.Add(Color.FromArgb(51, 53, 51));
      colors.Add(Color.FromArgb(51, 52, 51));
      colors.Add(Color.FromArgb(51, 52, 51));

      return colors;
    }

    /// <summary>
    /// This method will return a palette bitmap image for the given scheme.
    /// </summary>
    /// <param name="scheme">The palette scheme to get</param>
    /// <returns>The given palette scheme as a bitmap</returns>
    public static Bitmap GetPaletteAsBitmap(Scheme scheme)
    {
      // Get the requested palette
      List<Color> colours = GetPalette(scheme);

      // Create the bitmap
      Bitmap image = new Bitmap(colours.Count, 1);

      // Place each color in the same index location of the bitmap
      for (int x = 0; x < colours.Count; x++)
      {
        image.SetPixel(x, 0, colours[x]);
      }

      return image;
    }

    /// <summary>
    /// This method will return a palette image for the given scheme.
    /// </summary>
    /// <param name="scheme">The palette scheme to get</param>
    /// <returns>The given palette scheme as an image</returns>
    public static Image GetPaletteAsImage(Scheme scheme)
    {
      return (Image)GetPaletteAsBitmap(scheme);
    }

    #endregion
  }
}
