using UnityEngine;

public class SampleScene : MonoBehaviour
{
    [SerializeField] private TitleView titleView;
    [SerializeField] private InGameView inGameView;

    private void Start()
    {
        UIUtility.TrySetActive(titleView, true);
        UIUtility.TrySetActive(inGameView, false);
    }

    public void OnClickStart()
    {
        UIUtility.TrySetActive(titleView, false);
        UIUtility.TrySetActive(inGameView, true);
        inGameView.StartGame();
    }
}
