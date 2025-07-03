using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class RandomIdleAnimation : MonoBehaviour
{
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        if (!anim) { return; }
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        anim.Play(stateInfo.fullPathHash, -1, Random.Range(0f, 1f));
    }
}
