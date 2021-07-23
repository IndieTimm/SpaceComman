using UnityEngine;

namespace GameUtilities.Math
{
    public static class MathUtility
    {
        public static void MinMaxCompare(out float min, out float max, params float[] values)
        {
            min = float.MaxValue;
            max = float.MinValue;

            foreach (float value in values)
            {
                if (min > value)
                {
                    min = value;
                }

                if (max < value)
                {
                    max = value;
                }
            }
        }

        public static float SignOrZero(float value)
        {
            if (Mathf.Approximately(value, 0))
            {
                return 0;
            }

            return value > 0
                ? 1
                : -1;
        }

        public static int SignOrZero(int value)
        {
            if (value == 0)
            {
                return 0;
            }

            return value > 0
                ? 1
                : -1;
        }
    }
}
