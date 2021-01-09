using UnityEngine;
using UnityEngine.UI;

public class SampleScene : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasScaler canvasScaler;
    [SerializeField] private TitleView titleView;
    [SerializeField] private InGameView inGameView;
    [SerializeField] private GameObject finishView;

    private void Start()
    {
#if UNITY_EDITOR
        canvasGroup.alpha = 0.5f;
#else
        canvasGroup.alpha = 1f;
#endif

        UIUtility.TrySetActive(titleView, true);
        UIUtility.TrySetActive(inGameView, false);
        UIUtility.TrySetActive(finishView, false);
    }

    private void Update()
    {
        var screenAspectRatio = (float)Screen.width / Screen.height;
        var canvasAspectRatio = canvasScaler.referenceResolution.x / canvasScaler.referenceResolution.y;
        canvasScaler.matchWidthOrHeight = (screenAspectRatio < canvasAspectRatio) ? 0 : 1;
    }

    public void OnClickStart()
    {
        UIUtility.TrySetActive(titleView, false);
        UIUtility.TrySetActive(inGameView, true);
        UIUtility.TrySetActive(finishView, false);
        inGameView.StartGame(FinishGame);
    }

    private void FinishGame()
    {
        UIUtility.TrySetActive(titleView, false);
        UIUtility.TrySetActive(inGameView, false);
        UIUtility.TrySetActive(finishView, true);
    }
}
