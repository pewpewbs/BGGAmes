using UnityEngine;
using System.Collections;

// Клас для представлення цілі у грі
public class Target : MonoBehaviour
{
    public GameObject confettiPrefab; // Префаб конфетті
    public MainManager mainManager; // Посилання на головний менеджер гри
    public FadePanel fadePanel; // Посилання на панель затемнення
    private void Start()
    {
        if (mainManager == null) // Перевіряємо, чи головний менеджер не є null
        {
            mainManager = FindObjectOfType<MainManager>(); // Знаходимо головний менеджер у сцені
            if (mainManager == null)
            {
                Debug.LogError("MainManager not found!"); // Виводимо помилку, якщо головний менеджер не знайдений
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player") // Перевіряємо, чи об'єкт, що зіткнувся з ціллю, є гравцем
        {
            Debug.Log("You win"); // Виводимо повідомлення про перемогу
            Instantiate(confettiPrefab, other.transform.position, Quaternion.identity); // Створюємо конфетті в позиції гравця
            fadePanel.StartCoroutine(HandleWinSequence()); // Запускаємо корутину для обробки послідовності виграшу
        }
    }

    private IEnumerator HandleWinSequence()
    {
        yield return fadePanel.StartCoroutine(fadePanel.FadeInAnimation(1));// Викликаємо першу анімацію затухання

        mainManager.onNextLevel.Invoke();// Викликаємо івент переходу на наступний рівень
        
        yield return new WaitForSeconds(1f);// Затримка перед другою анімацією

        yield return fadePanel.StartCoroutine(fadePanel.FadeOutAnimation(1));// Викликаємо другу анімацію
    }
    public void SetPosition(Vector2Int position)
    {
        Vector3 spawnPos = new(position.x, 1, position.y); // Встановлюємо позицію цілі на нову позицію
        gameObject.transform.position = spawnPos; // Змінюємо позицію об'єкта
    }
}
