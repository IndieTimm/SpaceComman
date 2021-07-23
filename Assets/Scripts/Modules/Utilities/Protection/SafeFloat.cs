using UnityEngine;

namespace Protection
{
    [System.Serializable]
    public struct SafeFloat
    {
        public float Value
        {
            get => get_value();
            set => set_value(value);
        }

        [SerializeField]
        private float value;
        private float offset;

        public SafeFloat(float value)
        {
            offset = UnityEngine.Random.value;
            this.value = value + offset;
        }

        public void Initialization()
        {
            offset = UnityEngine.Random.value;
            value += offset;
        }

        private float get_value()
        {
            return value - offset;
        }

        private void set_value(float value)
        {
            this.value = value + offset;
        }

        public static SafeFloat operator +(SafeFloat a, float b)
        {
            return new SafeFloat(a.Value + b);
        }

        public static SafeFloat operator +(SafeFloat a, SafeFloat b)
        {
            return new SafeFloat(a.Value + b.Value);
        }
    }
}