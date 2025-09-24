using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    public Slider balanceSlider;
    public Slider protectionSlider;
    public TextMeshProUGUI timerText;
    public GameObject winPanel, losePanel;

    void Awake() { Instance = this; }

    void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    public void UpdateBalance(float current)
    {
        if (balanceSlider) balanceSlider.value = current / 100f;
    }

    public void UpdateProtection(float current)
    {
        if (protectionSlider) protectionSlider.value = current / 100f;
    }

    public void UpdateTimer(float seconds)
    {
        if (timerText) timerText.text = Mathf.CeilToInt(seconds).ToString();
    }

    public void ShowWin() { winPanel.SetActive(true); }
    public void ShowLose(string reason) { losePanel.SetActive(true); }
}
