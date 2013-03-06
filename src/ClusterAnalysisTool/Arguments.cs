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
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace ClusterAnalysisTool
{
  /// <summary>
  /// Arguments class
  /// * Arguments class: application arguments interpreter
  /// *
  /// * Authors:		R. LOPES
  /// * Contributors:	R. LOPES
  /// * Created:		25 October 2002
  /// * Modified:		28 October 2002
  /// *
  /// * Version:		1.0
  /// http://www.codeproject.com/Articles/3111/C-NET-Command-Line-Arguments-Parser
  /// </summary>
  public static class Arguments
  {

    public static StringDictionary GetArguments(string[] args)
    {
      StringDictionary Parameters = new StringDictionary();
      Regex Spliter = new Regex(@"^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
      Regex Remover = new Regex(@"^['""]?(.*?)['""]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
      string Parameter = null;
      string[] Parts;

      // Valid parameters forms:
      // {-,/,--}param{ ,=,:}((",')value(",'))
      // Examples: -param1 value1 --param2 /param3:"Test-:-work" /param4=happy -param5 '--=nice=--'
      foreach (string Txt in args)
      {
        // Look for new parameters (-,/ or --) and a possible enclosed value (=,:)
        Parts = Spliter.Split(Txt, 3);
        switch (Parts.Length)
        {
          // Found a value (for the last parameter found (space separator))
          case 1:
            if (Parameter != null)
            {
              if (!Parameters.ContainsKey(Parameter))
              {
                Parts[0] = Remover.Replace(Parts[0], "$1");
                Parameters.Add(Parameter, Parts[0]);
              }
              Parameter = null;
            }
            // else Error: no parameter waiting for a value (skipped)
            break;
          // Found just a parameter
          case 2:
            // The last parameter is still waiting. With no value, set it to true.
            if (Parameter != null)
            {
              if (!Parameters.ContainsKey(Parameter)) Parameters.Add(Parameter, "true");
            }
            Parameter = Parts[1];
            break;
          // Parameter with enclosed value
          case 3:
            // The last parameter is still waiting. With no value, set it to true.
            if (Parameter != null)
            {
              if (!Parameters.ContainsKey(Parameter)) Parameters.Add(Parameter, "true");
            }
            Parameter = Parts[1];
            // Remove possible enclosing characters (",')
            if (!Parameters.ContainsKey(Parameter))
            {
              Parts[2] = Remover.Replace(Parts[2], "$1");
              Parameters.Add(Parameter, Parts[2]);
            }
            Parameter = null;
            break;
        }
      }
      // In case a parameter is still waiting
      if (Parameter != null)
      {
        if (!Parameters.ContainsKey(Parameter)) Parameters.Add(Parameter, "true");
      }

      return Parameters;
    }

  }
}
