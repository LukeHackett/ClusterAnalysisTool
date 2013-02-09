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

using Analysis;
using Cluster;
using KML;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;

namespace ClusterAnalysisTool
{
  public class Program
  {
    private static String[] RequiredArguments = new String[] {"source", "output", "analysis"};
    private static String[] AnalysisOptions = new String[] { "week", "product" };

    public static void Main(string[] args)
    {
      // Get the input arguments
      StringDictionary arguments = Arguments.GetArguments(args);

      // Ensure that the minimum arguments have been passed
      if (!HasMinimumArguements(arguments))
      {
        // Error: minimum required arguments have not beed passed
        Console.WriteLine("ERROR: Not enough arugments have been passed.");
        Console.WriteLine("Please ensure the following parameters have been " +
                          "given:");
        PrintParamArray(RequiredArguments);
        Environment.Exit(1);
      }

      // Import all the files within the directory
      EventCollection collection = GetData(arguments["source"], SearchOption.TopDirectoryOnly);
      if (collection == null)
      {
        // Error: No files were found at the given directory
        Console.WriteLine("ERROR: No input KML files were found.");
        Console.WriteLine("Input directory: {0}", arguments["source"]);
        Environment.Exit(1);
      }

      // Obtain the String representation of the input value
      // Input value might be true, so ToString() method is used.
      String analysis = arguments["analysis"].ToString();

      // Perform the analysis based upon the input value
      if(analysis == "week")
      {
        // Create a Multi-Week Analysis object
        MultiWeekAnalysis multi = new MultiWeekAnalysis();
        multi.AddRange(collection);
        // Cluster the Multi-Week Data
        multi.Cluster();
        // Analyse the Multi-Week Data
        multi.AnalyseWeeks();
        // Save the results to the output directory

        foreach (DictionaryEntry entry in arguments)
        {
          Console.WriteLine("{0}    {1}", entry.Key, entry.Value);
        }

        Console.WriteLine("Dictionary contains output: {0}", arguments.ContainsKey("output"));
        Console.WriteLine(arguments["output"]);
        String s = arguments["output"].TrimEnd('\\') + @"\";

        multi.CreateWeeklyAnalysisJSON(s, false);

        Console.WriteLine("Done!!!!!!!!!!");
      }
      else if(analysis == "product")
      {

      }
      else
      {
        // Error: No valid analysis option was given
        Console.WriteLine("ERROR: No valid analysis option was given.");
        Console.WriteLine("A valid option is one of: ");
        PrintParamArray(AnalysisOptions);
        Environment.Exit(1);
      }


      Console.WriteLine("here");
    }

    /// <summary>
    /// This method will ensure that a given set of arguements meets the minimum 
    /// required arguements based upon the applciation preferences.
    /// </summary>
    /// <param name="arguments">A StringDictionary of the passed arguments</param>
    /// <returns>A boolean whether or not the arguments are correct</returns>
    private static bool HasMinimumArguements(StringDictionary arguments)
    {
      // Ensure that the there has been a source directory passed
      if (!arguments.ContainsKey("source"))
      {
        return false;
      }

      // Ensure that there has been an output directy passed
      if (!arguments.ContainsKey("output"))
      {
        return false;
      }

      // Ensure that there has been an output directy passed
      if (!arguments.ContainsKey("analysis"))
      {
        return false;
      }
      
      return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directory"></param>
    /// <param name="option"></param>
    /// <returns></returns>
    private static EventCollection GetData(String directory, SearchOption option)
    {
      EventCollection collection = new EventCollection();

      // Only continue if the input directory exists
      if (!Directory.Exists(directory))
      {
        return null;
      }

      // Get a list of all the KML file names within the directory
      String[] files = Directory.GetFiles(directory, "*.kml", option);
  
      // Loop over each of the file and add to the data
      foreach(String file in files)
      {
        // Parse the KML file
        KMLReader reader = new KMLReader(file);
        // Add the data to the known events
        collection.AddRange(reader.GetCallLogs());
      }

      return collection;
    }

    /// <summary>
    /// This method will print each String element found within an the given 
    /// String array to a new line within the Console.
    /// </summary>
    /// <param name="array">The String array to be printed</param>
    private static void PrintParamArray(String[] array)
    {
      // Loop over each parameter in the given array
      foreach (String param in array)
      {
        // Print the parameter usage in a formatted way
        Console.WriteLine("\t {0}", param);
      }

      // Print a whitespace line
      Console.WriteLine();
    }
  }
}
