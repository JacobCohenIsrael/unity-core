using JCI.Core.Animations;
using JCI.Extensions;
using RSG;
using UnityEngine;

public static class AnimatorExtensions
{
    public static IPromise WaitForState(this Animator animator, string stateName, int layer = 0, float minTimeToWait = 0f)
    {
        return animator.GetOrAddComponent<AnimatorListener>().WaitForState(animator, stateName, layer, minTimeToWait);
    }
    
    public static IPromise TriggerAndWaitForState(this Animator animator, string trigger, string stateName, int layer = 0, float minTimeToWait = 0f)
    {
        animator.SetTrigger(trigger);
        return animator.GetOrAddComponent<AnimatorListener>().WaitForState(animator, stateName, layer, minTimeToWait).Then(() =>
        {
            if (animator != null && animator.gameObject.activeInHierarchy)
                animator.ResetTrigger(trigger);
        });
    }
    
    public static IPromise TriggerAndWaitForStateChange(this Animator animator, string trigger, int layer = 0)
    {
        animator.SetTrigger(trigger);
        return animator.GetOrAddComponent<AnimatorListener>().WaitForStateChange(animator, layer).Then(() =>
        {
            if (animator != null && animator.gameObject.activeInHierarchy)
                animator.ResetTrigger(trigger);
        });
    }

    public static void ResetTriggers(this Animator animator, params string[] triggers)
    {
        foreach (var trigger in triggers)
        {
            animator.ResetTrigger(trigger);
        }
    }
}