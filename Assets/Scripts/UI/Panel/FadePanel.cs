using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadePanel : MonoBehaviour// Клас для обробки затемнення панелі
{
    [SerializeField] private GameObject panel; // Посилання на панель для затемнення
    [SerializeField] private Image fadePanel; // Посилання на Image компонент панелі
    [SerializeField] private MainManager mainManager; // Посилання на головний менеджер для івенту переходу на наступний рівень

    private void Start()
    {
        if (panel != null) // Перевіряємо, чи панель не є null
        {
            fadePanel = panel.GetComponent<Image>(); // Отримуємо компонент Image з панелі
            Debug.Log("Panel assigned successfully."); // Виводимо повідомлення про успішне призначення
        }
        if (fadePanel == null) // Перевіряємо, чи fadePanel не є null
        {
            Debug.LogError("FadePanel image is not assigned and not found in the GameObject."); // Виводимо помилку, якщо fadePanel не знайдено
        }
    }
    public IEnumerator FadeInAnimation(int fadeDuration) // Метод для анімації затемнення
    {
        if (fadePanel != null) // Перевіряємо, чи fadePanel не є null
        {
            yield return StartCoroutine(FadeIn(fadeDuration)); // Виконуємо анімацію затемнення
        }
    }
    public IEnumerator FadeOutAnimation(int fadeDuration) // Метод для анімації освітлення
    {
        if (fadePanel != null) // Перевіряємо, чи fadePanel не є null
        {
            yield return StartCoroutine(FadeOut(fadeDuration)); // Виконуємо анімацію освітлення
        }
    }

    private IEnumerator FadeIn(int fadeDuration) // Скрипт для плавного затемнення
    {
        panel.SetActive(true); // Активуємо панель
        for (float t = 0.01f; t < fadeDuration; t += Time.unscaledDeltaTime) // Цикл для поступового затемнення
        {
            fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, Mathf.Lerp(0, 1, t / fadeDuration)); // Змінюємо прозорість панелі
            yield return null; // Чекаємо наступного кадру
        }
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 1); // Встановлюємо прозорість на 1
    }

    private IEnumerator FadeOut(int fadeDuration) // Скрипт для плавного освітлення
    {
        for (float t = 0.01f; t < fadeDuration; t += Time.unscaledDeltaTime) // Цикл для поступового освітлення
        {
            fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, Mathf.Lerp(1, 0, t / fadeDuration)); // Змінюємо прозорість панелі
            yield return null; // Чекаємо наступного кадру
        }
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0); // Встановлюємо прозорість на 0
        panel.SetActive(false); // Деактивуємо панель
    }

    public void SetActive(bool setActive) // Метод для встановлення активності панелі
    {
        panel.SetActive(setActive); // Встановлюємо активність панелі
    }
}
