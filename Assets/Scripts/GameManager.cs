using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float levelTime = 90f;
    float timeLeft;
    bool finished = false;

    void Awake() { if (Instance == null) Instance = this; else Destroy(gameObject); }

    void Start()
    {
        timeLeft = levelTime;
    }

    void Update()
    {
        if (finished) return;
        timeLeft -= Time.deltaTime;
        UIController.Instance.UpdateTimer(timeLeft);
        if (timeLeft <= 0f) Lose("Time up");
    }

    public void Win()
    {
        finished = true;
        UIController.Instance.ShowWin();
        AudioManager.Instance.PlaySFX("win");
        // stop gameplay, show score etc.
    }

    public void Lose(string reason)
    {
        if (finished) return;
        finished = true;
        UIController.Instance.ShowLose(reason);
        AudioManager.Instance.PlaySFX("lose");
    }
}
