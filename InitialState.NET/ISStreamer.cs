/*Copyright 2019 Tektronix Inc.
 *
 *Licensed under the Apache License, Version 2.0 (the "License");
 *you may not use this file except in compliance with the License.
 *You may obtain a copy of the License at
 *
 *    https://www.apache.org/licenses/LICENSE-2.0
 *
 *Unless required by applicable law or agreed to in writing, software
 *distributed under the License is distributed on an "AS IS" BASIS,
 *WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *See the License for the specific language governing permissions and
 *limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections;

namespace InitialState.Streaming
{
    public struct StreamResponse
    {
        public bool Success;
        public System.Net.HttpStatusCode StatusCode;
        public int RateLimit;
        public int RateLimitRemaining;
        public DateTime RateLimitReset;
    }

    /// <summary>
    /// An object that serves to stream event data to an Initial State Event Data Stream.
    /// </summary>
    public class ISStreamer : IDisposable
    {
        /// <summary>
        /// An indicator of the status of a <see cref="CreateBucket(string, string, string)"/> call.
        /// </summary>
        public enum CreateBucketStatus
        {
            Success,
            AlreadyExists,
            Error
        }

        // Private Fields
        //---------------
        private HttpClient _httpClient = null;
        private StringBuilder _jsonStrBuilder = null;
        private readonly DateTime _epochDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        // Properties
        //-----------
        /// <summary>
        /// The base address for the Initial State API endpoint.
        /// </summary>
        public Uri ApiBaseAddress { get; private set; }
        
        /// <summary>
        /// The access key that will be used to access the Initial State API.
        /// </summary>
        public string AccessKey { get; private set; } = null;

        /// <summary>
        /// The bucket key that identifies the Initial State event data bucket to which data will be streamed.
        /// </summary>
        public string BucketKey { get; private set; } = null;

        /// <summary>
        /// Holds a collection of <see cref="ISEventData"/> that will be streamed to Initial State when <see cref="Stream"/>() is called.
        /// </summary>
        public ISEventDataCollection EventData { get; set; }


        // Constructors
        //-------------
        /// <summary>
        /// Initializes a new instance of the <see cref="ISStreamer"/> class.
        /// </summary>
        /// <remarks>Uses the default API base address https://groker.init.st/api/</remarks>
        public ISStreamer() : this("https://groker.init.st/api/") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ISStreamer"/> class with a specified API base address.
        /// </summary>
        /// <param name="apiBaseAddress">The URL where InitalState API calls will be made.</param>
        public ISStreamer(string apiBaseAddress)
        {
            this.ApiBaseAddress = new Uri(apiBaseAddress);

            this.EventData = new ISEventDataCollection();
            this._jsonStrBuilder = new StringBuilder(8192);
        }

        // Public Methods
        //---------------
        /// <summary>
        /// Connects the <see cref="ISStreamer"/> to an Event Data Bucket for streaming.
        /// </summary>
        /// <param name="accessKey">The access key that will be used to access the Initial State API.</param>
        /// <param name="bucketKey">The bucket key that identifies the Initial State Event Data bucket to which event data will be streamed.</param>
        public void ConnectBucket(string accessKey, string bucketKey)
        {
            //return CreateBucket(accessKey, bucketKey, null);

            this.AccessKey = accessKey;
            this.BucketKey = bucketKey;

            closeHttpClient();

            this._httpClient = new HttpClient { BaseAddress = this.ApiBaseAddress };

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Version", "~0");
            this._httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Is-AccessKey", this.AccessKey);
            this._httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Is-BucketKey", this.BucketKey);

        }

        /// <summary>
        /// Creates and connects the <see cref="ISStreamer"/> to a new Event Data Bucket for streaming.
        /// </summary>
        /// <param name="accessKey">The access key that will be used to access the Initial State API.</param>
        /// <param name="bucketKey">The bucket key that identifies the Initial State event data bucket to which data will be streamed.</param>
        /// <param name="bucketName">A name for the newly created event data bucket.</param>
        /// <returns>A status indicating the result of the <see cref="CreateBucket(string, string, string)"/> call.</returns>
        /// <remarks>If the bucket already exists, then the method will return <see cref="CreateBucketStatus.AlreadyExists"/>.</remarks>
        public CreateBucketStatus CreateBucket(string accessKey, string bucketKey, string bucketName)
        {
            closeHttpClient();

            this._httpClient = new HttpClient { BaseAddress = this.ApiBaseAddress };

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Version", "~0");
            this._httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-IS-AccessKey", accessKey);

            string json;
            if (bucketName != null)
                json = $"{{ \"bucketKey\": \"{bucketKey}\",  \"bucketName\": \"{bucketName}\"}}";
            else
                json = $"{{ \"bucketKey\": \"{bucketKey}\"}}";

            // Connect/Create Bucket
            try
            {
                using (var content = new StringContent(json,
                    System.Text.Encoding.Default,
                    "application/json"))
                {
                    using (var responseTask = _httpClient.PostAsync("buckets", content))
                    {
                        responseTask.Wait(5000);

                        // Keys accepted so store them
                        if (responseTask.Result.StatusCode == System.Net.HttpStatusCode.Created ||
                            responseTask.Result.StatusCode == System.Net.HttpStatusCode.NoContent)
                        {
                            this.AccessKey = accessKey;
                            this.BucketKey = bucketKey;
                        }

                        if (responseTask.Result.StatusCode == System.Net.HttpStatusCode.Created) // Then bucket was created
                        {
                            return CreateBucketStatus.Success;
                        }
                        else if (responseTask.Result.StatusCode == System.Net.HttpStatusCode.NoContent) // Then bucket already exists
                        {
                            return CreateBucketStatus.AlreadyExists;
                        }
                        else
                        {
                            return CreateBucketStatus.Error;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                closeHttpClient();
                throw ex;
            }
        }

        /// <summary>
        /// Streams the events contained in <see cref="EventData"/> to Initial State.  This call blocks and will not return until streaming of the data is complete.
        /// </summary>
        /// /// <returns>A <see cref="StreamResponse?"/> containing the response for the stream event.</returns>
        /// <remarks>Upon succesful streaming the the event data, <see cref="EventData"/> will be cleared.</remarks>
        public StreamResponse? Stream()
        {
            return Stream(-1);
        }
        /// <summary>
        /// Streams the events contained in <see cref="EventData"/> to Initial State.  This call blocks and will not return until streaming of the data is complete or timeout occurs.
        /// </summary>
        /// <returns>A <see cref="StreamResponse?"/> containing the response for the stream event.</returns>
        /// <remarks>Upon succesful streaming the the event data, <see cref="EventData"/> will be cleared.</remarks>
        public StreamResponse? Stream(int milliSecondsTimeout)
        {
            var streamTask = StreamAsync();
            streamTask.Wait(milliSecondsTimeout);
            if (streamTask.Exception != null)
            {
                throw streamTask.Exception;
            }
            return streamTask.Result;
        }
        /// <summary>
        /// Streams the event data contained in <see cref="EventData"/> to Initial State.
        /// </summary>s
        /// <returns>A <see cref="StreamResponse?"/> containing the response for the stream event.</returns>
        /// <remarks>Upon succesful streaming the the event data, <see cref="EventData"/> will be cleared.</remarks>
        public async Task<StreamResponse?> StreamAsync()
        {
            if (_httpClient == null)
            {
                throw new InvalidOperationException("Cannot post data.  Stream is not connected to a bucket.");
            }
            if (this.EventData.Count == 0)
            {
                // Nothing to send so just return
                return null;
            }

            StreamResponse sr;

            _jsonStrBuilder.Clear();
            _jsonStrBuilder.Append("[\r\n");

            foreach (ISEventData entry in this.EventData)
            {
                _jsonStrBuilder.Append(entry.ToJsonString() + ",\r\n");
            }
            _jsonStrBuilder.Remove(_jsonStrBuilder.Length - 3, 3); // Remove the last ",\r\n"
            _jsonStrBuilder.Append("\r\n]");

            var response = await sendToApi(_jsonStrBuilder.ToString());

            sr.Success = response.StatusCode == System.Net.HttpStatusCode.NoContent;
            sr.StatusCode = response.StatusCode;
            int.TryParse(response.Headers.GetValues("X-RateLimit-Limit").First(), out sr.RateLimit);
            int.TryParse(response.Headers.GetValues("X-RateLimit-Remaining").First(), out sr.RateLimitRemaining);
            int.TryParse(response.Headers.GetValues("X-RateLimit-Reset").First(), out int epochTimestamp);
            
            sr.RateLimitReset = _epochDateTime.AddSeconds(epochTimestamp).ToLocalTime();

            // Data is sent so clear out the buffer
            if (sr.Success)
            {
                this.EventData.Clear();
            }

            return sr;
        }

        /// <summary>
        /// Closes the <see cref="ISStreamer"/>'s connection to the event data bucket.
        /// </summary>
        public void Close()
        {
            closeHttpClient();
            AccessKey = null;
            BucketKey = null;
        }


        // Private Methods
        //----------------
        private async Task<HttpResponseMessage> sendToApi(string jsonData)
        {
            var response = await _httpClient.PostAsync("events",
                    new StringContent(jsonData, Encoding.UTF8, "application/json"));
            return response;
        }
        private void closeHttpClient()
        {
            if (this._httpClient != null)
            {
                this._httpClient.Dispose();
                this._httpClient = null;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    closeHttpClient();
                    if (EventData != null)
                    {
                        EventData.Clear();
                        EventData = null;
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
