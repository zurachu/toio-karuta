using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class KarutaPlayerIndicator : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject penaltyObject;
    [SerializeField] private GameObject winObject;

    public void UpdateView(KarutaPlayer player)
    {
        var newScoreText = $"{player.Score}";
        if (scoreText.text != newScoreText)
        {
            StartScoreAnimation();
        }

        UIUtility.TrySetText(scoreText, newScoreText);

        if (!penaltyObject.activeSelf && player.IsPenalty)
        {
            StartPenaltyAnimation();
        }

        UIUtility.TrySetActive(penaltyObject, player.IsPenalty);
        UIUtility.TrySetActive(winObject, player.IsWin);
    }

    private async void StartScoreAnimation()
    {
        await scoreText.transform.DOScale(1.5f, 0.25f);
        await scoreText.transform.DOScale(1f, 0.25f);
    }

    private async void StartPenaltyAnimation()
    {
        await penaltyObject.transform.DOScale(1f, 0.25f).From(3f);
    }
}
