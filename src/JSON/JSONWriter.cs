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

namespace JSON
{
  /// <summary>
  /// This class provides a number of static methods that allow various lists 
  /// of Strings to be correctly represented in JavaScript Object Notation, also 
  /// known as JSON.
  /// </summary>
  public static class JSONWriter
  {

    /// <summary>
    /// This method will create a well formed String representation of a JSON 
    /// array, based upon the given list of String elements. The key is the 
    /// name (or key) that is associated to the JSON array.
    /// </summary>
    /// <param name="key">The name of the array</param>
    /// <param name="elements">A list of string elements to form the array</param>
    /// <returns>A well formed string representing a JSON array</returns>
    public static String CreateJSONArray(String key, List<String> elements)
    {
      // Convert the String List to a String
      return CreateJSONArray(key, elements.ToArray());
    }

    /// <summary>
    /// This method will create a well formed String representation of a JSON 
    /// array, based upon the given list of String elements. The key is the 
    /// name (or key) that is associated to the JSON array.
    /// </summary>
    /// <param name="key">The name of the array</param>
    /// <param name="elements">An array of string elements to form the array</param>
    /// <returns>A well formed string representing a JSON array</returns>
    public static String CreateJSONArray(String key, params String[] elements)
    {
      // Convert the String array to a String
      return String.Format("\"{0}\": [{1}]", key, String.Join(",", elements));
    }
    
    /// <summary>
    /// This method will create a well formed JOSN key/value pair element.
    /// </summary>
    /// <param name="key">The associated key of the piar</param>
    /// <param name="value">The associated value of the pair</param>
    /// <returns>A well formed string representing a singluar key/value pair</returns>
    public static String CreateKeyValue(String key, String value)
    {
      return String.Format("\"{0}\": \"{1}\"", key, value);
    }

    /// <summary>
    /// This method will create a well formed String representation of a JSON 
    /// object. It is assumed that each value is a correct, well formed key/value 
    /// pair string element.
    /// </summary>
    /// <param name="values"></param>
    /// <returns>A well formed string representing a JSON Object</returns>
    public static String CreateJSONObject(params String[] values)
    {
      return "{ " + String.Join(", ", values) + " }";
    }
    
    /// <summary>
    /// This method will prettify a given JSON String, by ensuring that it is 
    /// human readable. This will mean that a JSON String will be spaned across 
    /// multiple lines rather than just over a single line.
    /// </summary>
    /// <param name="json">The JSON String to be prettified</param>
    /// <returns>A prettified JSON String</returns>
    public static String PrettifyJSON(String json)
    {
      // The charater to use as the indentation
      char INDENT_CHAR = ' ';
      // Stores the number of indentations are needed
      int indent = 0;
      // Whether or not the current character is within an quoted String
      bool quoted = false;
      // The String Builder object to create the final string
      StringBuilder sb = new StringBuilder();
      // Loop over the entire String
      for (var i = 0; i < json.Length; i++)
      {
        // Get the current character
        char ch = json[i];
        // Switch between the various String options
        switch (ch)
        {
          // Start a new Object or Array
          case '{':
          case '[':
            // Append the current character
            sb.Append(ch);
            // Create a new line if not currently in data
            if (!quoted)
            {
              // Add a new line
              sb.AppendLine();
              // Indent the next line indent-1 number of times
              sb.Append(new String(INDENT_CHAR, ++indent));
            }
            break;

            // End an Object or Array
            case '}':
            case ']':
              if (!quoted)
              {
                // Add a new line
                sb.AppendLine();
                // Indent the next line indent-1 number of times
                sb.Append(new String(INDENT_CHAR, --indent));
              }
              // Append the character
              sb.Append(ch);
              break;

            // Quoted Strings
            case '"':
              // Append the character
              sb.Append(ch);
              bool escaped = false;
              int index = i;
              // try to determin if there is another quote somewhere
              while (index > 0 && json[--index] == '\\')
              {
                escaped = !escaped;
              }
              if (!escaped)
              {
                quoted = !quoted;
              }
              break;

            // Classifying the next Key/Value pair
            case ',':
              // Append the current character
              sb.Append(ch);
              // Start a new line and indent if not in a quote
              if (!quoted)
              {
                // Add a new line
                sb.AppendLine();
                // Indent the next line indent-1 number of times
                sb.Append(new String(INDENT_CHAR, indent));
              }
              break;

            // For anything else just append
            default:
              sb.Append(ch);
              break;
        }
      }
      // return the final string 
      return sb.ToString();
    }
  }
}