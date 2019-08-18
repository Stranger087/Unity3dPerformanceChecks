using System;
using DefaultNamespace;
using DefaultNamespace.Parameters;
using DefaultNamespace.Widgets;
using UnityEngine;

public class TestParametersWidget : MonoBehaviour
{
    public static TestParametersWidget Instance;


    [SerializeField] private GameObject FlagBtnPrefab;
    [SerializeField] private GameObject NumberBtnPrefab;
    [SerializeField] private GameObject VariantBtnPrefab;

    [SerializeField] private Transform _ButtonsContainer;

    private void Start() {
        Instance = this;
    }

    public void SetTest(BaseTestManager test) {
        _ButtonsContainer.DestroyChildren();
        foreach (TestParameter parameter in test.Parameters) {
            switch (parameter.Type) {
                case ParameterType.SelectVariant:


                    Instantiate(VariantBtnPrefab, _ButtonsContainer).GetComponent<VariantParameterWidget>().Parameter = (VariantParameter) parameter;


                    break;
                case ParameterType.Flag:

                    Instantiate(FlagBtnPrefab, _ButtonsContainer).GetComponent<FlagParameterWidget>().Parameter = (FlagParameter) parameter;

                    break;

                case ParameterType.Slider:

                    Instantiate(NumberBtnPrefab, _ButtonsContainer).GetComponentInChildren<NumberParameterWidget>().Parameter = (NumberParameter) parameter;

                    break;

                default:
                    throw new NotImplementedException();
                    break;
            }
        }
    }
}