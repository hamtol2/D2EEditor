using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallerComponent : MonoBehaviour
{
    void Start()
    {
        int childCount = transform.childCount;
        TestComponent[] tests = transform.GetChild(0).GetComponentsInChildren<TestComponent>(true);
        Debug.Log(tests.Length);
    }
}