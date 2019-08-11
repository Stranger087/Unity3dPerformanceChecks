using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class MetricsWIdget : MonoBehaviour
{
    [SerializeField] private Text _Text;

    private const int CALCULATION_PERIOD = 250;
    private const int PERCISION_MULTIPLIER = 100;
    private List<float> _FrameTimes = new List<float>(300);


    private int _ReplaceIndex = 0;
    private float _PrevTime = 0;

    private int _HideUntilReplaceIndex = -1;

    private void OnEnable() {
        BaseTestManager.OnNeedRecalculateMetrics += OnSwitchHappened;
    }


    // Update is called once per frame
    void Update() {
        float currentTime = Time.realtimeSinceStartup;
        float deltaTime = currentTime - _PrevTime;
        _PrevTime = currentTime;

        //store delta time values
        if (_FrameTimes.Count < CALCULATION_PERIOD) {
            _FrameTimes.Add(deltaTime);
        }
        else {
            _FrameTimes[_ReplaceIndex] = deltaTime;
            if (++_ReplaceIndex == CALCULATION_PERIOD - 1) {
                _ReplaceIndex = 0;
            }
        }

        //calculate medium frame time
        float totalTime = 0;
        for (int i = 0; i < _FrameTimes.Count; i++) {
            totalTime += _FrameTimes[i];
        }

        float frameTime = Mathf.Floor(totalTime / _FrameTimes.Count * 1000f*PERCISION_MULTIPLIER) / PERCISION_MULTIPLIER;

        if (_HideUntilReplaceIndex >= 0) {
            if (_HideUntilReplaceIndex == _ReplaceIndex) {
                _HideUntilReplaceIndex = -1;
            }

            
        }
        else {
            _Text.text = SystemInfo.processorFrequency+ " frame time: "+ frameTime;
        }
    }


    private void OnSwitchHappened() {
        _HideUntilReplaceIndex = _ReplaceIndex;
        _Text.text += " ...calculating new value...";
    }
}