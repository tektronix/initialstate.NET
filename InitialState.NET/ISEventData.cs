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

namespace InitialState.Streaming
{
    /// <summary>
    /// Represents an Initial State Event Data point which can be added to a <see cref="ISEventDataCollection"/> to be sent to an Initial State Event Data Stream using a <see cref="ISStreamer"/>.
    /// </summary>
    public class ISEventData
    {
        /// <summary>
        /// The key for the Initial State Event Data.
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// The value for the Initial State Event Data.
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// The timestamp for the Initial State Event Data.
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Sets whether or not the timestamp should be used when generating the JSON for the Initial State Event Data.
        /// </summary>
        /// <remarks>If set to false, the JSON for the Event Data will not contain a timestamp.  When streamed, Initial State will use the time at which the event data is received to timestamp the data.</remarks>
        public bool UseTimestamp { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ISEventData"/> class that has the default initial property values.
        /// </summary>
        public ISEventData()
        {
            Key = "Status";
            Value = ":beer:";
            Timestamp = DateTime.Now;
            UseTimestamp = false;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ISEventData"/> class and sets the key and value to the passed values.
        /// </summary>
        /// <param name="key">The key of the Initial State Event Data.</param>
        /// <param name="value">The value of the Initial State Event Data.</param>
        public ISEventData(string key, string value)
        {
            this.Key = key;
            this.Value = value;
            this.Timestamp = DateTime.Now;
            this.UseTimestamp = false;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ISEventData"/> class and sets the key, value and timestamp to the passed values.
        /// </summary>
        /// <param name="key">The key of the Initial State Event Data.</param>
        /// <param name="value">The value of the Initial State Event Data.</param>
        /// <param name="timestamp">The timestamp of the Initial State Event Data.</param>
        /// <remarks><see cref="UseTimestamp"/> will be set to true.</remarks>
        public ISEventData(string key, string value, DateTime timestamp)
        {
            this.Key = key;
            this.Value = value;
            this.Timestamp = timestamp;
            this.UseTimestamp = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ISEventData"/> class and sets the key and value to the passed values.
        /// </summary>
        /// <param name="key">The key of the Initial State Event Data.</param>
        /// <param name="value">The value of the Initial State Event Data.</param>
        /// <remarks><paramref name="value"/> is converted to <see cref="string"/> using the obect's .ToString() method.</remarks>
        public ISEventData(string key, object value)
        {
            Key = key;
            Value = value.ToString();
            Timestamp = DateTime.Now;
            UseTimestamp = false;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ISEventData"/> class and sets the key, value and timestamp to the passed values.
        /// <see cref="UseTimestamp"/> will be set to true.
        /// </summary>
        /// <param name="key">The key of the Initial State Event Data.</param>
        /// <param name="value">The value of the Initial State Event Data.</param>
        /// <param name="timestamp">The timestamp of the Initial State Event Data.</param>
        /// <remarks>
        /// <see cref="UseTimestamp"/> will be set to true.
        /// <paramref name="value"/> is converted to <see cref="string"/> using the obect's .ToString() method.
        /// </remarks>
        public ISEventData(string key, object value, DateTime timestamp)
        {
            Key = key;
            Value = value.ToString();
            Timestamp = timestamp;
            UseTimestamp = true;
        }

        /// <summary>
        /// Returns the content of the <see cref="ISEventData"/> as a JSON formatted <see cref="string"/> which can be used to send an event to and Initial State Event Data Stream.
        /// </summary>
        /// <returns>a JSON formatted <see cref="string"/> representing the <see cref="ISEventData"/>.</returns>
        public string ToJsonString()
        {
            if (UseTimestamp)
            {
                DateTimeOffset dto = new DateTimeOffset(Timestamp, TimeZoneInfo.Local.GetUtcOffset(Timestamp));
                string iso8601tstmp = dto.ToString("o", System.Globalization.CultureInfo.InvariantCulture);
                return $"{{ \"key\" : \"{Key}\", \"value\" : \"{Value}\", \"iso8601\" : \"{iso8601tstmp}\" }}";
            }
            else
            {
                return $"{{ \"key\" : \"{Key}\", \"value\" : \"{Value}\" }}";
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Key: {Key}, Value: {Value}, Timestamp: {Timestamp}";
        }
    }
}
