using System;
using System.Reflection;

namespace InspectorReflector.Implementation
{
    public abstract class MemberInfoAndInspectAttr
    {
        public MemberInfoAndInspectAttr(MemberInfo info, Type realType, InspectAttribute inspectAttribute)
        {
            if(info == null)
                throw new ArgumentNullException("info");

            if(inspectAttribute == null)
                throw new ArgumentNullException("inspectAttribute");

            Info = info;
            RealType = realType;
            InspectAttribute = inspectAttribute;
        }

        public MemberInfo Info
        {
            get;
            private set;
        }

        public InspectAttribute InspectAttribute
        {
            get;
            private set;
        }

        public Type RealType
        {
            get;
            private set;
        }

        public abstract bool CanRead
        {
            get;
        }

        public abstract bool CanWrite
        {
            get;
        }

        public abstract object GetValue(object target);
        public abstract void SetValue(object target, object newValue);
    }
}