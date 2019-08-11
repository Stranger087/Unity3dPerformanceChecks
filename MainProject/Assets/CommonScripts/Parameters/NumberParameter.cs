using System;

namespace DefaultNamespace.Parameters
{
    public class NumberParameter : TestParameter
    {
        public override ParameterType Type {
            get { return ParameterType.Slider; }
        }

        public float Min;
        public float Max;
        public float Value;
        public int Precision = 100;

        public Action<float> OnChanged;
    }
}