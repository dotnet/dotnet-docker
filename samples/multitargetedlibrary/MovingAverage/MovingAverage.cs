using System;
using System.Collections.Generic;

namespace Utils
{
    public class MovingAverage
    {
        private const int MinWindowsSize = 6;
        private int _windowSize;
        private int[] _values;
        private int _average = 0;
        private int _index = 0;
        private int _sum = 0;

        public MovingAverage(int windowSize = MinWindowsSize)
        {
            if (windowSize < MinWindowsSize)
            {
                throw new ArgumentException($"Window size must be {MinWindowsSize} or greater");
            }

            _windowSize = windowSize;
            _values = new int[windowSize];
        }

        public int Window => _windowSize;

        public int Add(int value)
        {
            if (_index >= _windowSize)
            {
                _index = 0;
            }

            _sum -= _values[_index];
            _sum += value;
            _values[_index++] = value;
            _average = _sum / _windowSize;
            return _average;
        }

        public int GetAverage() => _average;

#if NETSTANDARD2_1_OR_GREATER
        public async IAsyncEnumerable<int> GetAveragesAsync(IAsyncEnumerable<int> source)
        {
            await foreach (int result in source)
            {
                yield return Add(result);
            }
        }
#endif
    }
}
