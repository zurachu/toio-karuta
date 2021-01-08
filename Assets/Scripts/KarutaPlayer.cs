using System;
using toio;
using toio.Simulator;

public class KarutaPlayer
{
    private static readonly string callbackKey = "EventScene";

    public StandardID.SimpleCardType? SimpleCardType { get; private set; }
    public bool IsPenalty
    {
        get => isPenalty;
        set
        {
            if (!value)
            {
                cube.PlayPresetSound(ToioSoundUtility.PresetSoundId.Enter);
            }

            if (!isPenalty && value)
            {
                cube.PlayPresetSound(ToioSoundUtility.PresetSoundId.Cancel);
            }

            isPenalty = value;
        }
    }
    private bool isPenalty;

    public int Score { get; private set; }

    private readonly Cube cube;
    private readonly Action<KarutaPlayer, StandardID.SimpleCardType> onTouchedSimpleCard;

    public KarutaPlayer(Cube cube, Action<KarutaPlayer, StandardID.SimpleCardType> onTouchedSimpleCard)
    {
        this.cube = cube;
        this.onTouchedSimpleCard = onTouchedSimpleCard;

        cube.standardIdCallback.RemoveListener(callbackKey);
        cube.standardIdCallback.AddListener(callbackKey, OnUpdateStandardId);
        cube.standardIdMissedCallback.RemoveListener(callbackKey);
        cube.standardIdMissedCallback.AddListener(callbackKey, OnMissedStandardId);
    }

    public void IncrementScore()
    {
        Score++;
        cube.PlayPresetSound(ToioSoundUtility.PresetSoundId.Get1);
    }

    private void OnUpdateStandardId(Cube c)
    {
        if (ToioSimpleCardUtility.IsSimpleCardId(c.standardId))
        {
            SimpleCardType = ToioSimpleCardUtility.TypeOf(c.standardId);

            if (!IsPenalty)
            {
                onTouchedSimpleCard?.Invoke(this, SimpleCardType.Value);
            }
        }
        else
        {
            SimpleCardType = null;
        }
    }

    private void OnMissedStandardId(Cube c)
    {
        SimpleCardType = null;
    }
}
