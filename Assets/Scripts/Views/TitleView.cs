using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using toio;
using KanKikuchi.AudioManager;

public class TitleView : MonoBehaviour
{
    [SerializeField] private RectTransform titleRectTransform;
    [SerializeField] private List<ToioCubePlayerIndicator> toioCubePlayerIndicators;
    [SerializeField] private Button connectButton;
    [SerializeField] private Button startButton;
    [SerializeField] private LicenseView licenseViewPrefab;

    private void Start()
    {
        var cubeManager = ToioCubeManagerService.Instance.CubeManager;
        UpdateView(cubeManager);

        titleRectTransform.DOLocalMoveY(-10, 1).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetRelative(true);
    }

    public async void OnClickConnect()
    {
        SEManager.Instance.Play(SEPath.SMALL_TRANSITION);

        var cubeManager = ToioCubeManagerService.Instance.CubeManager;
        var cube = await cubeManager.SingleConnect();
        if (cube == null)
        {
            return;
        }

        UpdateView(cubeManager);
    }

    public void OnClickLicense()
    {
        Instantiate(licenseViewPrefab, transform.parent);
    }

    private void UpdateView(CubeManager cubeManager)
    {
        UIUtility.TrySetActive(toioCubePlayerIndicators, false);
        foreach (var (cube, index) in cubeManager.cubes.WithIndex())
        {
            ListUtility.TryGetValue(toioCubePlayerIndicators, index, out var indicator);
            if (indicator == null)
            {
                continue;
            }

            UIUtility.TrySetActive(indicator, true);
            indicator.UpdateView(cube, index);
        }

        connectButton.interactable = cubeManager.cubes.Count < toioCubePlayerIndicators.Count;
        startButton.interactable = !ListUtility.IsNullOrEmpty(cubeManager.cubes);
    }
}
