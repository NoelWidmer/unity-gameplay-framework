using System;

namespace InspectorReflector
{
    /// <summary>
    ///     Properties and Fields marked with this attribute can be inspected by the IR.<br>
    ///     If the member is of type <see cref="sbyte"/> the IR will display a dragable slider.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class InspectAsSByteSliderAttribute : InspectAttribute
    {
        public readonly sbyte SliderMin;
        public readonly sbyte SliderMax;



        /// <summary/>
        /// <param name="sliderMin">The minimum value of the slider.</param>
        /// <param name="sliderMax">The maximum value of the slider.</param>
        public InspectAsSByteSliderAttribute(sbyte sliderMin, sbyte sliderMax)
        {
            SliderMin = sliderMin;
            SliderMax = sliderMax;
        }
    }
}