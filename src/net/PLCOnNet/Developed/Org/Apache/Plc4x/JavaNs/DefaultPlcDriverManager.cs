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

using Java.Lang;
using MASES.JCOBridge.C2JBridge;

namespace Org.Apache.Plc4x.JavaNs
{
    #region DefaultPlcDriverManager implementation
    public partial class DefaultPlcDriverManager
    {
        #region Static methods

        static object locker = new object();
        static DefaultPlcDriverManager _instance = null;
        /// <summary>
        /// The global <see cref="DefaultPlcDriverManager"/> instance
        /// </summary>
        public static DefaultPlcDriverManager Instance
        {
            get
            {
                lock(locker)
                {
                    if (_instance == null) _instance = new DefaultPlcDriverManager(ClassLoader.SystemClassLoader);
                    return _instance;
                }
            }
        }

        #endregion
    }
    #endregion
}