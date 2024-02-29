using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCI.Attributes
{
    public class AnimatorTriggerAttribute : PropertyAttribute
    {
        public string animatorReferenceName;

        public AnimatorTriggerAttribute(string referenceName = "animator")
        {
            this.animatorReferenceName = referenceName;
        }
    }
}
