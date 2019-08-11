using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace.Widgets
{
    public class FlagParameterWidget : BaseParameterWidget
    {
        [SerializeField] private GameObject _CheckView;
        public FlagParameter Parameter;


        protected override IEnumerator LateInit() {
            UpdateVisuals();
            yield break;
        }

        protected override void UpdateVisuals() {
            _CheckView.SetActive(Parameter.Checked);
        }

        public override void OnPointerUp(PointerEventData eventData) {
            Parameter.Checked = !Parameter.Checked;
            Parameter.OnChanged(Parameter.Checked);
            UpdateVisuals();
        }
    }
}