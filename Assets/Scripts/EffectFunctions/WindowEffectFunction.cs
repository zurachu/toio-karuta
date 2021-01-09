using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using KanKikuchi.AudioManager;

public class WindowEffectFunction : MonoBehaviour
{
    public bool IsClosing { get; private set; }

    public async UniTask Open()
    {
        SEManager.Instance.Play(SEPath.OPEN_MODAL);
        await transform.DOScale(1, 0.25f).From(0);
    }

    public async UniTask Close()
    {
        if (IsClosing)
        {
            return;
        }

        IsClosing = true;
        SEManager.Instance.Play(SEPath.CANCEL);
        await transform.DOScale(0, 0.25f);
    }
}
