using UnityEngine;
using UnityEngine.UI;
using Hypertext;

public class LicenseView : MonoBehaviour
{
    [SerializeField] private WindowEffectFunction window;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RegexHypertext text;
    [SerializeField] private TextAsset textAsset;

    private static readonly string regexUrl = @"https?://(?:[!-~]+\.)+[!-~]+";

    private async void Start()
    {
        text.text = textAsset.text;
        text.OnClick(regexUrl, Color.cyan, Application.OpenURL);

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
