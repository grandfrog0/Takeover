using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfoAnimationWithAnimator : UnitInfoAnimation
{
    [SerializeField] Animator anim;
    [SerializeField] string animName = "anim";
    public override IEnumerator Animation()
    {
        anim.Play(animName);
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
    }
}
