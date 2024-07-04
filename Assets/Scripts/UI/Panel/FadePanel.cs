using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadePanel : MonoBehaviour// ���� ��� ������� ���������� �����
{
    [SerializeField] private GameObject panel; // ��������� �� ������ ��� ����������
    [SerializeField] private Image fadePanel; // ��������� �� Image ��������� �����
    [SerializeField] private MainManager mainManager; // ��������� �� �������� �������� ��� ������ �������� �� ��������� �����

    private void Start()
    {
        if (panel != null) // ����������, �� ������ �� � null
        {
            fadePanel = panel.GetComponent<Image>(); // �������� ��������� Image � �����
            Debug.Log("Panel assigned successfully."); // �������� ����������� ��� ������ �����������
        }
        if (fadePanel == null) // ����������, �� fadePanel �� � null
        {
            Debug.LogError("FadePanel image is not assigned and not found in the GameObject."); // �������� �������, ���� fadePanel �� ��������
        }
    }
    public IEnumerator FadeInAnimation(int fadeDuration) // ����� ��� ������� ����������
    {
        if (fadePanel != null) // ����������, �� fadePanel �� � null
        {
            yield return StartCoroutine(FadeIn(fadeDuration)); // �������� ������� ����������
        }
    }
    public IEnumerator FadeOutAnimation(int fadeDuration) // ����� ��� ������� ���������
    {
        if (fadePanel != null) // ����������, �� fadePanel �� � null
        {
            yield return StartCoroutine(FadeOut(fadeDuration)); // �������� ������� ���������
        }
    }

    private IEnumerator FadeIn(int fadeDuration) // ������ ��� �������� ����������
    {
        panel.SetActive(true); // �������� ������
        for (float t = 0.01f; t < fadeDuration; t += Time.unscaledDeltaTime) // ���� ��� ����������� ����������
        {
            fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, Mathf.Lerp(0, 1, t / fadeDuration)); // ������� ��������� �����
            yield return null; // ������ ���������� �����
        }
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 1); // ������������ ��������� �� 1
    }

    private IEnumerator FadeOut(int fadeDuration) // ������ ��� �������� ���������
    {
        for (float t = 0.01f; t < fadeDuration; t += Time.unscaledDeltaTime) // ���� ��� ����������� ���������
        {
            fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, Mathf.Lerp(1, 0, t / fadeDuration)); // ������� ��������� �����
            yield return null; // ������ ���������� �����
        }
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0); // ������������ ��������� �� 0
        panel.SetActive(false); // ���������� ������
    }

    public void SetActive(bool setActive) // ����� ��� ������������ ��������� �����
    {
        panel.SetActive(setActive); // ������������ ��������� �����
    }
}
