using System;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class BaseTestManager : MonoBehaviour
    {

        public static event Action OnNeedRecalculateMetrics;
        public static event Action<string> OnSwitchNameChanged;

        protected int DrawsCount;
        

        protected virtual void OnEnable() {
            DrawsCount = SwitchWidget.DrawCount;
            SwitchWidget.OnSwitchTriggered += () => Handler_SwitchTriggered();
            SwitchWidget.OnDrawCountChanged += Handler_DrawCountChanged;
            if (OnNeedRecalculateMetrics != null) OnNeedRecalculateMetrics.Invoke();
            
            
        }

        private void Handler_DrawCountChanged() {
            DrawsCount = SwitchWidget.DrawCount;
            OnDrawsCountChanged();
            if (OnNeedRecalculateMetrics != null) OnNeedRecalculateMetrics.Invoke();
        }

        protected abstract void OnDrawsCountChanged();

        private void OnDisable() {
            SwitchWidget.OnSwitchTriggered -= () => Handler_SwitchTriggered();            
        }

        protected virtual void Handler_SwitchTriggered() {
            if (OnNeedRecalculateMetrics != null) OnNeedRecalculateMetrics.Invoke();
        }

        protected void SetSwitchName(string switchName) {
            if (OnSwitchNameChanged != null) OnSwitchNameChanged.Invoke(switchName);
        }
    }
}