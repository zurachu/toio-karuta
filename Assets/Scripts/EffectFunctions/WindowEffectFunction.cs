using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class WindowEffectFunction : MonoBehaviour
{
    public bool IsClosing { get; private set; }

    private void Start()
    {
        transform.DOScale(1, 0.25f).From(0);
    }

    public async UniTask Close()
    {
        IsClosing = true;
        await transform.DOScale(0, 0.25f);
    }
}
