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

using MASES.JNetPSCore;
using MASES.PLC4Net;

namespace MASES.PLC4NetPS
{
    /// <summary>
    /// Public Helper class
    /// </summary>
    public static class PLC4NetPSHelper<TClass> where TClass : PLC4NetCore<TClass>
    {
        public static void SetClassToRun(string classToRun) { JNetPSHelper<TClass>.Set(typeof(PLC4NetCore<>), nameof(PLC4NetPSCore.ApplicationClassToRun), classToRun); }

        public static void SetCommonLoggingPath(string commonLoggingath) { JNetPSHelper<TClass>.Set(typeof(PLC4NetCore<>), nameof(PLC4NetPSCore.ApplicationCommonLoggingPath), commonLoggingath); }

        public static void SetLogPath(string logPath) { JNetPSHelper<TClass>.Set(typeof(PLC4NetCore<>), nameof(PLC4NetPSCore.ApplicationLogPath), logPath); }
    }
}