using System;

namespace InspectorReflector
{
    /// <summary>
    ///     Properties and Fields marked with this attribute can be inspected by the IR.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class InspectAttribute : Attribute
    {
        public readonly InspectionType InspectionType;



        /// <summary>
        ///     Sets the <see cref="InspectionType"/> to <see cref="InspectionType.Default"/>.
        /// </summary>
        public InspectAttribute()
        {
            InspectionType = InspectionType.Default;
        }

        /// <summary>
        ///     Allows a custom <see cref="InspectionType"/>.
        /// </summary>
        public InspectAttribute(InspectionType inspectionType)
        {
            InspectionType = inspectionType;
        }
    }
}