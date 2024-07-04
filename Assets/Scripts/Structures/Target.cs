using UnityEngine;
using System.Collections;

// ���� ��� ������������� ��� � ��
public class Target : MonoBehaviour
{
    public GameObject confettiPrefab; // ������ �������
    public MainManager mainManager; // ��������� �� �������� �������� ���
    public FadePanel fadePanel; // ��������� �� ������ ����������
    private void Start()
    {
        if (mainManager == null) // ����������, �� �������� �������� �� � null
        {
            mainManager = FindObjectOfType<MainManager>(); // ��������� �������� �������� � ����
            if (mainManager == null)
            {
                Debug.LogError("MainManager not found!"); // �������� �������, ���� �������� �������� �� ���������
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player") // ����������, �� ��'���, �� �������� � �����, � �������
        {
            Debug.Log("You win"); // �������� ����������� ��� ��������
            Instantiate(confettiPrefab, other.transform.position, Quaternion.identity); // ��������� ������� � ������� ������
            fadePanel.StartCoroutine(HandleWinSequence()); // ��������� �������� ��� ������� ����������� �������
        }
    }

    private IEnumerator HandleWinSequence()
    {
        yield return fadePanel.StartCoroutine(fadePanel.FadeInAnimation(1));// ��������� ����� ������� ���������

        mainManager.onNextLevel.Invoke();// ��������� ����� �������� �� ��������� �����
        
        yield return new WaitForSeconds(1f);// �������� ����� ������ ��������

        yield return fadePanel.StartCoroutine(fadePanel.FadeOutAnimation(1));// ��������� ����� �������
    }
    public void SetPosition(Vector2Int position)
    {
        Vector3 spawnPos = new(position.x, 1, position.y); // ������������ ������� ��� �� ���� �������
        gameObject.transform.position = spawnPos; // ������� ������� ��'����
    }
}
