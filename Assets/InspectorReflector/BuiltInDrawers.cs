using System;
using UnityEditor;
using UnityEngine;

namespace InspectorReflector.Implementation
{
    public static class BuiltInDrawers
    {
        public static object DrawAnimationCurve(MemberInfoAndInspectAttr memberInfo, object value)
        {
            return EditorGUILayout.CurveField(memberInfo.Info.Name, (AnimationCurve)value);
        }



        public static object DrawBool(MemberInfoAndInspectAttr memberInfo, object value)
        {
            return EditorGUILayout.Toggle(memberInfo.Info.Name, (bool)value);
        }



        public static object DrawByte(MemberInfoAndInspectAttr memberInfo, object value)
        {
            int newValue;
            if(memberInfo.InspectAttribute is InspectAsByteSliderAttribute)
            {
                var attr = (InspectAsByteSliderAttribute)memberInfo.InspectAttribute;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(memberInfo.Info.Name);
                newValue = EditorGUILayout.IntSlider((byte)value, attr.SliderMin, attr.SliderMax);
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                newValue = EditorGUILayout.IntField(memberInfo.Info.Name, (byte)value);

                if(newValue < byte.MinValue)
                    return byte.MinValue;
                else if(newValue > byte.MaxValue)
                    return byte.MaxValue;
            }

            return (byte)newValue;
        }



        public static object DrawBounds(MemberInfoAndInspectAttr memberInfo, object value)
        {
            return EditorGUILayout.BoundsField(memberInfo.Info.Name, (Bounds)value);
        }



        public static object DrawChar(MemberInfoAndInspectAttr memberInfo, object value)
        {
            char oldValue = (char)value;
            string newValue = EditorGUILayout.TextField(memberInfo.Info.Name, string.Empty + oldValue);

            if(newValue == null || newValue == string.Empty)
                return default(char);

            return char.Parse(newValue.Substring(0, 1));
        }



        public static object DrawColor(MemberInfoAndInspectAttr memberInfo, object value)
        {
            return EditorGUILayout.ColorField(memberInfo.Info.Name, (Color)value);
        }



        public static object DrawDouble(MemberInfoAndInspectAttr memberInfo, object value)
        {
            if(memberInfo.InspectAttribute is InspectAsFloatSliderAttribute)
            {
                var attr = (InspectAsFloatSliderAttribute)memberInfo.InspectAttribute;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(memberInfo.Info.Name);

                float narrowedDouble = Convert.ToSingle((double)value);
                float newValue = EditorGUILayout.Slider(narrowedDouble, attr.SliderMin, attr.SliderMax);

                EditorGUILayout.EndHorizontal();
                return (double)newValue;
            }
            else
            {
                switch(memberInfo.InspectAttribute.InspectionType)
                {
                    case InspectionType.DelayedDouble:
                        return EditorGUILayout.DelayedDoubleField(memberInfo.Info.Name, (double)value);
                }

                return EditorGUILayout.DoubleField(memberInfo.Info.Name, (double)value);
            }
        }



        public static object DrawDropableObject(MemberInfoAndInspectAttr memberInfo, UnityEngine.Object value, bool allowSceneObjects)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(memberInfo.Info.Name);
            UnityEngine.Object result = EditorGUILayout.ObjectField(value, memberInfo.RealType, allowSceneObjects);
            EditorGUILayout.EndHorizontal();
            return result;
        }



        public static object DrawEnum(MemberInfoAndInspectAttr memberInfo, object value)
        {
            var flagAttrs = memberInfo.RealType.GetCustomAttributes(typeof(FlagsAttribute), false);

            if(flagAttrs == null || flagAttrs.Length == 0)
            {
                return EditorGUILayout.EnumPopup(memberInfo.Info.Name, (Enum)value);
            }
            else if(flagAttrs.Length == 1)
            {
                return EditorGUILayout.EnumMaskField(memberInfo.Info.Name, (Enum)value);
            }
            else
            {
                Debug.LogWarning("Multiple attributes of type " + typeof(FlagsAttribute).FullName + " found for enum of type " + memberInfo.RealType.FullName);
                return value;
            }
        }



        public static object DrawLayerMask(MemberInfoAndInspectAttr memberInfo, object value)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(memberInfo.Info.Name);
            int newValue = (EditorGUILayout.LayerField((LayerMask)value));
            EditorGUILayout.EndHorizontal();
            return (LayerMask)newValue;
        }



        public static object DrawFloat(MemberInfoAndInspectAttr memberInfo, object value)
        {
            if(memberInfo.InspectAttribute is InspectAsFloatSliderAttribute)
            {
                var attr = (InspectAsFloatSliderAttribute)memberInfo.InspectAttribute;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(memberInfo.Info.Name);
                float newVal = EditorGUILayout.Slider((float)value, attr.SliderMin, attr.SliderMax);
                EditorGUILayout.EndHorizontal();
                return newVal;
            }
            else
            {
                switch(memberInfo.InspectAttribute.InspectionType)
                {
                    case InspectionType.DelayedFloat:
                        return EditorGUILayout.DelayedFloatField(memberInfo.Info.Name, (float)value);
                }

                return EditorGUILayout.FloatField(memberInfo.Info.Name, (float)value);
            }
        }



        public static object DrawInt(MemberInfoAndInspectAttr memberInfo, object value)
        {
            if(memberInfo.InspectAttribute is InspectAsIntSliderAttribute)
            {
                var attr = (InspectAsIntSliderAttribute)memberInfo.InspectAttribute;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(memberInfo.Info.Name);
                int newVal = EditorGUILayout.IntSlider((int)value, attr.SliderMin, attr.SliderMax);
                EditorGUILayout.EndHorizontal();
                return newVal;
            }
            else
            {
                switch(memberInfo.InspectAttribute.InspectionType)
                {
                    case InspectionType.DelayedInt:
                        return EditorGUILayout.DelayedIntField(memberInfo.Info.Name, (int)value);
                }

                return EditorGUILayout.IntField(memberInfo.Info.Name, (int)value);
            }
        }



        public static object DrawLong(MemberInfoAndInspectAttr memberInfo, object value)
        {
            return EditorGUILayout.LongField(memberInfo.Info.Name, (long)value);
        }



        public static object DrawRect(MemberInfoAndInspectAttr memberInfo, object value)
        {
            return EditorGUILayout.RectField(memberInfo.Info.Name, (Rect)value);
        }



        public static object DrawSByte(MemberInfoAndInspectAttr memberInfo, object value)
        {
            int newValue;
            if(memberInfo.InspectAttribute is InspectAsSByteSliderAttribute)
            {
                var attr = (InspectAsSByteSliderAttribute)memberInfo.InspectAttribute;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(memberInfo.Info.Name);
                newValue = EditorGUILayout.IntSlider((sbyte)value, attr.SliderMin, attr.SliderMax);
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                newValue = EditorGUILayout.IntField(memberInfo.Info.Name, (sbyte)value);

                if(newValue < sbyte.MinValue)
                    return sbyte.MinValue;
                else if(newValue > sbyte.MaxValue)
                    return sbyte.MaxValue;
            }

            return (sbyte)newValue;
        }



        public static object DrawShort(MemberInfoAndInspectAttr memberInfo, object value)
        {
            int newValue;
            if(memberInfo.InspectAttribute is InspectAsShortSliderAttribute)
            {
                var attr = (InspectAsShortSliderAttribute)memberInfo.InspectAttribute;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(memberInfo.Info.Name);
                newValue = EditorGUILayout.IntSlider((short)value, attr.SliderMin, attr.SliderMax);
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                 newValue = EditorGUILayout.IntField(memberInfo.Info.Name, (short)value);

                if(newValue < short.MinValue)
                    return short.MinValue;
                else if(newValue > short.MaxValue)
                    return short.MaxValue;
            }

            return (short)newValue;
        }



        public static object DrawString(MemberInfoAndInspectAttr memberInfo, object value)
        {
            switch(memberInfo.InspectAttribute.InspectionType)
            {
                case InspectionType.DelayedString:
                    return EditorGUILayout.DelayedTextField(memberInfo.Info.Name, (string)value);

                case InspectionType.TagString:
                    return EditorGUILayout.TagField(memberInfo.Info.Name, (string)value);

                case InspectionType.AreaString:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel(memberInfo.Info.Name);
                    string result = EditorGUILayout.TextArea((string)value);
                    EditorGUILayout.EndHorizontal();
                    return result;
            }

            return EditorGUILayout.TextField(memberInfo.Info.Name, (string)value);
        }



        public static object DrawUInt(MemberInfoAndInspectAttr memberInfo, object value)
        {
            long newValue = EditorGUILayout.LongField(memberInfo.Info.Name, (uint)value);

            if(newValue < uint.MinValue)
                return uint.MinValue;
            else if(newValue > uint.MaxValue)
                return uint.MaxValue;

            return (uint)newValue;
        }



        public static object DrawULong(MemberInfoAndInspectAttr memberInfo, object value)
        {
            ulong oldValue = (ulong)value;
            string newValue = EditorGUILayout.TextField(memberInfo.Info.Name, string.Empty + oldValue);

            if(newValue == null || newValue == string.Empty)
                return default(ulong);

            ulong result;
            if(ulong.TryParse(newValue, out result))
                return result;
            
            return oldValue;
        }



        public static object DrawUShort(MemberInfoAndInspectAttr memberInfo, object value)
        {
            int newValue;
            if(memberInfo.InspectAttribute is InspectAsUShortSliderAttribute)
            {
                var attr = (InspectAsUShortSliderAttribute)memberInfo.InspectAttribute;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(memberInfo.Info.Name);
                newValue = EditorGUILayout.IntSlider((ushort)value, attr.SliderMin, attr.SliderMax);
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                newValue = EditorGUILayout.IntField(memberInfo.Info.Name, (ushort)value);

                if(newValue < ushort.MinValue)
                    return ushort.MinValue;
                else if(newValue > ushort.MaxValue)
                    return ushort.MaxValue;
            }

            return (ushort)newValue;
        }



        public static object DrawVector2(MemberInfoAndInspectAttr memberInfo, object value)
        {
            return EditorGUILayout.Vector2Field(memberInfo.Info.Name, (Vector2)value);
        }



        public static object DrawVector3(MemberInfoAndInspectAttr memberInfo, object value)
        {
            return EditorGUILayout.Vector3Field(memberInfo.Info.Name, (Vector3)value);
        }



        public static object DrawVector4(MemberInfoAndInspectAttr memberInfo, object value)
        {
            return EditorGUILayout.Vector4Field(memberInfo.Info.Name, (Vector4)value);
        }
    }
}