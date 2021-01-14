using UnityEngine;
using UnityEngine.UI;
using toio;

public class ToioCubePlayerIndicator : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private Image image;
    [SerializeField] private Text text;

    private void Start()
    {
        UIUtility.TrySetColor(image, color);
    }

    public void UpdateView(Cube cube, int index)
    {
        UIUtility.TrySetText(text, $"プレイヤー{index + 1}");
        ToioLedUtility.TurnLedOn(cube, color, 0);
    }
}
