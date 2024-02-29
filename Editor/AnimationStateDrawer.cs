using JCI.Attributes;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace JCI.Editor
{
    [CustomPropertyDrawer(typeof(AnimatorStateAttribute))]
    public class AnimatorStateDrawer : PropertyDrawer
    {
        private string[] stateNames;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var animatorReferenceName = ((AnimatorStateAttribute)attribute).animatorReferenceName;

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

            var controller = animator.runtimeAnimatorController as AnimatorController;
            if (controller == null)
            {
                EditorGUI.PropertyField(position, property, label);
                EditorGUI.EndProperty();
                return;
            }

            var stateNames = GetStateNames(controller);
            var selectedIndex = Mathf.Max(0, System.Array.IndexOf(stateNames, property.stringValue));

            selectedIndex = EditorGUI.Popup(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), label.text, selectedIndex, stateNames);

            property.stringValue = stateNames[selectedIndex];

            EditorGUI.EndProperty();
        }

        private string[] GetStateNames(AnimatorController controller)
        {
            var stateNames = new string[controller.layers[0].stateMachine.states.Length];
            for (int i = 0; i < controller.layers[0].stateMachine.states.Length; i++)
            {
                stateNames[i] = controller.layers[0].stateMachine.states[i].state.name;
            }
            return stateNames;
        }
    }
}

