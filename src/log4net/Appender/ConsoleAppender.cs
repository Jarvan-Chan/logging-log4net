#region Apache License
//
// Licensed to the Apache Software Foundation (ASF) under one or more 
// contributor license agreements. See the NOTICE file distributed with
// this work for additional information regarding copyright ownership. 
// The ASF licenses this file to you under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with 
// the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;
using log4net.Core;
using log4net.Util;

namespace log4net.Appender;

/// <summary>
/// Appends logging events to the console.
/// </summary>
/// <remarks>
/// <para>
/// ConsoleAppender appends log events to the standard output stream
/// or the error output stream using a layout specified by the 
/// user.
/// </para>
/// <para>
/// By default, all output is written to the console's standard output stream.
/// The <see cref="Target"/> property can be set to direct the output to the
/// error stream.
/// </para>
/// <para>
/// NOTE: This appender writes each message to the <c>System.Console.Out</c> or 
/// <c>System.Console.Error</c> that is set at the time the event is appended.
/// Therefore it is possible to programmatically redirect the output of this appender 
/// (for example NUnit does this to capture program output). While this is the desired
/// behavior of this appender it may have security implications in your application. 
/// </para>
/// </remarks>
/// <author>Nicko Cadell</author>
/// <author>Gert Driesen</author>
public class ConsoleAppender : AppenderSkeleton
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ConsoleAppender" /> class.
  /// </summary>
  /// <remarks>
  /// The instance of the <see cref="ConsoleAppender" /> class is set up to write 
  /// to the standard output stream.
  /// </remarks>
  public ConsoleAppender()
  {
  }

  /// <summary>
  /// Target is the value of the console output stream.
  /// This is either <c>"Console.Out"</c> or <c>"Console.Error"</c>.
  /// </summary>
  /// <value>
  /// Target is the value of the console output stream.
  /// This is either <c>"Console.Out"</c> or <c>"Console.Error"</c>.
  /// </value>
  /// <remarks>
  /// <para>
  /// Target is the value of the console output stream.
  /// This is either <c>"Console.Out"</c> or <c>"Console.Error"</c>.
  /// </para>
  /// </remarks>
  public virtual string Target
  {
    get => _writeToErrorStream ? ConsoleError : ConsoleOut;
    set => _writeToErrorStream = SystemInfo.EqualsIgnoringCase(ConsoleError, value?.Trim());
  }

  /// <summary>
  /// This method is called by the <see cref="AppenderSkeleton.DoAppend(LoggingEvent)"/> method.
  /// </summary>
  /// <param name="loggingEvent">The event to log.</param>
  /// <remarks>
  /// <para>
  /// Writes the event to the console.
  /// </para>
  /// <para>
  /// The format of the output will depend on the appender's layout.
  /// </para>
  /// </remarks>
  protected override void Append(LoggingEvent loggingEvent)
  {
    if (_writeToErrorStream)
    {
      // Write to the error stream
      Console.Error.Write(RenderLoggingEvent(loggingEvent));
    }
    else
    {
      // Write to the output stream
      Console.Write(RenderLoggingEvent(loggingEvent));
    }
  }

  /// <summary>
  /// This appender requires a <see cref="Layout"/> to be set.
  /// </summary>
  protected override bool RequiresLayout => true;

  /// <summary>
  /// The <see cref="Target"/> to use when writing to the Console standard output stream.
  /// </summary>
  public const string ConsoleOut = "Console.Out";

  /// <summary>
  /// The <see cref="Target"/> to use when writing to the Console standard error output stream.
  /// </summary>
  public const string ConsoleError = "Console.Error";

  private bool _writeToErrorStream;
}