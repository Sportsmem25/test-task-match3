using UnityEngine;
using System.Collections;

public class ComboPopupController : MonoBehaviour
{
    public static ComboPopupController Instance;
    [SerializeField] private Transform root;
    [SerializeField] private GameObject popupPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowPopup(string text)
    {
        GameObject popup = Instantiate(popupPrefab, root);
        TMPro.TextMeshProUGUI label = popup.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        
        label.text = text;
        StartCoroutine(AutoDestroyPopup(popup));
    }

    private IEnumerator AutoDestroyPopup(GameObject popup)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(popup);
    }
}