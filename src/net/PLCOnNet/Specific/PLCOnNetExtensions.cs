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

using Java.Util.Concurrent;
using Org.Apache.Plc4x.JavaNs.Api.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MASES.PLCOnNet.Specific
{
    /// <summary>
    /// Extensions associated with <see cref="CompletableFuture{T}"/>
    /// </summary>
    public static class PLCOnNetExtensions
    {
        /// <summary>
        /// Execute the <paramref name="process"/> on completion of <paramref name="cf"/> using the <see cref="CancellationToken"/> passed from <paramref name="token"/>
        /// </summary>
        /// <typeparam name="T">A <see cref="Type"/> inherited from <see cref="PlcResponse"/> asociated to the <see cref="CompletableFuture{T}"/> passed from <paramref name="cf"/></typeparam>
        /// <param name="cf">The <see cref="CompletableFuture{T}"/> to manage</param>
        /// <param name="process">The <see cref="Action{T}"/> to be executed on <see cref="CompletableFuture{T}.WhenComplete(Java.Util.Function.BiConsumer)"/> of <paramref name="cf"/></param>
        /// <param name="token">The optional <see cref="CancellationToken"/> can be passed</param>
        /// <returns>The <see cref="Task"/> of the <see langword="async"/> pattern</returns>
        public static async Task CompleteAsync<T>(this CompletableFuture<T> cf, Action<T> process, CancellationToken token = default)
            where T : PlcResponse
        {
            await Task.Run(() =>
            {
                Java.Lang.Exception _exception = null;
                ManualResetEvent _resetEvent = new(false);
                try
                {
                    using Java.Util.Function.BiConsumer<T, Java.Lang.Exception> responseWaiter = new()
                    {
                        OnAccept = (r, e) =>
                        {
                            _exception = e;
                            _resetEvent.Set();
                            if (_exception == null)
                            {
                                process(r);
                            }
                        }
                    };
                    var cpStage = cf.WhenComplete(responseWaiter);
                    _resetEvent.WaitOne();
                    if (_exception != null) return Task.FromException(_exception);
                    return Task.CompletedTask;
                }
                catch (OperationCanceledException)
                {
                    return Task.FromCanceled(token);
                }
                finally
                {
                    _resetEvent.Dispose();
                }
            }, token);
        }

        /// <summary>
        /// Execute the <paramref name="process"/> on completion of <paramref name="cf"/>
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> asociated to the <see cref="CompletableFuture{T}"/> passed from <paramref name="cf"/></typeparam>
        /// <param name="cf">The <see cref="CompletableFuture{T}"/> to manage</param>
        /// <param name="process">The <see cref="Action{T}"/> to be executed on <see cref="CompletableFuture{T}.WhenComplete(Java.Util.Function.BiConsumer)"/> of <paramref name="cf"/></param>
        public static void Complete<T>(this CompletableFuture<T> cf, Action<T> process)
        {
            var response = cf.Get();
            process(response);
        }

        /// <summary>
        /// Execute the <paramref name="process"/> on completion of <paramref name="request"/> using the <see cref="CancellationToken"/> passed from <paramref name="token"/>
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> asociated to the <see cref="PlcRequest"/> passed from <paramref name="request"/></typeparam>
        /// <param name="request">The <see cref="PlcRequest"/> to manage</param>
        /// <param name="process">The <see cref="Action{T}"/> to be executed on completion of <paramref name="request"/></param>
        /// <param name="token">The optional <see cref="CancellationToken"/> can be passed</param>
        /// <returns>The <see cref="Task"/> of the <see langword="async"/> pattern</returns>
        public static async Task RequestAsync<T>(this PlcRequest request, Action<T> process, CancellationToken token = default)
            where T : PlcResponse
        {
            var cf = request.Execute<T>();
            await CompleteAsync(cf, process, token);
        }

        /// <summary>
        /// Execute the <paramref name="process"/> on completion of <paramref name="request"/>
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> asociated to the <see cref="PlcRequest"/> passed from <paramref name="request"/></typeparam>
        /// <param name="request">The <see cref="PlcRequest"/> to manage</param>
        /// <param name="process">The <see cref="Action{T}"/> to be executed on completion of <paramref name="request"/></param>
        public static void Request<T>(this PlcRequest request, Action<T> process)
            where T : PlcResponse
        {
            var cf = request.Execute<T>();
            var response = cf.Get();
            process(response);
        }
    }
}
