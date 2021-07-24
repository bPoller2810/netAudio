using System;

namespace netAudio.core.Extensions
{
    /// <summary>
    /// Extensions for Arrays
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Creates a SubArray of the given one
        /// </summary>
        /// <typeparam name="T">The generic type of the Array</typeparam>
        /// <param name="array">The given input</param>
        /// <param name="offset">The offset from where the new Array starts</param>
        /// <param name="length">The length of the new Array</param>
        /// <returns>The newly created SubArray</returns>
        public static T[] SubArray<T>(this T[] array, int offset, int length)
        {
            var result = new T[length];
            Array.Copy(array, offset, result, 0, length);
            return result;
        }
    }
}
