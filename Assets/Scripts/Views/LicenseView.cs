using UnityEngine;
using UnityEngine.UI;

public class LicenseView : MonoBehaviour
{
    [SerializeField] private WindowEffectFunction window;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Text text;
    [SerializeField] private TextAsset textAsset;

    private async void Start()
    {
        UIUtility.TrySetText(text, textAsset.text);

        var scrollbar = scrollRect.verticalScrollbar;
        scrollRect.verticalScrollbar = null;
        await window.Open();
        scrollRect.verticalScrollbar = scrollbar;
    }

    public async void Close()
    {
        if (window.IsClosing)
        {
            return;
        }

        await window.Close();
        Destroy(gameObject);
    }
}
