using System.Collections.Generic;
using UnityEngine;

public static class AnimatorExtensions
{
    public static bool HasParameter(this Animator animator, string paramName)
    {
        if (string.IsNullOrEmpty(paramName)) return false;

        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }

    public static bool HasState(this Animator animator, int layerIndex, string stateName)
    {
        if (string.IsNullOrEmpty(stateName)) return false;
      
        int stateHash = Animator.StringToHash(stateName);

        return animator.HasState(layerIndex, stateHash);
    }

    public static bool HasState(this Animator animator, string stateName)
    {
        if (string.IsNullOrEmpty(stateName)) return false;

        int stateHash = Animator.StringToHash(stateName);

        return animator.HasState(0, stateHash);
    }

    public static void UpdateAnimatorFloat(Dictionary<string, Animator> Animators, string parameter, float value)
    {
        foreach (var (name, anim) in Animators)
        {
            if (anim != null && anim.HasParameter(parameter)) anim.SetFloat(parameter, value);
        }
    }
    public static void PlayAnimation(Dictionary<string, Animator> Animators, string animationName)
    {
        foreach (var (name, anim) in Animators)
        {
            if (anim != null && anim.HasState(animationName)) anim.Play(animationName);
        }
    }
    public static void UpdateAnimatorBool(Dictionary<string, Animator> Animators, string parameter, bool value)
    {
        //bodyAnim.SetBool(property, value);
        foreach (var (name, anim) in Animators)
        {
            if (anim != null && anim.HasParameter(parameter)) anim.SetBool(parameter, value);
        }
    }
    public static void UpdateAnimatorSpeed(Dictionary<string, Animator> Animators, float speed)
    {
        foreach (var (name, anim) in Animators)
        {
            if (anim != null) anim.speed = speed;
        }
    }



}