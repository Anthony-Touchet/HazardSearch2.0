using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationUtils : MonoBehaviour
{
    public Animator animator;
    public Animator myAnimator;

    public void SetAnimationSync()
    {
        myAnimator.Play(0);
        animator.Play(0);
    }
}
