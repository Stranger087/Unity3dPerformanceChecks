using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class BaseTestManager : MonoBehaviour
    {

        //Static
        public static event Action OnNeedRecalculateMetrics;
        public static event Action<string> OnSwitchNameChanged;

        public List<TestParameter> Parameters = new List<TestParameter>(); 

        protected int DrawsCount;
        protected bool Initialized = false;
        

        protected virtual void OnEnable() {
            DrawsCount = SwitchWidget.DrawCount;
            if (OnNeedRecalculateMetrics != null) OnNeedRecalculateMetrics.Invoke();
            
            SetupParameters();
        }

        protected abstract void SetupParameters();

    }
}