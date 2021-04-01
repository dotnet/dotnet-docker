using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using Utils;

namespace tests
{
    public class MovingAverageTests
    {
        [Fact]
        public void Average()
        {
            int[] values = Enumerable.Range(1, 10).ToArray();
            MovingAverage avg = new(values.Length);
            for (int i = 0; i < values.Length; i++)
            {
                avg.Add(values[i]);
            }

            Assert.True(avg.GetAverage() == 5);
        }

#if NET5_0_OR_GREATER
        [Fact]
        public async Task StreamingAverage()
        {
            //setup
            int count = 100;
            _sensorReadings = Enumerable.Range(1, count).ToArray();
            MovingAverage avg = new(10);

            await foreach(int average in avg.GetAveragesAsync(ReadSensorAsync(count)))
            {
                // could do something here
            }

            Assert.True(avg.GetAverage() == 95);
        }

        private int[] _sensorReadings;

        private async IAsyncEnumerable<int> ReadSensorAsync(int readings)
        {
            int count = 0;
            while (count < readings)
            {
                // perform reading
                await Task.Delay(1);
                yield return _sensorReadings[count++];
            }
            
        }
#endif
    }
}
