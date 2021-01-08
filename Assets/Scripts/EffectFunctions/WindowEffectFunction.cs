using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class WindowEffectFunction : MonoBehaviour
{
    public bool IsClosing { get; private set; }

    public async UniTask Open()
    {
        await transform.DOScale(1, 0.25f).From(0);
    }

    public async UniTask Close()
    {
        if (IsClosing)
        {
            return;
        }

        IsClosing = true;
        await transform.DOScale(0, 0.25f);
    }
}
