using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorScript
{
    public Animator animator;
    public float _speed;
    public float _Hor;
    public float _Ver;
    public bool _Locomotiom;
     
    private string currrentAnimation = "";

    public void animation_Walk(float Speed)
    {
        //Debug.Log(Speed + " : " + Hor + " : " + Ver + " : " + isLocomotion);
        if (this.animator != null)
        {
            Debug.Log("con");
            
            if (Speed >= 6)
            {
                AnimationChange("Run");
            }
            else if (Speed >= 2)
            {
                AnimationChange("Walk1");
            }
            else if(Speed <= 0.1)
            {
                AnimationChange("Idle");
            }

        }
        else
        {
            //Debug.Log("null");
            //animator = gameObject.GetComponent<Animator>();
        }
        
    }
    public void animation_Walk(float Speed, float Hor, float Ver, bool isLocomotion)
    {
        //Debug.Log(Speed + " : " + Hor + " : " + Ver + " : " + isLocomotion);
        if (this.animator != null)
        {
            Debug.Log("con");
            if (Speed != 0)
            {
                AnimationChange("Idle");
            }
            else if (Speed >= 2)
            {
                AnimationChange("Walk1");
            }
            else if (Speed >= 6)
            {
                AnimationChange("Run");
            }
        }
        else
        {
            //animator = gameObject.GetComponent<Animator>();
        }

    }
    private void AnimationChange(string animation,float CrossFade = 0.2f)
    {
        if (currrentAnimation != animation)
        {
            currrentAnimation = animation;
            this.animator.Play(animation);
            //this.animator.CrossFade(animation, CrossFade);
            Debug.Log(currrentAnimation);
        }
    }
}
