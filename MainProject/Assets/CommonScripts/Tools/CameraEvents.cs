using System;
using UnityEngine;

public class CameraEvents:MonoBehaviour
{
    public event Action OnWillRenderObjectEvent;
    
    private void OnWillRenderObject() {
        Debug.Log("onwilretnfer");
        if (OnWillRenderObjectEvent != null) OnWillRenderObjectEvent.Invoke();
    }
}