using System;
using UnityEngine;

public static class AnimatorExtensions
{
    public static bool HasAnimation(this Animator animator, string animationName)
    {
        if (animator == null || animator.runtimeAnimatorController == null)
        {
            return false;
        }

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        int clipsLength = clips.Length;
        for (int i = 0; i < clipsLength; i++)
        {
            if (clips[i].name.Contains(animationName.ToString()))
            {
                return true;
            }
        }

        return false;
    }

    public static bool HasParam(this Animator animator, string paramName)
    {
        if (animator.runtimeAnimatorController == null)
        {
            return false;
        }

        AnimatorControllerParameter[] parameters = animator.parameters;

        int parametersLength = animator.parameters.Length;
        for (int i = 0; i < parametersLength; i++)
        {
            if (parameters[i].name == paramName)
            {
                return true;
            }
        }

        return false;
    }

    public static AnimationClip GetAnimation<TEnum>(this Animator animator, TEnum value) where TEnum : Enum
    {
        return GetAnimation(animator, value.ToString());
    }

    public static AnimationClip GetAnimation(this Animator animator, string name)
    {
        if (animator == null || animator.runtimeAnimatorController == null)
        {
            return null;
        }

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        int clipsLength = clips.Length;
        for (int i = 0; i < clipsLength; i++)
        {
            AnimationClip clip = clips[i];
            if (clip.name.Contains(name))
            {
                return clip;
            }
        }

        return null;
    }

    public static bool TryGetAnimation<TEnum>(this Animator animator, TEnum value, out AnimationClip clip) where TEnum : Enum
    {
        clip = GetAnimation(animator, value);
        return clip != null;
    }
}
