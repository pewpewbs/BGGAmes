using UnityEngine;
public class Trap : MonoBehaviour// ���� ��� ������������� ������ � ��
{
    private void OnTriggerEnter(Collider other)// �����, ���� ����������� ��� ���� ���������� � ������
    {
        if (other.CompareTag("Player"))// ����������, �� ��'���, �� �������� � �������, �� ��� "Player"
        {
            PlayerManager player = other.gameObject.GetComponent<PlayerManager>(); // �������� ��������� PlayerManager � ��'���� ������

            if (player.protectActivated)// ����������, �� ����������� ������ � ������
            {
                Debug.Log("Protect");// �������� �����������, �� ������ �����������, � �������� � ������
                return;
            }
            else
            {
                player.Explode();// ���� ������ �� �����������, ��������� ����� ������ ������

                Debug.Log("You died");// �������� �����������, �� ������� �������

                player.Lost();// ��������� �����, ���� �������� ������� ������
            }
        }
    }
}
