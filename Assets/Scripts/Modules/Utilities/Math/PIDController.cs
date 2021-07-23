using UnityEngine;

namespace GameUtilities.Math
{
    [System.Serializable]
    public class Vector3PIDController
    {
        public float P = 1.0F;
        public float I = 0.2F;
        public float D = 0.7F;

        private Vector3 error_sum;
        private Vector3 error_old;

        public Vector3PIDController()
        {

        }

        public Vector3PIDController(float p, float i, float d)
        {
            P = p;
            I = i;
            D = d;
        }

        public Vector3 PID(Vector3 error)
        {
            Vector3 delta = Vector3.zero;

            delta += P * error;
            delta += I * error_sum;
            delta += D * (error - error_old);

            error_sum += error;
            error_old = error;

            return delta;
        }

        public void Reset()
        {
            error_sum = Vector3.zero;
            error_old = Vector3.zero;
        }
    }
}
