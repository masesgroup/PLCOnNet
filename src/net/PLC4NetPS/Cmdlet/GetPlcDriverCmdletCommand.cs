/*
*  Copyright 2025 MASES s.r.l.
*
*  Licensed under the Apache License, Version 2.0 (the "License");
*  you may not use this file except in compliance with the License.
*  You may obtain a copy of the License at
*
*  http://www.apache.org/licenses/LICENSE-2.0
*
*  Unless required by applicable law or agreed to in writing, software
*  distributed under the License is distributed on an "AS IS" BASIS,
*  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
*  See the License for the specific language governing permissions and
*  limitations under the License.
*
*  Refer to LICENSE for more information.
*/

using Java.Io;
using Java.Net;
using Org.Apache.Plc4x.JavaNs;
using Org.Apache.Plc4x.JavaNs.Api;
using Org.Apache.Plc4x.JavaNs.Api.Authentication;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace MASES.PLC4NetPS.Cmdlet
{
    [Cmdlet(VerbsCommon.Get, "PlcDriver")]
    [OutputType(typeof(PlcDriver))]
    public class GetPlcDriverCmdletCommand : PLC4NetPSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The protocol code")]
        public string ProtocolCode { get; set; }

        // This method gets called once for each cmdlet in the pipeline when the pipeline starts executing
        protected override void BeginProcessing()
        {
            WriteVerbose("Begin GetPlcDriverCmdletCommand!");
        }

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessCommand()
        {
            var driverManager = PlcDriverManager.Default;
            var driver = driverManager.GetDriver(ProtocolCode);

            WriteObject(driver);
        }

        // This method will be called once at the end of pipeline execution; if no input is received, this method is not called
        protected override void EndProcessing()
        {
            WriteVerbose("End GetPlcDriverCmdletCommand!");
        }
    }
}
