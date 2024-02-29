using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCI.Attributes
{
    public class AnimatorStateAttribute : PropertyAttribute
    {
        public string animatorReferenceName;

        public AnimatorStateAttribute(string referenceName = "animator")
        {
            this.animatorReferenceName = referenceName;
        }
    }
}
