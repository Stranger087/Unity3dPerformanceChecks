using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Widgets
{
    public class ChooseVariantWidget : MonoBehaviour
    {
        public static ChooseVariantWidget Instance;

        [SerializeField] private GameObject _ButtonPrefab;
        [SerializeField] private GameObject _ChooseWindow;

        private void Awake() {
            Instance = this;
        }

        public void RegisterVariation(Action<int> callback, string buttonName, string[] variants) {
            Button btn = Instantiate(_ButtonPrefab.gameObject, transform).GetComponent<Button>();
            btn.name = buttonName;
            btn.GetComponentInChildren<Text>().text = buttonName;
            btn.onClick.AddListener(() => {
                
                
                _ChooseWindow.transform.GetComponentsInChildren<Button>().ToList().ForEach(_=>Destroy(_.gameObject));

                for (int i = 0; i < variants.Length; i++) {
                    int saveI = i;
                    var cBtn = Instantiate(_ButtonPrefab, _ChooseWindow.transform).GetComponent<Button>();
                    cBtn.GetComponentInChildren<Text>().text = variants[i];
                    cBtn.onClick.AddListener(()=> {
                        callback(saveI);
                        _ChooseWindow.SetActive(false);
                    });
                }
                
                _ChooseWindow.SetActive(true);
                
            });
        }
    }
}