using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace InspectorReflector.Implementation
{
    public class IRDrawer
    {
        #region Static

        public static readonly Dictionary<string, Func<MemberInfoAndInspectAttr, object, object>> _drawersLookup;



        static IRDrawer()
        {
            _drawersLookup = new Dictionary<string, Func<MemberInfoAndInspectAttr, object, object>>();

            _drawersLookup.Add("$", BuiltInDrawers.DrawEnum);
            
            RegisterDrawer<AnimationCurve>(BuiltInDrawers.DrawAnimationCurve);
            RegisterDrawer<bool>(BuiltInDrawers.DrawBool);
            RegisterDrawer<byte>(BuiltInDrawers.DrawByte);
            RegisterDrawer<Bounds>(BuiltInDrawers.DrawBounds);
            RegisterDrawer<char>(BuiltInDrawers.DrawChar);
            RegisterDrawer<Color>(BuiltInDrawers.DrawColor);
            RegisterDrawer<double>(BuiltInDrawers.DrawDouble);
            RegisterDrawer<float>(BuiltInDrawers.DrawFloat);
            RegisterDrawer<int>(BuiltInDrawers.DrawInt);
            RegisterDrawer<LayerMask>(BuiltInDrawers.DrawLayerMask);
            RegisterDrawer<long>(BuiltInDrawers.DrawLong);
            RegisterDrawer<Rect>(BuiltInDrawers.DrawRect);
            RegisterDrawer<sbyte>(BuiltInDrawers.DrawSByte);
            RegisterDrawer<short>(BuiltInDrawers.DrawShort);
            RegisterDrawer<string>(BuiltInDrawers.DrawString);
            RegisterDrawer<uint>(BuiltInDrawers.DrawUInt);
            RegisterDrawer<ulong>(BuiltInDrawers.DrawULong);
            RegisterDrawer<ushort>(BuiltInDrawers.DrawUShort);
            RegisterDrawer<Vector2>(BuiltInDrawers.DrawVector2);
            RegisterDrawer<Vector3>(BuiltInDrawers.DrawVector3);
            RegisterDrawer<Vector4>(BuiltInDrawers.DrawVector4);
        }



        public static void RegisterDrawer<T>(Func<MemberInfoAndInspectAttr, object, object> drawer, bool overwrite = false)
        {
            string aqtn = typeof(T).AssemblyQualifiedName;

            if(_drawersLookup.ContainsKey(aqtn))
            {
                if(overwrite == false)
                    throw new InvalidOperationException("A drawer for the following type has already been registered: " + typeof(T).FullName);

                if(drawer != null)
                    _drawersLookup.Remove(aqtn);
            }

            if(drawer == null)
                throw new ArgumentNullException("drawer");

            _drawersLookup.Add(aqtn, drawer);
        }

        #endregion



        public bool SupportsIRInspection(object target)
        {
            if(target == null)
                throw new ArgumentNullException("target");
            
            object[] attributes = target.GetType().GetCustomAttributes(typeof(EnableIRAttribute), false);

            if(attributes == null || attributes.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        public void ReflectInspector(object target)
        {
            if(target == null)
                throw new ArgumentNullException("target");

            if(target.GetType().Name == "SampleRef")
            {
            }

            // Get all relevant fields and properties.
            List<FieldInfo> inspectableFields = null;
            List<PropertyInfo> inspectableProperties = null;
            {
                BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

                FieldInfo[] inspectableFieldsTmp = target.GetType().GetFields(flags);
                PropertyInfo[] inspectablePropertiesTmp = target.GetType().GetProperties(flags);

                if(inspectableFieldsTmp != null && inspectableFieldsTmp.Length > 0)
                    inspectableFields = inspectableFieldsTmp.ToList();

                if(inspectablePropertiesTmp != null || inspectablePropertiesTmp.Length > 0)
                    inspectableProperties = inspectablePropertiesTmp.ToList();

                if(inspectableFields == null && inspectableProperties == null)
                    return;
            }
            
            // Ignore indexed properties.
            if(inspectableProperties != null)
            {
                var nonIndexedProperties = new List<PropertyInfo>();

                foreach(var property in inspectableProperties)
                {
                    ParameterInfo[] paramInfos = property.GetIndexParameters();

                    if(paramInfos == null || paramInfos.Length == 0)
                        nonIndexedProperties.Add(property);
                }

                if(nonIndexedProperties != null || nonIndexedProperties.Count > 0)
                    inspectableProperties = nonIndexedProperties;
                else
                    inspectableProperties = null;

                if(inspectableFields == null && inspectableProperties == null)
                    return;
            }



            var fieldOrPropertyInfos = new List<MemberInfoAndInspectAttr>();



            // Get the fields that truly need to be inspected.
            if(inspectableFields != null)
            {
                foreach(var inspectableField in inspectableFields)
                {
                    object[] attributes = inspectableField.GetCustomAttributes(typeof(InspectAttribute), false);

                    if(attributes != null)
                    {
                        if(attributes.Length == 1)
                        {
                            fieldOrPropertyInfos.Add(new FieldInfoAndInspectAttr(inspectableField, (InspectAttribute)attributes[0]));
                        }
                        else if(attributes.Length == 2)
                        {
                            Warn("Found multiple attributes of type " + typeof(InspectAttribute).Name + " on " + inspectableField.DeclaringType.FullName + "." + inspectableField.Name);
                        }
                    }
                }
            }

            // Get the properties that truly need to be inspected.
            if(inspectableProperties != null)
            {
                foreach(var inspectableProperty in inspectableProperties)
                {
                    object[] attributes = inspectableProperty.GetCustomAttributes(typeof(InspectAttribute), false);

                    if(attributes != null)
                    {
                        if(attributes.Length == 1)
                        {
                            fieldOrPropertyInfos.Add(new PropertyInfoAndInspectAttr(inspectableProperty, (InspectAttribute)attributes[0]));
                        }
                        else if(attributes.Length == 2)
                        {
                            Warn("Found multiple attributes of type " + typeof(InspectAttribute).Name + " on " + inspectableProperty.DeclaringType.FullName + "." + inspectableProperty.Name);
                        }
                    }
                }
            }

            if(fieldOrPropertyInfos.Count == 0)
                return;
            
            DrawFieldsAndProperties(target, fieldOrPropertyInfos);
        }



        private void DrawFieldsAndProperties(object target, List<MemberInfoAndInspectAttr> fieldOrPropertyInfos)
        {
            foreach(var fieldOrPropertyInfo in fieldOrPropertyInfos)
            {
                MemberInfo memberInfo = fieldOrPropertyInfo.Info;

                InspectionType inspectionType = fieldOrPropertyInfo.InspectAttribute.InspectionType;

                if(fieldOrPropertyInfo.CanRead == false)
                {
                    Warn("The following property cannot be read from: " + memberInfo.Name);
                }
                else if(inspectionType == InspectionType.Readonly || fieldOrPropertyInfo.CanWrite == false)
                {
                    string text = fieldOrPropertyInfo.GetValue(target).ToString();
                    EditorGUILayout.LabelField(memberInfo.Name, text);
                }
                else if(inspectionType == InspectionType.ReadonlySelectable)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel(memberInfo.Name);
                    string text = fieldOrPropertyInfo.GetValue(target).ToString();
                    EditorGUILayout.SelectableLabel(text);
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    object origValueOrRef = fieldOrPropertyInfo.GetValue(target);
                    object newValueOrRef;

                    Func<MemberInfoAndInspectAttr, object, object> drawer;

                    if(fieldOrPropertyInfo.RealType.IsEnum)
                    {
                        newValueOrRef = _drawersLookup["$"](fieldOrPropertyInfo, origValueOrRef);
                    }
                    else
                    {
                        string aqtn = fieldOrPropertyInfo.RealType.AssemblyQualifiedName;

                        if(_drawersLookup.TryGetValue(aqtn, out drawer))
                        {
                            //TODO better null checking (string)
                            if(origValueOrRef != null || fieldOrPropertyInfo.RealType == typeof(string))
                            {
                                newValueOrRef = drawer(fieldOrPropertyInfo, origValueOrRef);
                            }
                            else
                            {
                                DrawNull(memberInfo.Name);
                                newValueOrRef = origValueOrRef;
                            }
                        }
                        else
                        {
                            if(origValueOrRef != null && fieldOrPropertyInfo.RealType.IsValueType)
                            {
                                Warn("The following value-type has no drawer: " + memberInfo.DeclaringType.FullName + "." + memberInfo.Name);
                                newValueOrRef = origValueOrRef;
                            }
                            else
                            {
                                newValueOrRef = DrawReference(fieldOrPropertyInfo, origValueOrRef);
                            }
                        }
                    }

                    if(origValueOrRef != newValueOrRef)
                        fieldOrPropertyInfo.SetValue(target, newValueOrRef);
                }
            }
        }



        private object DrawReference(MemberInfoAndInspectAttr fieldOrPropertyInfo, object reference)
        {
            InspectionType inspectionType = fieldOrPropertyInfo.InspectAttribute.InspectionType;

            if(reference is UnityEngine.Object && inspectionType == InspectionType.DropableObject)
            {
                return BuiltInDrawers.DrawDropableObject(fieldOrPropertyInfo, (UnityEngine.Object)reference, false);
            }
            else if(reference is UnityEngine.Object && inspectionType == InspectionType.DropableObjectAllowSceneObjects)
            {
                return BuiltInDrawers.DrawDropableObject(fieldOrPropertyInfo, (UnityEngine.Object)reference, true);
            }
            else
            {
                if(reference == null)
                {
                    DrawNull(fieldOrPropertyInfo.Info.Name);
                    return null;
                }
                else
                {
                    bool show = EditorGUILayout.Foldout(true, fieldOrPropertyInfo.Info.Name);
                    
                    EditorGUI.indentLevel++;
                    ReflectInspector(reference);
                    EditorGUI.indentLevel--;

                    return reference;
                }
            }
        }



        private void DrawNull(string propertyName)
        {
            EditorGUILayout.LabelField(propertyName, "Not set (null)");
        }



        private void Warn(string message)
        {
            Debug.LogWarning("[Inspection] " + message);
        }
    }
}