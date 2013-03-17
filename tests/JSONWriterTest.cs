using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JSON;

namespace Tests
{
  [TestClass]
  public class JSONWriterTest : Test
  {
    #region Public Methods

    /// <summary>
    /// This method will test to ensure that a JSON array can be created from 
    /// a given List of Strings.
    /// </summary>
    [TestMethod]
    public void TestCreateJSONArrayList()
    {
      // The key of the JSON array
      String key = "cars";
      // The values within the array
      List<String> elements = new List<String>(new String[]{"Saab", "Volvo", "BMW"});   
      // The expected JSON String
      String expected = "\"cars\": [Saab,Volvo,BMW]";
      // The actual JSON String
      String actual = JSONWriter.CreateJSONArray(key, elements);
      // Ensure the expected JSON String and the actual JSON String are equal
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// This method will test to ensure that a JSON array can be created from 
    /// a given number of additional string parameters.
    /// </summary>
    [TestMethod]
    public void TestCreateJSONArrayParams()
    {
      // The expected JSON String
      String expected = "\"cars\": [Saab,Volvo,BMW]";
      // The actual JSON String
      String actual = JSONWriter.CreateJSONArray("cars", "Saab", "Volvo", "BMW");
      // Ensure the expected JSON String and the actual JSON String are equal
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// This method will test to ensure that a JSON Key-value pair can be 
    /// created from a given key and value.
    /// </summary>
    [TestMethod]
    public void TestCreateKeyValue()
    {
      // The expected JSON String
      String expected = "\"name\": \"Dave\"";
      // The actual JSON String
      String actual = JSONWriter.CreateKeyValue("name", "Dave");
      // Ensure the expected JSON String and the actual JSON String are equal
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// This method will test to ensure that a JSON object can be created from
    /// a given number of elements
    /// </summary>
    [TestMethod]
    public void TestCreateJSONObjectElements()
    {
      // The expected JSON String
      String expected = "{ apple, banana, pear }";
      // The actual JSON String
      String actual = JSONWriter.CreateJSONObject("apple", "banana", "pear");
      // Ensure the expected JSON String and the actual JSON String are equal
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// This method will test to ensure that a JSON object can be created from
    /// a given number of key-value elements
    /// </summary>
    [TestMethod]
    public void TestCreateJSONObjectKeyValue()
    {
      // The expected JSON String
      String expected = "{ \"name\": \"Dave\", \"age\": \"30\" }";
      //The input data
      String name = JSONWriter.CreateKeyValue("name", "Dave");
      String age = JSONWriter.CreateKeyValue("age", "30");
      // The actual JSON String
      String actual = JSONWriter.CreateJSONObject(name, age);
      // Ensure the expected JSON String and the actual JSON String are equal
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// This method will ensure that a minified JSON String can be successfully 
    /// unminified.
    /// </summary>
    [TestMethod]
    public void TestPrettifyJSON()
    {
      // The input JSON String
      String input = "{ \"name\": \"Dave\", \"age\": \"30\" }";
      // The expected JSON String (note: neeeds to contain the indentations)
      String expected = "{\r\n  \"name\": \"Dave\",\r\n  \"age\": \"30\" \r\n}";
      // The actual JSON String
      String actual = JSONWriter.PrettifyJSON(input);
      // Ensure the expected JSON String and the actual JSON String are equal
      Assert.AreEqual(expected, actual);
    }

    #endregion
  }
}