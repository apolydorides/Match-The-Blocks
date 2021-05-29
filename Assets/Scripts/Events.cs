using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Events : MonoBehaviour
{
    
    public static Events current;

    private void Awake()
        {
            current = this;
        }
    

    // generating the new arrow block
    public event Action onGenerateArrow;
    public void GenerateArrow()
    {
        if (onGenerateArrow != null)
        {
            onGenerateArrow();
        }
    }

    // move the new arrow block to center position
    public event Action onMoveToCenter;
    public void MoveToCenter()
    {
        if (onMoveToCenter != null)
        {
            onMoveToCenter();
        }
    }

    // fade arrow block to invisible
    public event Action onDisappear;
    public void Disappear()
    {
        if (onDisappear != null)
        {
            onDisappear();
        }
    }    

}
