using UnityEngine;

public class Pause : MonoBehaviour
{
    // ��������� �� ������ ������
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject changeLanguageButton;
    [SerializeField] private GameObject shieldButton;

    [SerializeField] private FadePanel fadePanel;// ��������� �� ������

    public int fadeDuration = 1;// ��������� ������� ����������/���������

    private bool isPaused = false; // ���� �����

    void Start()
    {
        SetPausedState(false); // ������������ ���������� ���� ��� �����
    }

    public void TogglePause()// ���������� ���� �����
    {
        isPaused = !isPaused;
        SetPausedState(isPaused);//������ ����� ����

        if (isPaused)
        {
            Time.timeScale = 0; // ��������� ���
            fadePanel.StartCoroutine(fadePanel.FadeInAnimation(fadeDuration)); // �������� ������� ����������
        }
        else
        {
            Time.timeScale = 1; // ���������� ���
            fadePanel.StartCoroutine(fadePanel.FadeOutAnimation(fadeDuration)); // �������� ������� ���������
        }
    }
    private void SetPausedState(bool paused)// ���� ��� ������ ������ � ����� ������ �����
    {
        pauseButton.SetActive(!paused);
        resumeButton.SetActive(paused);
        exitButton.SetActive(paused);
        changeLanguageButton.SetActive(paused);
        shieldButton.gameObject.SetActive(!paused);
    }
}
