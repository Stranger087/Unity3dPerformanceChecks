using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Widgets
{
    public class ChooseTestWidget : MonoBehaviour
    {
        [SerializeField] private GameObject BtnPrefab;
        [SerializeField] private GameObject[] TestObjects;

        private GameObject _prevTest;

        private void Start() {
            for (int i = 0; i < TestObjects.Length; i++) {
                var btn = Instantiate(BtnPrefab, transform);
                var saveI = i;
                btn.GetComponent<Button>().onClick.AddListener(() => SelectTest(saveI));
                btn.GetComponentInChildren<Text>().text = TestObjects[i].name;
            }
        }

        private void SelectTest(int index) {
            if (_prevTest != null)
                _prevTest.SetActive(false);
            TestObjects[index].SetActive(true);
            _prevTest = TestObjects[index];

            TestParametersWidget.Instance.SetTest(TestObjects[index].GetComponent<BaseTestManager>());
        }
    }
}