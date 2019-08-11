using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseParameterWidget : MonoBehaviour, IPointerUpHandler
{
    private void OnEnable() {
        StartCoroutine(LateInit());
    }

    protected abstract IEnumerator LateInit();
    protected abstract void UpdateVisuals();

    public abstract void OnPointerUp(PointerEventData eventData);
}