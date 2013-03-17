using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Tests
{
  public class Test
  {
    #region Properties

    /// <summary>
    /// The absolute path of the tests directory
    /// </summary>
    public String SOLUTION_DIR;
      
    /// <summary>
    /// The absolute path of the root project directory
    /// </summary>
    public String ROOT_DIR;

    /// <summary>
    /// The absolute path of the test output directory
    /// </summary>
    public String OUTPUT_DIR;

    #endregion

    #region Constructor

    /// <summary>
    /// Primary Constructor
    /// </summary>
    public Test()
    {
      // Get the current executing assembly location
      String current = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      // Set the SOLUTION_DIR as two up from executing directory
      SOLUTION_DIR = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetFullPath(current))) + "\\";
      // Set the entire ROOT_DIR
      ROOT_DIR = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetFullPath(current)))) + "\\";
      // Set the TEMP directory
      OUTPUT_DIR = SOLUTION_DIR + "\\output\\";
    }

    #endregion
  }
}
