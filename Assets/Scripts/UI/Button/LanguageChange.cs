using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageChange : MonoBehaviour
{
    private TranslationManager translationManager; // �������� ���������
    [SerializeField] private List<TMP_Text> textElements; // ������ ��������� ��������, �� ������� ������� ��� ��� ����
    private void Start()
    {
        // ����������� ��������� ��������� �� ������� �� ���� ���� ����
        translationManager = gameObject.AddComponent<TranslationManager>();
        translationManager.OnLanguageChanged += UpdateTranslations;
        translationManager.ChangeLanguage("en"); // ���������� ��������� ����
    }

    private void UpdateTranslations()
    {
        // ��������� ��� ��������� �������� ��� ��� ����
        foreach (var textElement in textElements)
        {
            UpdateTextElement(textElement);
        }
    }
    private void UpdateTextElement(TMP_Text textElement)
    {
        if (textElement != null)
        {
            string translationKey = textElement.name;// ������������� ��'� ������ �� ���� ��������� ��� ��������� ������������� ������
            string translatedText = translationManager.GetTranslation(translationKey);//����� ������ = ������ � ������� �� ������� ������
            textElement.text = translatedText; // ��������� ���������� ��������
        }
    }

    public void ChangeLanguage(string language)
    {
        translationManager.ChangeLanguage(language);// ���� ���� � �������� ���������
    }

    public void OnLanguageButtonClick()// ���������� ������
    {
        string nextLanguage = translationManager.CurrentLanguage == "en" ? "uk" : "en";// ���� ����
        ChangeLanguage(nextLanguage);
    }
}