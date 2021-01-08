using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using toio.Simulator;

public class InGameView : MonoBehaviour
{
    [SerializeField] private List<KarutaPlayerIndicator> karutaPlayerIndicators;
    [SerializeField] private Text releaseFromCardText;
    [SerializeField] private Text readyText;
    [SerializeField] private Image targetImage;
    [SerializeField] private Text targetText;
    [SerializeField] private Text remainingCountText;

    private List<KarutaPlayer> karutaPlayers;
    private List<StandardID.SimpleCardType> targetSimpleCardTypes;
    private StandardID.SimpleCardType? currentTargetSimpleCardType;
    private volatile bool isWithinGame;

    private void Start()
    {
        releaseFromCardText.DOFade(0, 1).SetLoops(-1, LoopType.Yoyo);
    }

    public async void StartGame()
    {
        var cubeManager = ToioCubeManagerService.Instance.CubeManager;
        karutaPlayers = cubeManager.cubes.ConvertAll(_cube => new KarutaPlayer(_cube, OnTouchedSimpleCard));
        targetSimpleCardTypes = ListUtility.Shuffle(ToioSimpleCardUtility.AlphabetTypes);
        isWithinGame = false;
        UpdateView(karutaPlayers);

        for (var endedGameCount = 0; endedGameCount < targetSimpleCardTypes.Count; endedGameCount++)
        {
            UpdateRemainingCountText(endedGameCount);
            await DoOneGame(endedGameCount);
            UpdateRemainingCountText(endedGameCount);
        }
    }

    private async UniTask DoOneGame(int index)
    {
        isWithinGame = false;
        UIUtility.TrySetActive(releaseFromCardText, false);
        UIUtility.TrySetActive(readyText, false);
        UIUtility.TrySetActive(targetImage, false);
        UpdateView(karutaPlayers);

        await ResetPenalties();

        isWithinGame = true;
        UIUtility.TrySetActive(readyText, true);

        await UniTask.Delay(Random.Range(2000, 4000));

        currentTargetSimpleCardType = targetSimpleCardTypes[index];
        karutaPlayers.ForEach(_player => _player.OnDisplayedTarget());
        UIUtility.TrySetActive(readyText, false);
        ShowTarget(currentTargetSimpleCardType.Value);
        await UniTask.WaitWhile(() => isWithinGame);
        await UniTask.Delay(1000);
    }

    private void ShowTarget(StandardID.SimpleCardType simpleCardType)
    {
        UIUtility.TrySetActive(targetImage, true);
        targetImage.sprite = ToioSimpleCardUtility.SpriteOf(simpleCardType);
        UIUtility.TrySetText(targetText, ToioSimpleCardUtility.NameOf(simpleCardType));
    }

    private void OnTouchedSimpleCard(KarutaPlayer karutaPlayer, StandardID.SimpleCardType simpleCardType)
    {
        if (!isWithinGame || karutaPlayer.IsPenalty || !ToioSimpleCardUtility.IsAlphabet(simpleCardType))
        {
            return;
        }

        if (!currentTargetSimpleCardType.HasValue || currentTargetSimpleCardType.Value != simpleCardType)
        {
            karutaPlayer.IsPenalty = true;
            UpdateView(karutaPlayers);
            if (karutaPlayers.TrueForAll(_player => _player.IsPenalty))
            {
                _ = ResetPenalties(1000);
            }

            return;
        }

        karutaPlayer.IncrementScore();
        UpdateView(karutaPlayers);
        isWithinGame = false;
        currentTargetSimpleCardType = null;
    }

    private async UniTask ResetPenalties(int milliSecond = 0)
    {
        if (milliSecond > 0)
        {
            await UniTask.Delay(milliSecond);
        }

        if (karutaPlayers.Any(_player => _player.SimpleCardType.HasValue))
        {
            UIUtility.TrySetActive(releaseFromCardText, true);
            await UniTask.WaitWhile(() => karutaPlayers.Any(_player => _player.SimpleCardType.HasValue));
            UIUtility.TrySetActive(releaseFromCardText, false);
        }

        karutaPlayers.ForEach(_player => _player.IsPenalty = false);
        UpdateView(karutaPlayers);
    }

    private void UpdateView(List<KarutaPlayer> karutaPlayers)
    {
        foreach (var (player, index) in karutaPlayers.WithIndex())
        {
            if (ListUtility.TryGetValue(karutaPlayerIndicators, index, out var indicator))
            {
                indicator.UpdateView(player);
            }
        }
    }

    private void UpdateRemainingCountText(int endedGameCount)
    {
        UIUtility.TrySetText(remainingCountText, $"{targetSimpleCardTypes.Count - endedGameCount}");
    }
}
