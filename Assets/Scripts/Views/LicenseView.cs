using UnityEngine;
using Hypertext;

public class LicenseView : MonoBehaviour
{
    [SerializeField] private RegexHypertext text;

    private static readonly string regexUrl = @"https?://(?:[!-~]+\.)+[!-~]+";

    void Start()
    {
        text.OnClick(regexUrl, Color.cyan, Application.OpenURL);
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
