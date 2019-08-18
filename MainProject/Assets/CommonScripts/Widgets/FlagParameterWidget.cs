using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace.Widgets
{
    public class FlagParameterWidget : BaseParameterWidget
    {
        [SerializeField] private Text _Text;
        public FlagParameter Parameter;


        protected override void LateInit() {
            UpdateVisuals();
        }

        protected override void UpdateVisuals() {
            _Text.text = Parameter.Name + (Parameter.Checked?" ✓":"");
        }

        public override void OnPointerUp(PointerEventData eventData) {
            Parameter.Checked = !Parameter.Checked;
            Parameter.OnChanged(Parameter.Checked);
            UpdateVisuals();
        }
    }
}