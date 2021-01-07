using UnityEngine;
using Hypertext;

public class LicenseView : MonoBehaviour
{
    [SerializeField] private WindowEffectFunction window;
    [SerializeField] private RegexHypertext text;

    private static readonly string regexUrl = @"https?://(?:[!-~]+\.)+[!-~]+";

    private void Start()
    {
        text.OnClick(regexUrl, Color.cyan, Application.OpenURL);
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
