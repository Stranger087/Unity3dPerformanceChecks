using System.Collections;
using DefaultNamespace.Parameters;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NumberParameterWidget : BaseParameterWidget
{
    [SerializeField] private Slider _Slider;
    [SerializeField] private Text _ValueText;
    public NumberParameter Parameter;


    protected override void LateInit() {
        _Slider.onValueChanged.AddListener(Handler_ValueCHanged);
        _Slider.minValue = Parameter.Min;
        _Slider.maxValue = Parameter.Max;
        _Slider.value = Parameter.Value;
        Parameter.OnSettingsChange += UpdateSettings;
        UpdateVisuals();
    }

    private void UpdateSettings() {
        _Slider.maxValue = Parameter.Max;
        _Slider.value = Mathf.Min(_Slider.value, Parameter.Max);
        OnPointerUp(null);
        UpdateVisuals();
    }

    private void Handler_ValueCHanged(float arg0) {
        UpdateVisuals();
    }

    public override void OnPointerUp(PointerEventData eventData) {

        float val = Mathf.Round(_Slider.value * Parameter.Precision) / Parameter.Precision;
        Parameter.Value = val;
        Parameter.OnChanged(val);
    }
    
    protected override void UpdateVisuals() {
        float val = Mathf.Round(_Slider.value * Parameter.Precision) / Parameter.Precision;
        _ValueText.text = Parameter.Name +": "+ val.ToString();
    }
}