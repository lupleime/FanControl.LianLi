using LianLi;
using System;
using System.Collections.Generic;

namespace FanControl.LianLiPlugin.Tests
{
    internal static class Program
    {
        private static int _assertions;

        private static int Main()
        {
            try
            {
                TestOriginalSlA100Mapping();
                TestOtherModelsRemainUnchanged();
                TestBoundsAndMonotonicity();

                Console.WriteLine($"PASS: {_assertions} assertions");
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("FAIL: " + ex.Message);
                return 1;
            }
        }

        private static void TestOriginalSlA100Mapping()
        {
            var expected = new Dictionary<int, byte>
            {
                { 0, 10 },
                { 5, 45 },
                { 10, 47 },
                { 20, 53 },
                { 50, 71 },
                { 100, 100 }
            };

            foreach (KeyValuePair<int, byte> item in expected)
            {
                Equal(item.Value, FanSpeedMapper.Map(0xa100, item.Key), $"A100 {item.Key}%");
            }
        }

        private static void TestOtherModelsRemainUnchanged()
        {
            Equal((byte)42, FanSpeedMapper.Map(0x7750, 0), "7750 0%");
            Equal((byte)42, FanSpeedMapper.Map(0xa101, 0), "AL 0%");
            Equal((byte)9, FanSpeedMapper.Map(0xa102, 0), "SL-Infinity 0%");
            Equal((byte)12, FanSpeedMapper.Map(0xa103, 0), "SL V2 0%");
            Equal((byte)12, FanSpeedMapper.Map(0xa104, 0), "AL V2 0%");
            Equal((byte)12, FanSpeedMapper.Map(0xa105, 0), "SL V2 alternate PID 0%");
        }

        private static void TestBoundsAndMonotonicity()
        {
            int[] productIds = { 0x7750, 0xa100, 0xa101, 0xa102, 0xa103, 0xa104, 0xa105 };

            foreach (int productId in productIds)
            {
                byte previous = 0;
                for (int percentage = 0; percentage <= 100; percentage++)
                {
                    byte current = FanSpeedMapper.Map(productId, percentage);
                    True(current > 0, $"PID {productId:x4} emitted raw zero");
                    True(current >= previous, $"PID {productId:x4} decreased at {percentage}%");
                    previous = current;
                }

                Equal((byte)100, previous, $"PID {productId:x4} full speed");
            }

            Equal((byte)10, FanSpeedMapper.Map(0xa100, -1), "negative input clamps to minimum");
            Equal((byte)100, FanSpeedMapper.Map(0xa100, 101), "over-100 input clamps to full speed");
            Equal((byte)100, FanSpeedMapper.Map(0xa100, float.NaN), "NaN fails safe");
            Equal((byte)100, FanSpeedMapper.Map(0xa100, float.PositiveInfinity), "infinity fails safe");
            Equal((byte)100, FanSpeedMapper.Map(0xffff, 0), "unknown PID fails safe");
        }

        private static void Equal<T>(T expected, T actual, string message)
        {
            _assertions++;
            if (!EqualityComparer<T>.Default.Equals(expected, actual))
            {
                throw new InvalidOperationException($"{message}: expected {expected}, actual {actual}");
            }
        }

        private static void True(bool condition, string message)
        {
            _assertions++;
            if (!condition)
            {
                throw new InvalidOperationException(message);
            }
        }
    }
}
