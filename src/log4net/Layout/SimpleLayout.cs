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
using System.IO;

using log4net.Core;
using log4net.Util;

namespace log4net.Layout;

/// <summary>
/// A very simple layout
/// </summary>
/// <remarks>
/// <para>
/// SimpleLayout consists of the level of the log statement,
/// followed by " - " and then the log message itself. For example,
/// <code>
/// DEBUG - Hello world
/// </code>
/// </para>
/// </remarks>
/// <author>Nicko Cadell</author>
/// <author>Gert Driesen</author>
public class SimpleLayout : LayoutSkeleton
{
  /// <summary>
  /// Constructs a SimpleLayout
  /// </summary>
  public SimpleLayout() => IgnoresException = true;

  /// <summary>
  /// Initialize layout options
  /// </summary>
  /// <remarks>
  /// <para>
  /// This is part of the <see cref="IOptionHandler"/> delayed object
  /// activation scheme. The <see cref="ActivateOptions"/> method must 
  /// be called on this object after the configuration properties have
  /// been set. Until <see cref="ActivateOptions"/> is called this
  /// object is in an undefined state and must not be used. 
  /// </para>
  /// <para>
  /// If any of the configuration properties are modified then 
  /// <see cref="ActivateOptions"/> must be called again.
  /// </para>
  /// </remarks>
  public override void ActivateOptions()
  {
    // nothing to do.
  }

  /// <summary>
  /// Produces a simple formatted output.
  /// </summary>
  /// <param name="loggingEvent">the event being logged</param>
  /// <param name="writer">The TextWriter to write the formatted event to</param>
  /// <remarks>
  /// <para>
  /// Formats the event as the level of the event,
  /// followed by " - " and then the log message itself. The
  /// output is terminated by a newline.
  /// </para>
  /// </remarks>
  public override void Format(TextWriter writer, LoggingEvent loggingEvent)
  {
    writer.EnsureNotNull();

    if (loggingEvent.EnsureNotNull().Level is Level level)
    {
      writer.Write(level.DisplayName);
      writer.Write(" - ");
    }
    loggingEvent.WriteRenderedMessage(writer);
    writer.WriteLine();
  }
}
