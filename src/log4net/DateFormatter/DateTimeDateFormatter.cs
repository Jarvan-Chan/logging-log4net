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
using System.Text;
using System.Globalization;
using log4net.Util;

namespace log4net.DateFormatter;

/// <summary>
/// Formats a <see cref="DateTime"/> as <c>"dd MMM yyyy HH:mm:ss,fff"</c>
/// </summary>
/// <remarks>
/// <para>
/// Formats a <see cref="DateTime"/> in the format 
/// <c>"dd MMM yyyy HH:mm:ss,fff"</c> for example, 
/// <c>"06 Nov 1994 15:49:37,459"</c>.
/// </para>
/// </remarks>
/// <author>Nicko Cadell</author>
/// <author>Gert Driesen</author>
/// <author>Angelika Schnagl</author>
public class DateTimeDateFormatter : AbsoluteTimeDateFormatter
{
  /// <summary>
  /// Formats the date without the milliseconds part
  /// </summary>
  /// <param name="dateToFormat">The date to format.</param>
  /// <param name="buffer">The string builder to write to.</param>
  /// <remarks>
  /// <para>
  /// Formats a DateTime in the format <c>"dd MMM yyyy HH:mm:ss"</c>
  /// for example, <c>"06 Nov 1994 15:49:37"</c>.
  /// </para>
  /// <para>
  /// The base class will append the <c>",fff"</c> milliseconds section.
  /// This method will only be called at most once per second.
  /// </para>
  /// </remarks>
  protected override void FormatDateWithoutMillis(DateTime dateToFormat, StringBuilder buffer)
  {
    buffer.EnsureNotNull();
    int day = dateToFormat.Day;
    if (day < 10)
    {
      buffer.Append('0');
    }
    buffer
      .Append(day).Append(' ')
      .Append(DateTimeFormatInfo.InvariantInfo.GetAbbreviatedMonthName(dateToFormat.Month)).Append(' ')
      .Append(dateToFormat.Year).Append(' ');

    // Append the 'HH:mm:ss'
    base.FormatDateWithoutMillis(dateToFormat, buffer);
  }
}
