using System.Collections.Generic;
using System.Xml;
using UnityEngine;
public class TranslationManager : MonoBehaviour// ���� ��� ��������� ����������� � ��
{
    // ������� � ���� ��� ���� ����
    public delegate void LanguageChangedEventHandler();
    public event LanguageChangedEventHandler OnLanguageChanged;

    private Dictionary<string, Dictionary<string, string>> translations;// ������� ��� ��������� ���������
    
    private string currentLanguage;// ������� ����
    public string CurrentLanguage => currentLanguage;// ������ ��� ������� ����

    private void Awake()
    {
        translations = new Dictionary<string, Dictionary<string, string>>();// ���������� ������� ���������

        LoadTranslations();// ����������� ���������
    }
    public void LoadTranslations()// ����� ��� ������������ ���������
    {
        TextAsset xmlAsset = Resources.Load<TextAsset>("Translations");// ���� �� XML-�����
        XmlDocument xmlDoc = new();
        xmlDoc.LoadXml(xmlAsset.text);// ������������ ��������� � XML-�����

        XmlNodeList languageNodes = xmlDoc.DocumentElement.SelectNodes("/translations/language");// �������� �� ����� ���

        foreach (XmlNode languageNode in languageNodes)// ��������� ����� ����� ����� ����
        {
            // �������� ��� ����
            string language = languageNode.Attributes["code"].Value;
            Dictionary<string, string> languageTranslations = new();

            foreach (XmlNode translationNode in languageNode.SelectNodes("translation"))// �������� �� ����� ��������� ��� ���� ����
            {
                // �������� ���� � �������� ���������
                string key = translationNode.Attributes["key"].Value;
                string value = translationNode.InnerText;
                languageTranslations[key] = value;
            }
            translations[language] = languageTranslations;// ������ ��������� ���� �� ��������� ��������
        }
    }
    public void ChangeLanguage(string language)// ����� ��� ���� ����
    {
        if (translations.ContainsKey(language))// ����������, �� � ��������� ��� ������� ����
        {
            currentLanguage = language;// ������� ������� ����

            OnLanguageChanged?.Invoke();// ��������� ���� ���� ����
        }
        else
        {
            Debug.LogWarning($"Language '{language}' not found in translations.");// �������� ������������, ���� ���� �� ��������
        }
    }
    public string GetTranslation(string key)// ����� ��� ��������� ��������� �� ������
    {
        // ����������, �� � �������� ��� ������� ���� � �����
        if (translations.ContainsKey(currentLanguage) && translations[currentLanguage].ContainsKey(key))
        {
            return translations[currentLanguage][key];// ��������� ����������� ��������
        }
        return key;// ��������� ����, ���� �������� �� ��������
    }
}
