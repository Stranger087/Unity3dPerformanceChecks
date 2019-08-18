using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseParameterWidget : MonoBehaviour, IPointerUpHandler
{
    private void OnEnable() {
        StartCoroutine(InternalLateInit());
    }

    private IEnumerator InternalLateInit() {
        yield return 0;
        LateInit();
    }
    
    protected abstract void LateInit();
    protected abstract void UpdateVisuals();

    public abstract void OnPointerUp(PointerEventData eventData);
    
    
}