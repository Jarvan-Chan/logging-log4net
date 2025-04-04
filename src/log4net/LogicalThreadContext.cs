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

#if NET462_OR_GREATER
using CallContext = System.Runtime.Remoting.Messaging.CallContext;
#else
using CallContext = System.Threading.AsyncLocal<log4net.Util.PropertiesDictionary>;
#endif

namespace log4net;

/// <summary>
/// The log4net Logical Thread Context.
/// </summary>
/// <remarks>
/// <para>
/// The <c>LogicalThreadContext</c> provides a location for <see cref="CallContext"/> specific debugging 
/// information to be stored.
/// The <c>LogicalThreadContext</c> properties override any <see cref="ThreadContext"/> or <see cref="GlobalContext"/>
/// properties with the same name.
/// </para>
/// <para>
/// For .NET Standard this class uses System.Threading.AsyncLocal rather than <see cref="CallContext"/>.
/// </para>
/// <para>
/// The Logical Thread Context has a properties map and a stack.
/// The properties and stack can 
/// be included in the output of log messages. The <see cref="Layout.PatternLayout"/>
/// supports selecting and outputting these properties.
/// </para>
/// <para>
/// The Logical Thread Context provides a diagnostic context for the current call context. 
/// This is an instrument for distinguishing interleaved log
/// output from different sources. Log output is typically interleaved
/// when a server handles multiple clients near-simultaneously.
/// </para>
/// <para>
/// The Logical Thread Context is managed on a per <see cref="CallContext"/> basis.
/// </para>
/// <para>
/// The <see cref="CallContext"/> requires a link time 
/// <see cref="System.Security.Permissions.SecurityPermission"/> for the
/// <see cref="System.Security.Permissions.SecurityPermissionFlag.Infrastructure"/>.
/// If the calling code does not have this permission then this context will be disabled.
/// It will not store any property values set on it.
/// </para>
/// </remarks>
/// <example>Example of using the thread context properties to store a username.
/// <code lang="C#">
/// LogicalThreadContext.Properties["user"] = userName;
///  log.Info("This log message has a LogicalThreadContext Property called 'user'");
/// </code>
/// </example>
/// <example>Example of how to push a message into the context stack
/// <code lang="C#">
///  using(LogicalThreadContext.Stacks["LDC"].Push("my context message"))
///  {
///    log.Info("This log message has a LogicalThreadContext Stack message that includes 'my context message'");
///  
///  } // at the end of the using block the message is automatically popped 
/// </code>
/// </example>
/// <threadsafety static="true" instance="true" />
/// <author>Nicko Cadell</author>
public static class LogicalThreadContext
{
  /// <summary>
  /// The thread properties map
  /// </summary>
  /// <remarks>
  /// <para>
  /// The <c>LogicalThreadContext</c> properties override any <see cref="ThreadContext"/> 
  /// or <see cref="GlobalContext"/> properties with the same name.
  /// </para>
  /// </remarks>
  public static LogicalThreadContextProperties Properties { get; } = new();

  /// <summary>
  /// The logical thread stacks.
  /// </summary>
  public static LogicalThreadContextStacks Stacks { get; } = new(Properties);
}