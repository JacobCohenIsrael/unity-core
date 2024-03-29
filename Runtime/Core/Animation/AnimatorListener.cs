using System.Collections;
using System.Linq;
using JCI.Extensions;
using RSG;
using UnityEngine;

namespace JCI.Core.Animations
{
    public class AnimatorListener : MonoBehaviour
    {
        public IPromise WaitForStateChange(Animator animator, int layer)
        {
            var currentState = animator.GetCurrentAnimatorStateInfo(layer);
            StopCoroutine(nameof(_WaitForStateChangeRoutine));
            var p = new Promise();
            StartCoroutine(_WaitForStateChangeRoutine());

            IEnumerator _WaitForStateChangeRoutine()
            {
                while (true)
                {
                    var state = animator.GetCurrentAnimatorStateInfo(layer);
                    if (state.fullPathHash != currentState.fullPathHash)
                    {
                        p.Resolve();
                        yield break;
                    }
                    yield return null;
                }
            }

            return p;
        }

        public IPromise WaitForState(Animator animator, string stateName, int layer, float timeToWait = 0f)
        {
            StopCoroutine(nameof(_WaitForStateRoutine));
            var p = new Promise();
            StartCoroutine(_WaitForStateRoutine());

            IEnumerator _WaitForStateRoutine()
            {
                if (timeToWait == 0)
                    yield return null;
                else
                    yield return new WaitForSeconds(timeToWait); // we wait an extra frame here so the animation state have time to change the state in case we transition from current state
                while (true)
                {
                    if (animator.GetCurrentAnimatorStateInfo(layer).IsName(stateName))
                    {
                        p.Resolve();
                        yield break;
                    }
                    yield return null;
                }
            }

            return p;
        }
    }
}