using UnityEngine;
using UnityEngine.UI;

public class KarutaPlayerIndicator : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject penaltyObject;

    public void UpdateView(KarutaPlayer player)
    {
        UIUtility.TrySetText(scoreText, $"{player.Score}");
        UIUtility.TrySetActive(penaltyObject, player.IsPenalty);
    }
}
