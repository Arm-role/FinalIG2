using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // ระบุ Animator Controller
    public Animator animatorController;
    public Animation _animation;

    void Update()
    {
        //StateName();
        //ClipName();
        //ClipPlay();

    }
    private void StateName()
    {
        // ตรวจสอบให้แน่ใจว่า Animator Controller ถูกกำหนด
        if (animatorController != null)
        {
            // เข้าถึง state ปัจจุบันของ Animator Controller
            AnimatorStateInfo stateInfo = animatorController.GetCurrentAnimatorStateInfo(0);

            // ตรวจสอบว่ากำลังอยู่ในการเปลี่ยน state หรือไม่
            if (!animatorController.IsInTransition(0))
            {
                // เข้าถึงชื่อของ state ปัจจุบัน
                string currentStateName = stateInfo.shortNameHash.ToString();
                Debug.Log("Current State: " + currentStateName);
            }
            else
            {
                // ถ้ากำลังอยู่ในการเปลี่ยน state
                string nextStateName = animatorController.GetNextAnimatorStateInfo(0).shortNameHash.ToString();
                Debug.Log("Transitioning to State: " + nextStateName);
            }
        }
        else
        {
            Debug.LogError("Animator Controller ไม่ได้ถูกกำหนด!");
        }
    }
    private void ClipName()
    {
        // ตรวจสอบให้แน่ใจว่า Animator Controller ถูกกำหนด
        if (animatorController != null)
        {
            // เข้าถึง state ปัจจุบันของ Animator Controller
            AnimatorStateInfo stateInfo = animatorController.GetCurrentAnimatorStateInfo(0);

            // เข้าถึงข้อมูล Animation Clip ที่กำลังเล่นใน state ปัจจุบัน
            int clipCount = animatorController.GetCurrentAnimatorClipInfoCount(0);
            Debug.Log("Animation Clip name: " + clipCount);
            for (int i = 0; i < clipCount; i++)
            {
                AnimationClip clip = animatorController.GetCurrentAnimatorClipInfo(0)[i].clip;
                Debug.Log("Animation Clip name: " + clip.name);
            }
        }
        else
        {
            Debug.LogError("Animator Controller ไม่ได้ถูกกำหนด!");
        }
    }
    private void ClipPlay()
    {
        // ตรวจสอบให้แน่ใจว่า Animator Controller ถูกกำหนด
        if (animatorController != null)
        {
            foreach (AnimationState state in _animation)
            {
                Debug.Log(state.name);
            }
            if(Input.GetKeyDown(KeyCode.A))
            {
                animatorController.Play("A");
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                animatorController.Play("S");
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                animatorController.Play("D");
            }
        }   
        else
        {
            Debug.LogError("Animator Controller ไม่ได้ถูกกำหนด!");
        }
    }
}
