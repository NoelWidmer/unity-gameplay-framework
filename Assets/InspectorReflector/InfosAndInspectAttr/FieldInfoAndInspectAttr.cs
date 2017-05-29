using System.Reflection;

namespace InspectorReflector.Implementation
{
    public class FieldInfoAndInspectAttr : MemberInfoAndInspectAttr
    {
        public FieldInfoAndInspectAttr(FieldInfo info, InspectAttribute inspectAttribute) : base(info, info.FieldType, inspectAttribute)
        {
        }

        public new FieldInfo Info
        {
            get
            {
                return (FieldInfo)base.Info;
            }
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return Info.IsInitOnly == false && Info.IsLiteral == false;
            }
        }

        public override object GetValue(object target)
        {
            return Info.GetValue(target);
        }

        public override void SetValue(object target, object newValue)
        {
            Info.SetValue(target, newValue);
        }
    }
}