using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Клас для обробки щита, який активується натисканням кнопки
public class Shield : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private PlayerManager player; // Посилання на PlayerManager для активації захисту

    public float holdDuration = 2f; // Тривалість утримання кнопки в секундах
    private bool isHolding = false; // Відслідковує, чи утримується кнопка
    private bool holdLimit = false; // Ліміт утримання (заборона натискання після використання)
    private float holdTime = 0f; // Час утримання кнопки

    private Button button; // Посилання на компонент кнопки
    void Start()
    {
        button = GetComponent<Button>(); // Знаходить компонент Button на цьому об'єкті
        if (button == null)
        {
            Debug.LogError("Button component not found on this GameObject."); // Виводить помилку, якщо кнопка не знайдена
        }
    }

    void Update()
    {
        if (isHolding && !holdLimit) // Якщо кнопка утримується і немає ліміту утримання
        {
            holdTime += Time.unscaledDeltaTime; // Додає час утримання без врахування Time.timeScale
            if (holdTime >= holdDuration) // Якщо час утримання перевищує тривалість утримання
            {
                player.DeactivateShield(); // Деактивує щит у гравця
                holdLimit = true; // Встановлює позначку що вказує шо ліміту утримання досягнуто
            }
        }
        if (holdLimit) // Якщо ліміту утримання досягнуто
        {
            holdTime += Time.unscaledDeltaTime; // Додає час утримання без врахування Time.timeScale
            if (holdTime >= holdDuration) // Якщо час утримання перевищує тривалість утримання
            {
                holdLimit = false; // Анулюємо ліміт
                holdTime = 0f; // Скидає час утримання
                button.interactable = true; // Робить кнопку знову доступною для натискання
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)//При нажатті кнопки
    {
        if (!isHolding && !holdLimit) // Якщо кнопка не утримується і немає ліміту утримання
        {
            player.ActivateShield(); // Активує щит у гравця
            isHolding = true; // Встановлює стан утримання
        }
    }
    public void OnPointerUp(PointerEventData eventData)//При віджатті кнопки
    {
        isHolding = false; // Вимикає стан утримання
        player.DeactivateShield(); // Деактивує щит у гравця
        holdTime = 0f; // Скидає час утримання
    }
    public void ResetShield()
    {
        isHolding = false; // Вимикає стан утримання
        holdLimit = false; // Вимикає ліміт утримання
        holdTime = 0f; // Скидає час утримання
        button.interactable = true; // Робить кнопку знову доступною для натискання
    }
}
