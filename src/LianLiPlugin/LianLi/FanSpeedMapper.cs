using System;

namespace LianLi
{
    internal static class FanSpeedMapper
    {
        internal const int OriginalSlA100Minimum = 10;

        public static byte Map(int productId, float requestedPercentage)
        {
            int percentage = NormalizePercentage(requestedPercentage);
            int rawSpeed;

            switch (productId)
            {
                case 0xa100:
                    rawSpeed = percentage == 0
                        ? OriginalSlA100Minimum
                        : (800 + (11 * percentage)) / 19;
                    break;
                case 0x7750:
                case 0xa101:
                    rawSpeed = (800 + (11 * percentage)) / 19;
                    break;
                case 0xa103:
                case 0xa104:
                case 0xa105:
                    rawSpeed = (int)((250 + (17.5 * percentage)) / 20);
                    break;
                case 0xa102:
                    rawSpeed = (200 + (19 * percentage)) / 21;
                    break;
                default:
                    return 100;
            }

            // Never emit a stop-like or out-of-protocol value.
            return rawSpeed >= 1 && rawSpeed <= 100
                ? (byte)rawSpeed
                : (byte)100;
        }

        private static int NormalizePercentage(float requestedPercentage)
        {
            if (float.IsNaN(requestedPercentage) || float.IsInfinity(requestedPercentage))
            {
                return 100;
            }

            return (int)Math.Max(0, Math.Min(100, requestedPercentage));
        }
    }
}
