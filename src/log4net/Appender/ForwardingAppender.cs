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

using log4net.Util;
using log4net.Core;

namespace log4net.Appender;

/// <summary>
/// This appender forwards logging events to attached appenders.
/// </summary>
/// <remarks>
/// <para>
/// The forwarding appender can be used to specify different thresholds
/// and filters for the same appender at different locations within the hierarchy.
/// </para>
/// </remarks>
/// <author>Nicko Cadell</author>
/// <author>Gert Driesen</author>
public class ForwardingAppender : AppenderSkeleton, IAppenderAttachable
{
  /// <summary>
  /// Closes the appender and releases resources.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Releases any resources allocated within the appender such as file handles, 
  /// network connections, etc.
  /// </para>
  /// <para>
  /// It is a programming error to append to a closed appender.
  /// </para>
  /// </remarks>
  protected override void OnClose()
  {
    // Remove all the attached appenders
    lock (LockObj)
    {
      _appenderAttachedImpl?.RemoveAllAppenders();
    }
  }

  /// <summary>
  /// Forward the logging event to the attached appenders 
  /// </summary>
  /// <param name="loggingEvent">The event to log.</param>
  /// <remarks>
  /// <para>
  /// Delivers the logging event to all the attached appenders.
  /// </para>
  /// </remarks>
  protected override void Append(LoggingEvent loggingEvent)
    => _appenderAttachedImpl?.AppendLoopOnAppenders(loggingEvent);

  /// <summary>
  /// Forward the logging events to the attached appenders 
  /// </summary>
  /// <param name="loggingEvents">The array of events to log.</param>
  /// <remarks>
  /// <para>
  /// Delivers the logging events to all the attached appenders.
  /// </para>
  /// </remarks>
  protected override void Append(LoggingEvent[] loggingEvents)
    => _appenderAttachedImpl?.AppendLoopOnAppenders(loggingEvents);

  /// <summary>
  /// Adds an <see cref="IAppender" /> to the list of appenders of this
  /// instance.
  /// </summary>
  /// <param name="appender">The <see cref="IAppender" /> to add to this appender.</param>
  /// <remarks>
  /// <para>
  /// If the specified <see cref="IAppender" /> is already in the list of
  /// appenders, then it won't be added again.
  /// </para>
  /// </remarks>
  public virtual void AddAppender(IAppender appender)
  {
    appender.EnsureNotNull();
    lock (LockObj)
    {
      _appenderAttachedImpl ??= new();
      _appenderAttachedImpl.AddAppender(appender);
    }
  }

  /// <summary>
  /// Gets the appenders contained in this appender as an 
  /// <see cref="System.Collections.ICollection"/>.
  /// </summary>
  /// <remarks>
  /// If no appenders can be found, then an <see cref="EmptyCollection"/> 
  /// is returned.
  /// </remarks>
  /// <returns>
  /// A collection of the appenders in this appender.
  /// </returns>
  public virtual AppenderCollection Appenders
  {
    get
    {
      lock (LockObj)
      {
        return _appenderAttachedImpl?.Appenders ?? AppenderCollection.EmptyCollection;
      }
    }
  }

  /// <summary>
  /// Looks for the appender with the specified name.
  /// </summary>
  /// <param name="name">The name of the appender to lookup.</param>
  /// <returns>
  /// The appender with the specified name, or <c>null</c>.
  /// </returns>
  /// <remarks>
  /// <para>
  /// Get the named appender attached to this appender.
  /// </para>
  /// </remarks>
  public virtual IAppender? GetAppender(string? name)
  {
    lock (LockObj)
    {
      if (_appenderAttachedImpl is null || name is null)
      {
        return null;
      }

      return _appenderAttachedImpl.GetAppender(name);
    }
  }

  /// <summary>
  /// Removes all previously added appenders from this appender.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This is useful when re-reading configuration information.
  /// </para>
  /// </remarks>
  public virtual void RemoveAllAppenders()
  {
    lock (LockObj)
    {
      if (_appenderAttachedImpl is not null)
      {
        _appenderAttachedImpl.RemoveAllAppenders();
        _appenderAttachedImpl = null;
      }
    }
  }

  /// <summary>
  /// Removes the specified appender from the list of appenders.
  /// </summary>
  /// <param name="appender">The appender to remove.</param>
  /// <returns>The appender removed from the list</returns>
  /// <remarks>
  /// The appender removed is not closed.
  /// If you are discarding the appender you must call
  /// <see cref="IAppender.Close"/> on the appender removed.
  /// </remarks>
  public virtual IAppender? RemoveAppender(IAppender? appender)
  {
    lock (LockObj)
    {
      if (appender is not null && _appenderAttachedImpl is not null)
      {
        return _appenderAttachedImpl.RemoveAppender(appender);
      }
    }
    return null;
  }

  /// <summary>
  /// Removes the appender with the specified name from the list of appenders.
  /// </summary>
  /// <param name="name">The name of the appender to remove.</param>
  /// <returns>The appender removed from the list</returns>
  /// <remarks>
  /// The appender removed is not closed.
  /// If you are discarding the appender you must call
  /// <see cref="IAppender.Close"/> on the appender removed.
  /// </remarks>
  public virtual IAppender? RemoveAppender(string? name)
  {
    lock (LockObj)
    {
      if (name is not null)
      {
        return _appenderAttachedImpl?.RemoveAppender(name);
      }
    }
    return null;
  }

  /// <summary>
  /// Implementation of the <see cref="IAppenderAttachable"/> interface
  /// </summary>
  private AppenderAttachedImpl? _appenderAttachedImpl;
}