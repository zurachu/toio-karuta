using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using toio.Simulator;

public class InGameView : MonoBehaviour
{
    [SerializeField] private List<KarutaPlayerIndicator> KarutaPlayerIndicators;
    [SerializeField] private Text releaseFromCardText;
    [SerializeField] private Text readyText;
    [SerializeField] private Text targetText;

    private List<KarutaPlayer> karutaPlayers;
    private List<StandardID.SimpleCardType> targetSimpleCardTypes;
    private StandardID.SimpleCardType? currentTargetSimpleCardType;
    private volatile bool isWithinGame;

    public async void StartGame()
    {
        var cubeManager = ToioCubeManagerService.Instance.CubeManager;
        karutaPlayers = cubeManager.cubes.ConvertAll(_cube => new KarutaPlayer(_cube, OnTouchedSimpleCard));
        targetSimpleCardTypes = ListUtility.Shuffle(ToioSimpleCardUtility.AlphabetTypes);
        isWithinGame = false;
        UpdateView(karutaPlayers);

        for (var endedGameCount = 0; endedGameCount < targetSimpleCardTypes.Count; endedGameCount++)
        {
            await DoOneGame(endedGameCount);
        }
    }

    private async Task DoOneGame(int index)
    {
        isWithinGame = false;
        UIUtility.TrySetActive(releaseFromCardText, false);
        UIUtility.TrySetActive(readyText, false);
        UIUtility.TrySetActive(targetText, false);
        UpdateView(karutaPlayers);

        if (karutaPlayers.Any(_player => _player.SimpleCardType.HasValue))
        {
            UIUtility.TrySetActive(releaseFromCardText, true);
            await UniTask.WaitWhile(() => karutaPlayers.Any(_player => _player.SimpleCardType.HasValue));
            UIUtility.TrySetActive(releaseFromCardText, false);
        }

        isWithinGame = true;
        UIUtility.TrySetActive(readyText, true);
        ResetPenalties();

        await UniTask.Delay(Random.Range(2000, 4000));

        currentTargetSimpleCardType = targetSimpleCardTypes[index];
        karutaPlayers.ForEach(_player => _player.OnDisplayedTarget());
        UIUtility.TrySetActive(readyText, false);
        UIUtility.TrySetActive(targetText, true);
        UIUtility.TrySetText(targetText, ToioSimpleCardUtility.NameOf(currentTargetSimpleCardType.Value));
        await UniTask.WaitWhile(() => isWithinGame);
        await UniTask.Delay(1000);
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
                ResetPenalties(1000);
            }

            return;
        }

        karutaPlayer.IncrementScore();
        UpdateView(karutaPlayers);
        isWithinGame = false;
        currentTargetSimpleCardType = null;
    }

    private async void ResetPenalties(int milliSecond = 0)
    {
        if (milliSecond > 0)
        {
            await UniTask.Delay(milliSecond);
        }

        karutaPlayers.ForEach(_player => _player.IsPenalty = false);
        UpdateView(karutaPlayers);
    }

    private void UpdateView(List<KarutaPlayer> karutaPlayers)
    {
        foreach (var (player, index) in karutaPlayers.WithIndex())
        {
            if (ListUtility.TryGetValue(KarutaPlayerIndicators, index, out var indicator))
            {
                indicator.UpdateView(player);
            }
        }
    }
}
