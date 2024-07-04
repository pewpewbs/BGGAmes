using System.Collections.Generic;
using System.Xml;
using UnityEngine;
public class TranslationManager : MonoBehaviour// Клас для управління перекладами у грі
{
    // Делегат і подія для зміни мови
    public delegate void LanguageChangedEventHandler();
    public event LanguageChangedEventHandler OnLanguageChanged;

    private Dictionary<string, Dictionary<string, string>> translations;// Словник для зберігання перекладів
    
    private string currentLanguage;// Поточна мова
    public string CurrentLanguage => currentLanguage;// Геттер для поточної мови

    private void Awake()
    {
        translations = new Dictionary<string, Dictionary<string, string>>();// Ініціалізуємо словник перекладів

        LoadTranslations();// Завантажуємо переклади
    }
    public void LoadTranslations()// Метод для завантаження перекладів
    {
        TextAsset xmlAsset = Resources.Load<TextAsset>("Translations");// Шлях до XML-файлу
        XmlDocument xmlDoc = new();
        xmlDoc.LoadXml(xmlAsset.text);// Завантаження перекладів з XML-файлу

        XmlNodeList languageNodes = xmlDoc.DocumentElement.SelectNodes("/translations/language");// Отримуємо всі вузли мов

        foreach (XmlNode languageNode in languageNodes)// Проходимо через кожен вузол мови
        {
            // Отримуємо код мови
            string language = languageNode.Attributes["code"].Value;
            Dictionary<string, string> languageTranslations = new();

            foreach (XmlNode translationNode in languageNode.SelectNodes("translation"))// Отримуємо всі вузли перекладів для даної мови
            {
                // Отримуємо ключ і значення перекладу
                string key = translationNode.Attributes["key"].Value;
                string value = translationNode.InnerText;
                languageTranslations[key] = value;
            }
            translations[language] = languageTranslations;// Додаємо переклади мови до основного словника
        }
    }
    public void ChangeLanguage(string language)// Метод для зміни мови
    {
        if (translations.ContainsKey(language))// Перевіряємо, чи є переклади для вказаної мови
        {
            currentLanguage = language;// Змінюємо поточну мову

            OnLanguageChanged?.Invoke();// Викликаємо подію зміни мови
        }
        else
        {
            Debug.LogWarning($"Language '{language}' not found in translations.");// Виводимо попередження, якщо мова не знайдена
        }
    }
    public string GetTranslation(string key)// Метод для отримання перекладу за ключем
    {
        // Перевіряємо, чи є переклад для поточної мови і ключа
        if (translations.ContainsKey(currentLanguage) && translations[currentLanguage].ContainsKey(key))
        {
            return translations[currentLanguage][key];// Повертаємо перекладене значення
        }
        return key;// Повертаємо ключ, якщо переклад не знайдено
    }
}
