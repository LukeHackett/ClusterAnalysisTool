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
    #region Properties

    /// <summary>
    /// An array of all the minimum required arguments for the tool to work
    /// </summary>
    private static String[] RequiredArguments = new String[] {"source", "output", "analysis"};

    /// <summary>
    /// An array of the various analysis options
    /// </summary>
    private static String[] AnalysisOptions = new String[] { "week", "product", "all" };

    /// <summary>
    /// A Dictionary of the arugments that were passed to the main method
    /// </summary>
    private static StringDictionary InputArguments;

    /// <summary>
    /// Default EPS value
    /// </summary>
    private const double EPS = 1.5;

    /// <summary>
    /// Default Minimum Points value to form a cluster
    /// </summary>
    private const int MIN_POINTS = 3;

    #endregion

    #region Public Static Methods

    /// <summary>
    /// The main entry point for the console application
    /// </summary>
    /// <param name="args">Additional program arguments</param>
    public static void Main(string[] args)
    {
      // Get the input arguments
      InputArguments = Arguments.GetArguments(args);

      // Print the help contents if requested
      if (InputArguments.ContainsKey("help"))
      {
        PrintHelp();
        Environment.Exit(0);
      }

      // Ensure that the minimum arguments have been passed
      if (!HasMinimumArguements())
      {
        // Error: minimum required arguments have not beed passed
        Console.WriteLine("ERROR: Not enough arugments have been passed.");
        Console.WriteLine("Please ensure the following parameters have been " +
                          "given:");
        PrintParamArray(RequiredArguments);
        // show the help for the user
        PrintHelp();
        Environment.Exit(1);
      }

      // Deduce if a file needs to be analysed or a directory
      // Get the attributes for file or directory
      FileAttributes fileattr = File.GetAttributes(InputArguments["source"]);

      // Detect whether its a directory or file
      if ((fileattr & FileAttributes.Directory) == FileAttributes.Directory)
      {
        // Input is a directory
        AnalyseDirectory();
      }
      else
      {
        // Input is a single file
        AnalyseFile();
      }
    }

    #endregion

    #region Private Static Methods

    /// <summary>
    /// This method will print usage instructions - more specifically how to 
    /// use this console application.
    /// </summary>
    private static void PrintHelp()
    {
      Console.WriteLine("");
      Console.WriteLine("Cluster Analysis Tool");
      Console.WriteLine("Copyright (c) BlackBerry 2013. All Rights Reserved.");
      Console.WriteLine("");
      Console.WriteLine("USAGE:");
      Console.WriteLine("\t --source=[value]");
      Console.WriteLine("\t\t The source directory or .KML file");
      Console.WriteLine("\t --output=[value]");
      Console.WriteLine("\t\t The analysis output directory");
      Console.WriteLine("\t --analysis=[option]");
      Console.WriteLine("\t\t The type of analysis to be performed. " + 
                        "option can be one of 'all', 'week' or 'product'");
      Console.WriteLine("");
      Console.WriteLine("\t --jsminify");
      Console.WriteLine("\t\t Minifies all JavaScript output (default is false)");
      Console.WriteLine("\t --eps=[value]");
      Console.WriteLine("\t\t The EPS value to be used (default is " + EPS + ")");
      Console.WriteLine("\t --min=[value]");
      Console.WriteLine("\t\t The minimum number of objects required to form " + 
                        "a cluster (default is " + MIN_POINTS + ").");
      Console.WriteLine("");
    }

    /// <summary>
    /// This method will ensure that a given set of arguements meets the minimum 
    /// required arguements based upon the applciation preferences.
    /// </summary>
    /// <param name="arguments">A StringDictionary of the passed arguments</param>
    /// <returns>A boolean whether or not the arguments are correct</returns>
    private static bool HasMinimumArguements()
    {
      // Ensure that the there has been a source directory passed
      if (!InputArguments.ContainsKey("source"))
      {
        return false;
      }

      // Ensure the source directory/file exists
      try
      {
        // Get the attributes for file or directory
        String arg = InputArguments["source"];
        FileAttributes fileattr = File.GetAttributes(InputArguments["source"]);
      }
      catch (FileNotFoundException ex)
      {
        Console.WriteLine("ERROR: The given input directory/file does not exist.");
        Console.WriteLine("Source: " +  ex.FileName);
        Environment.Exit(1);
      }

      // Ensure that there has been an output directory directy passed
      if (!InputArguments.ContainsKey("output"))
      {
        return false;
      }

      // Create the output directory if it doesn't exist
      String output = Path.GetDirectoryName(InputArguments["output"]);
      Directory.CreateDirectory(output);

      // Ensure that there has been an output directy passed
      if (!InputArguments.ContainsKey("analysis"))
      {
        return false;
      }
      
      return true;
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

    /// <summary>
    /// This method will analyse a given directory. It will firstly scan the 
    /// directoy for all kml files. The analysis will produce an output KML 
    /// file and a number of output JSON analysis files, depending upon the 
    /// runtime options supplied.
    /// </summary>
    private static void AnalyseDirectory()
    {
      String directory = InputArguments["source"];

      EventCollection collection = new EventCollection();

      // Only continue if the input directory exists
      if (!Directory.Exists(directory))
      {
        return;
      }

      // Get a list of all the KML file names within the directory
      String[] files = Directory.GetFiles(directory, "*.kml", SearchOption.TopDirectoryOnly);

      // Loop over each of the file and add to the data
      foreach (String file in files)
      {
        // Parse the KML file
        KMLReader reader = new KMLReader(file);
        // Add the data to the known events
        collection.AddRange(reader.GetCallLogs());
      }

      // Perform the analysis
      PerformAnalysis(collection);

    }

    /// <summary>
    /// This method will analyse the file that is currently stored within the 
    /// source key of the InputArguments dictionary.
    /// </summary>
    private static void AnalyseFile()
    {
      AnalyseFile(InputArguments["source"]);
    }

    /// <summary>
    /// This method will analyse a given kml file. The analysis will produce 
    /// an output KML file and a number of output JSON analysis files, 
    /// depending upon the runtime options supplied.
    /// </summary>
    /// <param name="file">The KML file to analyse</param>
    private static void AnalyseFile(String file)
    {
      // Ensure the file exists
      if (!File.Exists(file))
      {
        throw new FileNotFoundException("File does not exist", file);
      }

      // Read in all the known events from the input file
      KMLReader reader = new KMLReader(file);
      EventCollection collection = reader.GetCallLogs();

      // Perform the analysis
      PerformAnalysis(collection);
    }

    /// <summary>
    /// This method will cluster and analyse a given event collection. The type 
    /// of analysis that will be performed will depend upon the runtime 
    /// analysis options.
    /// </summary>
    /// <param name="collection">The EventCollection to be clustered and analysed</param>
    private static void PerformAnalysis(EventCollection collection)
    {
      // Analyse the data based upon the requirement
      switch (InputArguments["analysis"])
      {
        case "week":
          PerformWeekAnalysis(collection);
          break;

        case "product":
          PerformProductAnalysis(collection);
          break;

        case "all":
          PerformWeekAnalysis(collection);
          PerformProductAnalysis(collection);
          break;

        default:
          // Error: No valid analysis option was given
          Console.WriteLine("ERROR: No valid analysis option was given.");
          Console.WriteLine("A valid option is one of: ");
          PrintParamArray(AnalysisOptions);
          Environment.Exit(1);
          break;
      }
    }

    /// <summary>
    /// This method will cluster and analyse a given event collection based 
    /// upon a weekly approach.
    /// </summary>
    /// <param name="collection">The EventCollection to be clustered and analysed</param>
    private static void PerformWeekAnalysis(EventCollection collection)
    {
      // Output directory
      String output = InputArguments["output"];

      // Create a Multi-Week Analysis object
      MultiWeekAnalysis multi = new MultiWeekAnalysis();
      multi.AddRange(collection);

      // Obtain the EPS distance value
      double eps = GetEPSInput();

      // Obtain the Minimum Number of Objects per cluster value
      int min = GetMinPointsInput();

      // Cluster the Multi-Week Data
      multi.Cluster(eps, min);
      
      // Analyse the Multi-Week Data
      multi.AnalyseWeeks();

      // Output the KML file
      String kml_output = GenerateOutputName(".kmz");
      multi.GenerateKMZ(kml_output);
      
      // Save the results to the output directory
      bool minify = InputArguments.ContainsKey("jsminify") ? true : false;
      multi.CreateWeeklyAnalysisJSON(output, minify);
    }

    /// <summary>
    /// This method will cluster and analyse a given event collection based 
    /// upon a product approach.
    /// </summary>
    /// <param name="collection">The EventCollection to be clustered and analysed</param>
    private static void PerformProductAnalysis(EventCollection collection)
    {
      // Output directory
      String output = InputArguments["output"];

      // Create a Multi-Week Analysis object
      MultiProductAnalysis multi = new MultiProductAnalysis();
      multi.AddRange(collection);

      // Obtain the EPS distance value
      double eps = GetEPSInput();

      // Obtain the Minimum Number of Objects per cluster value
      int min = GetMinPointsInput();

      // Cluster the Multi-Week Data
      multi.Cluster(eps, min);

      // Analyse the Multi-Week Data
      multi.AnalyseProducts();

      // Output the KML file
      String kml_output = GenerateOutputName(".kmz");
      multi.GenerateKMZ(kml_output);

      // Save the results to the output directory
      bool minify = InputArguments.ContainsKey("jsminify") ? true : false;
      multi.CreateProductsAnalysisJSON(output, minify);
    }

    /// <summary>
    /// This method will create a KML filename based upon the runtime source 
    /// parameter. If a file has been given, then the filename will be used as
    /// the output KML filename. If a directory has been used, the name of the 
    /// parent directory will be used for the output KML filename.
    /// </summary>
    /// <returns>A well formed KML file name</returns>
    private static String GenerateOutputName(String ext = "")
    {
      // The name of the output file
      String name;

      // Get the attributes for file or directory
      FileAttributes fileattr = File.GetAttributes(InputArguments["source"]);
      
      // Detect whether its a directory or file
      if ((fileattr & FileAttributes.Directory) == FileAttributes.Directory)
      {
        // Directory
        name = Path.GetFileName(Path.GetDirectoryName(InputArguments["source"]));
      }
      else
      {
        // File 
        name = Path.GetDirectoryName(InputArguments["source"]);
      }
      return InputArguments["output"] + name + "_output" + ext;
    }

    /// <summary>
    /// This method will try to obtain a correct EPS value from the given 
    /// command line arguments. If the argument is invalid (null or less than 
    /// 0), then the default value will be returned.
    /// </summary>
    /// <returns>The EPS value as a double</returns>
    private static double GetEPSInput()
    {
      // Return the default value
      if (!InputArguments.ContainsKey("eps"))
      {
        return EPS;
      }

      try
      {
        // Obtain the value
        double eps = Double.Parse(InputArguments["eps"]);

        // Return the eps value if deemed valid
        if (eps > 0)
        {
          return eps;
        }
      }
      catch (FormatException)
      {
        // do nothing
      }

      // default value
      return EPS;
    }

    /// <summary>
    /// This method will try to obtain a correct Minimum Points value from the 
    /// given command line arguments. If the argument is invalid (null or less 
    /// than 0), then the default value will be returned.
    /// </summary>
    /// <returns>The minimum number of points per cluster</returns>
    private static int GetMinPointsInput()
    {
      // Return the default value
      if (!InputArguments.ContainsKey("min"))
      {
        return MIN_POINTS;
      }

      try
      {
        // Obtain the value
        int minpts = int.Parse(InputArguments["min"]);

        // Return the eps value if deemed valid
        if (minpts > 0)
        {
          return minpts;
        }
      }
      catch (FormatException)
      {
        // do nothing
      }

      // default value
      return MIN_POINTS;
    }


    #endregion
  }
}
