using System.Collections.Generic;
using JCI.Attributes;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace JCI.Editor
{
    [CustomPropertyDrawer(typeof(AnimatorStateAttribute))]
    public class AnimatorStateDrawer : PropertyDrawer
    {
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

            var controller = GetAnimatorController(animator);
            if (controller == null)
            {
                EditorGUI.PropertyField(position, property, label);
                EditorGUI.EndProperty();
                return;
            }

            var stateNames = GetStateNames(controller);
            var selectedIndex = Mathf.Max(0, System.Array.IndexOf(stateNames, property.stringValue));

            selectedIndex =
                EditorGUI.Popup(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                    label.text, selectedIndex, stateNames);

            property.stringValue = stateNames[selectedIndex];

            EditorGUI.EndProperty();
        }

        private AnimatorController GetAnimatorController(Animator animator)
        {
            if (animator.runtimeAnimatorController is AnimatorOverrideController overrideController)
            {
                return overrideController.runtimeAnimatorController as AnimatorController;
            }
            else
            {
                return animator.runtimeAnimatorController as AnimatorController;
            }
        }

        private string[] GetStateNames(AnimatorController controller)
        {
            var stateNames = new List<string>();

            if (controller != null)
            {
                foreach (var layer in controller.layers)
                {
                    var stateMachine = layer.stateMachine;
                    foreach (var state in stateMachine.states)
                    {
                        stateNames.Add(state.state.name);
                    }
                }
            }

            return stateNames.ToArray();
        }
    }
}