using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using toio;

public class TitleView : MonoBehaviour
{
    [SerializeField] private List<ToioCubePlayerIndicator> toioCubePlayerIndicators;
    [SerializeField] private Button connectButton;
    [SerializeField] private Button startButton;

    private void Start()
    {
        var cubeManager = ToioCubeManagerService.Instance.CubeManager;
        UpdateView(cubeManager);
    }

    public async void OnClickConnect()
    {
        var cubeManager = ToioCubeManagerService.Instance.CubeManager;
        var cube = await cubeManager.SingleConnect();
        if (cube == null)
        {
            return;
        }

        UpdateView(cubeManager);
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
