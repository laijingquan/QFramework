/****************************************************************************
 * Copyright (c) 2017 liangxie
 * 
 * http://liangxiegame.com
 * https://github.com/liangxiegame/QFramework
 * https://github.com/liangxiegame/QSingleton
 * https://github.com/liangxiegame/QChain
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 ****************************************************************************/

namespace QFramework
{
    using System.Collections.Generic;

    /// Automatic Reference Counting (ARC)
    /// is used internally to prevent pooling retained Objects.
    /// If you use retain manually you also have to
    /// release it manually at some point.
    /// SafeARC checks if the object has already been
    /// retained or released. It's slower, but you keep the information
    /// about the owners.
    public sealed class SafeARC : IRefCounter
    {
        public int RefCount
        {
            get { return mOwners.Count; }
        }

        public HashSet<object> Owners
        {
            get { return mOwners; }
        }

        readonly object mObj;
        readonly HashSet<object> mOwners = new HashSet<object>();

        public SafeARC(object obj)
        {
            mObj = obj;
        }

        public void Retain(object refOwner)
        {
            if (!Owners.Add(refOwner))
            {
                throw new ObjectIsAlreadyRetainedByOwnerException(mObj, refOwner);
            }
        }

        public void Release(object refOwner)
        {
            if (!Owners.Remove(refOwner))
            {
                throw new ObjectIsNotRetainedByOwnerExceptionWithHint(mObj, refOwner);
            }
        }
    }
}