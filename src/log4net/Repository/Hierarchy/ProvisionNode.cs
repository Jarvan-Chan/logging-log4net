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
using System.Collections;
using System.Collections.Generic;

namespace log4net.Repository.Hierarchy;

/// <summary>
/// Provision nodes are used where no logger instance has been specified
/// </summary>
/// <remarks>
/// <para>
/// <see cref="ProvisionNode"/> instances are used in the 
/// <see cref="Hierarchy" /> when there is no specified 
/// <see cref="Logger" /> for that node.
/// </para>
/// <para>
/// A provision node holds a list of child loggers on behalf of a logger that does not exist.
/// </para>
/// </remarks>
/// <author>Nicko Cadell</author>
/// <author>Gert Driesen</author>
internal sealed class ProvisionNode
{
  private readonly List<Logger> _loggers;

  /// <summary>
  /// Create a new provision node with child node
  /// </summary>
  /// <param name="log">A child logger to add to this node.</param>
  internal ProvisionNode(Logger log) => _loggers = [log];

  /// <summary>
  /// Add a <see cref="Logger"/> to the internal List
  /// </summary>
  /// <param name="log">Logger</param>
  internal void Add(Logger log)
  {
    lock (((IList)_loggers).SyncRoot)
    {
      _loggers.Add(log);
    }
  }

  /// <summary>
  /// Calls <paramref name="callback"/> for each logger in the internal list
  /// </summary>
  /// <param name="callback">Callback to execute</param>
  /// <param name="parent">Parant logger</param>
  internal void ForEach(Action<Logger, Logger> callback, Logger parent)
  {
    lock (((IList)_loggers).SyncRoot)
    {
      foreach (Logger log in _loggers)
      {
        callback(log, parent);
      }
    }
  }
}