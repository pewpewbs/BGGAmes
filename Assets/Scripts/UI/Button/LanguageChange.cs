using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageChange : MonoBehaviour
{
    private TranslationManager translationManager; // Менеджер перекладів
    [SerializeField] private List<TMP_Text> textElements; // Список текстових елементів, які потрібно оновити при зміні мови
    private void Start()
    {
        // Ініціалізація менеджера перекладів та підписка на подію зміни мови
        translationManager = gameObject.AddComponent<TranslationManager>();
        translationManager.OnLanguageChanged += UpdateTranslations;
        translationManager.ChangeLanguage("en"); // Встановити початкову мову
    }

    private void UpdateTranslations()
    {
        // Оновлення всіх текстових елементів при зміні мови
        foreach (var textElement in textElements)
        {
            UpdateTextElement(textElement);
        }
    }
    private void UpdateTextElement(TMP_Text textElement)
    {
        if (textElement != null)
        {
            string translationKey = textElement.name;// Використовуємо ім'я кнопки як ключ перекладу для отримання перекладеного тексту
            string translatedText = translationManager.GetTranslation(translationKey);//Текст кнопки = тексту в таблиці за заданим ключем
            textElement.text = translatedText; // Оновлення текстового елемента
        }
    }

    public void ChangeLanguage(string language)
    {
        translationManager.ChangeLanguage(language);// Зміна мови у менеджері перекладів
    }

    public void OnLanguageButtonClick()// Натискання кнопки
    {
        string nextLanguage = translationManager.CurrentLanguage == "en" ? "uk" : "en";// Зміна мови
        ChangeLanguage(nextLanguage);
    }
}