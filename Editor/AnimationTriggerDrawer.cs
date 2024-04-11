using JCI.Attributes;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace JCI.Editor
{
    [CustomPropertyDrawer(typeof(AnimatorTriggerAttribute))]
    public class AnimatorTriggerDrawer : PropertyDrawer
    {
        private string[] triggerNames;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var animatorReferenceName = ((AnimatorTriggerAttribute)attribute).animatorReferenceName;

            var animatorProperty = property.serializedObject.FindProperty(animatorReferenceName);
            if (animatorProperty == null || animatorProperty.objectReferenceValue == null)
            {
                EditorGUI.PropertyField(position, property, label);
                EditorGUI.EndProperty();
                return;
            }

            var animator = animatorProperty.objectReferenceValue as Animator;
            if (animator == null)
            {
                EditorGUI.PropertyField(position, property, label);
                EditorGUI.EndProperty();
                return;
            }

            AnimatorController controller = null;
            if (animator.runtimeAnimatorController is AnimatorOverrideController)
            {
                var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
                controller = overrideController.runtimeAnimatorController as AnimatorController;
            }
            else
            {
                controller = animator.runtimeAnimatorController as AnimatorController;
            }

            if (controller == null)
            {
                EditorGUI.PropertyField(position, property, label);
                EditorGUI.EndProperty();
                return;
            }

            var parameterNames = GetTriggerNames(controller);
            var selectedIndex = Mathf.Max(0, System.Array.IndexOf(parameterNames, property.stringValue));

            selectedIndex =
                EditorGUI.Popup(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                    label.text, selectedIndex, parameterNames);

            property.stringValue = parameterNames[selectedIndex];

            EditorGUI.EndProperty();
        }

        private string[] GetTriggerNames(AnimatorController controller)
        {
            var parameterNames = new string[controller.parameters.Length];
            for (int i = 0; i < controller.parameters.Length; i++)
            {
                parameterNames[i] = controller.parameters[i].name;
            }

            return parameterNames;
        }
    }
}