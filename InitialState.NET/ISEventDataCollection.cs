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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialState.Streaming
{
    /// <summary>
    /// Represents a collection of <see cref="ISEventData"/> objects that are ready to be sent to an Initial State Event Stream using an <see cref="ISStreamer"/>.
    /// </summary>
    [System.ComponentModel.ListBindable(false)]
    public class ISEventDataCollection : System.Collections.Generic.IList<ISEventData>
    {
        private List<ISEventData> _events = null;


        // Properties
        //-----------
        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException:">index is less than 0.-or-index is equal to or greater than <see cref="Count"/>.</exception>
        public ISEventData this[int index] { get => ((IList<ISEventData>)_events)[index]; set => ((IList<ISEventData>)_events)[index] = value; }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ISEventDataCollection"/>.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="ISEventDataCollection"/>.</returns>
        public int Count => ((IList<ISEventData>)_events).Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="ISEventDataCollection"/> is read-only.
        /// </summary>
        public bool IsReadOnly => ((IList<ISEventData>)_events).IsReadOnly;


        // Constructors
        //-------------
        public ISEventDataCollection()
        {
            _events = new List<ISEventData>(32);
        }

        // Methods
        //--------
        /// <summary>
        /// Adds a <see cref="ISEventData"/> to the end of the <see cref="ISEventDataCollection"/>.
        /// </summary>
        /// <param name="item">The <see cref="ISEventData"/> to be added to the end of the <see cref="ISEventDataCollection"/>.</param>
        /// <exception cref="ArgumentNullException:">item is null</exception>
        public void Add(ISEventData item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "Cannot add null items.");
            }
            ((IList<ISEventData>)_events).Add(item);
        }

        /// <summary>
        /// Removes all elements from the <see cref="ISEventDataCollection"/>.
        /// </summary>
        public void Clear()
        {
            ((IList<ISEventData>)_events).Clear();
        }

        /// <summary>
        /// Determines whether an element is in the <see cref="ISEventDataCollection"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="ISEventDataCollection"/>.</param>
        /// <returns>true if item is found in the <see cref="ISEventDataCollection"/>; otherwise, false.</returns>
        public bool Contains(ISEventData item)
        {
            return ((IList<ISEventData>)_events).Contains(item);
        }

        /// <summary>
        /// Copies the entire <see cref="ISEventDataCollection"/> to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from <see cref="ISEventDataCollection"/>. The System.Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException">array is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
        /// <exception cref="ArgumentException">The number of elements in the source <see cref="ISEventDataCollection"/> is greater than the available space from arrayIndex to the end of the destination array.</exception>
        public void CopyTo(ISEventData[] array, int arrayIndex)
        {
            ((IList<ISEventData>)_events).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="ISEventDataCollection"/>.
        /// </summary>
        /// <returns>A <see cref="ISEventDataCollection"/>.Enumerator for the <see cref="ISEventDataCollection"/>.</returns>
        public IEnumerator<ISEventData> GetEnumerator()
        {
            return ((IList<ISEventData>)_events).GetEnumerator();
        }

        /// <summary>
        /// Searches for the specified <see cref="ISEventData"/> and returns the zero-based index of the first occurrence within the entire <see cref="ISEventDataCollection"/>.
        /// </summary>
        /// <param name="item">The <see cref="ISEventData"/> to locate in the <see cref="ISEventDataCollection"/>.</param>
        /// <returns>The zero-based index of the first occurrence of item within the entire <see cref="ISEventDataCollection"/>, if found; otherwise, –1.</returns>
        public int IndexOf(ISEventData item)
        {
            return ((IList<ISEventData>)_events).IndexOf(item);
        }

        /// <summary>
        /// Inserts an element into the <see cref="ISEventDataCollection"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The <see cref="ISEventData"/> to insert.</param>
        /// /// <exception cref="ArgumentNullException:">item is null</exception>
        public void Insert(int index, ISEventData item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "Cannot insert null items.");
            }
            ((IList<ISEventData>)_events).Insert(index, item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ISEventDataCollection"/>.
        /// </summary>
        /// <param name="item">The <see cref="ISEventData"/> to remove from the <see cref="ISEventDataCollection"/>.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the <see cref="ISEventDataCollection"/>.</returns>
        public bool Remove(ISEventData item)
        {
            return ((IList<ISEventData>)_events).Remove(item);
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="ISEventDataCollection"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">: index is less than 0.-or-index is equal to or greater than <see cref="Count"/>.</exception>
        public void RemoveAt(int index)
        {
            ((IList<ISEventData>)_events).RemoveAt(index);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="ISEventDataCollection"/>.
        /// </summary>
        /// <returns>A <see cref="ISEventDataCollection"/>.Enumerator for the <see cref="ISEventDataCollection"/>.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<ISEventData>)_events).GetEnumerator();
        }
    }
}
