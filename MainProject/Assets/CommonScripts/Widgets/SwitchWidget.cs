using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWidget : MonoBehaviour
{
    public static event Action OnSwitchTriggered;
    public static event Action OnDrawCountChanged;
    public static int DrawCount;

    [SerializeField] private Button SwitchButton;
    [SerializeField] private Button[] DrawCountButtons;
    [SerializeField] private int[] DrawCounts;

    private void OnEnable() {
        SwitchButton.onClick.AddListener(() => {
            if (OnSwitchTriggered != null) OnSwitchTriggered();
        });

        for (int i = 0; i < DrawCountButtons.Length; i++) {
            var saveI = i;
            DrawCountButtons[i].onClick.AddListener(() => ChangeDrawsCount(saveI));
        }

        DrawCount = DrawCounts[0];
        Refresh();

        BaseTestManager.OnSwitchNameChanged += Handler_SwitchNameChanged;
    }

    private void Handler_SwitchNameChanged(string switchName) {
        SwitchButton.GetComponentInChildren<Text>().text = switchName;
    }

    private void ChangeDrawsCount(int index) {
        DrawCount = DrawCounts[index];
        if (OnDrawCountChanged != null) OnDrawCountChanged.Invoke();
        Refresh();
    }

    private void Refresh() {
        for (int i = 0; i < DrawCountButtons.Length; i++) {
            DrawCountButtons[i].GetComponentInChildren<Text>().text = DrawCounts[i].ToString();
            DrawCountButtons[i].GetComponentInChildren<Text>().color = DrawCount == DrawCounts[i] ? Color.green : Color.black;
        }
    }
}