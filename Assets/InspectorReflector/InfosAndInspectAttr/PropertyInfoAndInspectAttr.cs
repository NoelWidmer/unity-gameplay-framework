using System.Reflection;

namespace InspectorReflector.Implementation
{
    public class PropertyInfoAndInspectAttr : MemberInfoAndInspectAttr
    {
        public PropertyInfoAndInspectAttr(PropertyInfo info, InspectAttribute inspectAttribute) : base(info, info.PropertyType, inspectAttribute) 
        {
        }

        public new PropertyInfo Info
        {
            get
            {
                return (PropertyInfo)base.Info;
            }
        }

        public override bool CanRead
        {
            get
            {
                return Info.CanRead;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return Info.CanWrite;
            }
        }

        public override object GetValue(object target)
        {
            return Info.GetValue(target, null);
        }

        public override void SetValue(object target, object newValue)
        {
            Info.SetValue(target, newValue, null);
        }
    }
}