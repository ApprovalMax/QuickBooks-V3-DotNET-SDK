﻿////*********************************************************
// <copyright file="Logger.cs" company="Intuit">
/*******************************************************************************
 * Copyright 2016 Intuit
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *******************************************************************************/

// <summary>This file contains Logger.</summary>
////*********************************************************

namespace Intuit.Ipp.Core.Configuration
{
    using Intuit.Ipp.Diagnostics;

    /// <summary>
    /// Contains properties used to set the Logging mechanism.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Enable request and response logging.
        /// </summary>
        public bool UseVerboseLogging { get; set; }

        /// <summary>
        /// Gets or sets the Request logging mechanism.
        /// </summary>
        public RequestLog RequestLog { get; set; }

        /// <summary>
        /// Gets or sets the Custom logger implementation class.
        /// </summary>
        public ILogger CustomLogger { get; set; }
    }
}
