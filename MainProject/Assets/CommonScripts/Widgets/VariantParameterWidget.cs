using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VariantParameterWidget : BaseParameterWidget
{
    [SerializeField] private Text _Text;
    [SerializeField] private GameObject _VariantBtnPrefab;
    public VariantParameter Parameter;

    protected override void LateInit() {
        UpdateVisuals();
    }

    protected override void UpdateVisuals() {
        _Text.text = Parameter.Name + ": " + Parameter.Variants[Parameter.CurrentIndex];
    }

    public override void OnPointerUp(PointerEventData eventData) {
        var chooseWindow = ChooseVariantWindow.Instance;

        chooseWindow.transform.GetComponentsInChildren<Button>().ToList().ForEach(_ => Destroy(_.gameObject));

        for (int i = 0; i < Parameter.Variants.Count; i++) {
            int saveI = i;
            var cBtn = Instantiate(_VariantBtnPrefab, chooseWindow.transform).GetComponent<Button>();
            cBtn.GetComponentInChildren<Text>().text = Parameter.Variants[i].ToString();
            cBtn.onClick.AddListener(() => {
                if (Parameter.CurrentIndex != saveI) {
                    Parameter.CurrentIndex = saveI;
                    Parameter.OnChanged(Parameter.Variants[Parameter.CurrentIndex]);
                    UpdateVisuals();
                }

                chooseWindow.SetActive(false);
            });
        }

        chooseWindow.SetActive(true);
    }

}