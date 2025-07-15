using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationUtilities 
{
    private const bool DEBUG = true;

    public static float GetAnimationClipDuration(Animator animator, string animationName)
    {
        AnimationClip clip = GetAnimationClipByName(animator, animationName);

        if (clip == null)
        {
            if (DEBUG) Debug.Log("Clip is null. Returning 0.");
        }

        return clip.length;
    }

    public static AnimationClip GetAnimationClipByName(Animator animator, string animationName)
    {
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animationName) return clip;

        }

        if (DEBUG) Debug.Log($"Could not find an Animation Clip with name:{animationName}. Returning null");

        return null;
    }
}
