////*********************************************************
// <copyright file="TraceLogger.cs" company="Intuit">
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
// <summary>This file contains SdkException.</summary>
// <summary>This file contains Trace Logger.</summary>
////*********************************************************

namespace Intuit.Ipp.Diagnostics
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Logs trace messages using System.Diagnostics.
    /// </summary>
    public class TraceLogger : ILogger
    {
        /// <summary>
        /// Provides a multilevel switch to control tracing.
        /// </summary>
        private TraceSwitch traceSwitch;

        /// <summary>
        /// Matches a named placeholder of a message template, e.g. {RequestUri}.
        /// </summary>
        private static readonly Regex TemplatePlaceholder = new Regex(@"\{\w+\}", RegexOptions.Compiled);

        /// <summary>
        /// Initializes a new instance of the TraceLogger class.
        /// </summary>
        public TraceLogger()
        {
            // Searches for the switch name "IPPTraceSwitch" in the config file of the client.
            // If not found then default trace switch is OFF.
            this.traceSwitch = new TraceSwitch("IPPTraceSwitch", "IPP Trace Switch");
        }

        /// <summary>
        /// Logs messages depending on the ids trace level.
        /// </summary>
        /// <param name="idsTraceLevel">IDS Trace Level.</param>
        /// <param name="messageToWrite">The message to write.</param>
        /// <param name="args">The arguments.</param>
        public void Log(TraceLevel idsTraceLevel, string messageToWrite, params object?[] args)
        {
            if ((int)this.traceSwitch.Level < (int)idsTraceLevel)
            {
                return;
            }

            StackTrace st = new StackTrace(1, true);
            StackFrame sf = new StackFrame();
            sf = st.GetFrame(0);

            StringBuilder logMessage = new StringBuilder();
            logMessage.Append(string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} -- ", idsTraceLevel.ToString()));
            logMessage.Append(DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) + " -- ");
            string fileName = Path.GetFileName(sf.GetFileName());
            logMessage.Append(fileName + " - " + sf.GetFileLineNumber() + " - " + sf.GetMethod() + " - " + RenderTemplate(messageToWrite, args));

            // The message is already rendered: the Trace overloads that take arguments would run it through
            // string.Format, which throws on a named placeholder or on the braces of a logged JSON body.
            switch (idsTraceLevel)
            {
                case TraceLevel.Info:
                    Trace.TraceInformation(logMessage.ToString());
                    break;
                case TraceLevel.Verbose:
                    Trace.WriteLine(logMessage.ToString());
                    break;
                case TraceLevel.Warning:
                    Trace.TraceWarning(logMessage.ToString());
                    break;
                case TraceLevel.Error:
                    Trace.TraceError(logMessage.ToString());
                    break;
                case TraceLevel.Off:
                    break;
            }
        }

        /// <summary>
        /// Renders a message template such as "Request {Method} {RequestUri}" by substituting its named
        /// placeholders with the arguments in order, the way structured loggers do.
        /// </summary>
        /// <param name="messageTemplate">The message template.</param>
        /// <param name="args">The arguments, one per placeholder.</param>
        /// <returns>The rendered message; the template as is when there are no arguments.</returns>
        private static string RenderTemplate(string messageTemplate, object?[] args)
        {
            if (messageTemplate == null || args == null || args.Length == 0)
            {
                return messageTemplate;
            }

            int argumentIndex = 0;

            return TemplatePlaceholder.Replace(
                messageTemplate,
                placeholder => argumentIndex < args.Length
                    ? Convert.ToString(args[argumentIndex++], CultureInfo.InvariantCulture) ?? string.Empty
                    : placeholder.Value);
        }
    }
}
