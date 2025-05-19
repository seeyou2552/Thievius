using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationController : MonoBehaviour
{
    public void ShowAnimation(GameObject obj) // 활성화 후 애니메이션
    {
        Animator animator;
        animator = obj.GetComponent<Animator>();
        obj.SetActive(true);
        animator.SetTrigger("Show");
    }

    public void HideAnimation(GameObject obj, float delay) // 애니메이션 후 숨기기
    {
        Animator animator;
        animator = obj.GetComponent<Animator>();
        animator.SetTrigger("Hide");
        StartCoroutine(ObjActiveFalse(obj, delay));
    }

    IEnumerator ObjActiveFalse(GameObject obj, float delay) // Hide에서 ActiveFalse용 메서드
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}
