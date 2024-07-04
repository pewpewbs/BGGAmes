using UnityEngine;

public class Pause : MonoBehaviour
{
    // Посилання на потрібні кнопки
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject changeLanguageButton;
    [SerializeField] private GameObject shieldButton;

    [SerializeField] private FadePanel fadePanel;// Посилання на панель

    public int fadeDuration = 1;// Тривалість анімації затемнення/освітлення

    private bool isPaused = false; // Стан паузи

    void Start()
    {
        SetPausedState(false); // Встановлюємо початковий стан без паузи
    }

    public void TogglePause()// Перемикаємо стан паузи
    {
        isPaused = !isPaused;
        SetPausedState(isPaused);//Задаємо новий стан

        if (isPaused)
        {
            Time.timeScale = 0; // Зупиняємо гру
            fadePanel.StartCoroutine(fadePanel.FadeInAnimation(fadeDuration)); // Виконуємо анімацію затемнення
        }
        else
        {
            Time.timeScale = 1; // Продовжуємо гру
            fadePanel.StartCoroutine(fadePanel.FadeOutAnimation(fadeDuration)); // Виконуємо анімацію освітлення
        }
    }
    private void SetPausedState(bool paused)// Стан всіх кнопок екрану в різних станах паузи
    {
        pauseButton.SetActive(!paused);
        resumeButton.SetActive(paused);
        exitButton.SetActive(paused);
        changeLanguageButton.SetActive(paused);
        shieldButton.gameObject.SetActive(!paused);
    }
}
