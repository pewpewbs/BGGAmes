using UnityEngine;
public class Patricle : MonoBehaviour//��� ��� ������ �� ����������� �� ������ ��� ������
{
    private void OnCollisionEnter(Collision collision)//��� ������� � ���� ��� ��������
    {
        Destroy(gameObject, 2f);//������� ������ ���� ������� ������
    }
}
