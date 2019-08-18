using UnityEngine;

public class ChooseVariantWindow:MonoBehaviour
{

    public static GameObject Instance;
    private void Start() {
        Instance = gameObject;
        gameObject.SetActive(false);
    }
}