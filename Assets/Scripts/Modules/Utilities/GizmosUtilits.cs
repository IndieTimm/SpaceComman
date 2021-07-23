using UnityEngine;

namespace GameUtilities
{
    public static class GizmosUtilits
    {
        private const int MIN_CIRCLE_SEGMENTS_NUMBER = 20;
        private const int MAX_CIRCLE_SEGMENTS_NUMBER = 200;

        public static void DrawEllipse(Vector3 point, float radius)
        {
            DrawEllipse(point, radius, radius);
        }

        public static void DrawEllipse(Vector3 point, float radiusX, float radiusY)
        {
            DrawEllipse(point, Vector3.forward, Vector3.up, radiusX, radiusY);
        }

        public static void DrawEllipse(Vector3 point, Vector3 forward, Vector3 up, float radiusX, float radiusY)
        {
            var segmentsTime = Mathf.Max(radiusX, radiusY) / 30F;
            var segments = (int)Mathf.Lerp(MIN_CIRCLE_SEGMENTS_NUMBER, MAX_CIRCLE_SEGMENTS_NUMBER, segmentsTime);
            var angle = 0f;

            Quaternion rot = Quaternion.LookRotation(forward, up);

            Vector3 lastPoint = Vector3.zero;
            Vector3 thisPoint = Vector3.zero;

            for (int i = 0; i < segments + 1; i++)
            {
                thisPoint.x = Mathf.Sin(Mathf.Deg2Rad * angle) * radiusX;
                thisPoint.z = Mathf.Cos(Mathf.Deg2Rad * angle) * radiusY;

                if (i > 0)
                {
                    Gizmos.DrawLine(rot * lastPoint + point, rot * thisPoint + point);
                }

                lastPoint = thisPoint;
                angle += 360f / segments;
            }
        }
    }
}