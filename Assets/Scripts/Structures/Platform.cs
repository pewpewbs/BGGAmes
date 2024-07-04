using UnityEngine;

public class Platform : MonoBehaviour// ���� ��� ������������� ���������, ��� ���������� � ����� (�����)
{
    public Node[,] grid; // ���������, ��� ���������� � ����� (�����)
    public Vector2Int GridSize { get; private set; } // ����� ����, ���������� � ��������� ��������
    public GameObject blockPrefab; // ������ �����, ���� ���� ����������������� ��� ��������� �����

    private readonly Collider[] collidersBuffer = new Collider[100]; // ����� ��� ��������� ���������� OverlapSphereNonAlloc (�������� 100 ����������)

    public void SetGridSize(Vector2Int newSize)// ������������ ����� ����� ����
    {
        GridSize = newSize;
    }

    public GameObject GetBlock(int x, int y) // ����� ��� ��������� ����� �� ������������ � ����
    {
        if (x < 0 || x >= GridSize.x || y < 0 || y >= GridSize.y) // ��������, �� ���������� ����������� � ����� ����
        {
            Debug.LogWarning("Coordinates out of bounds"); // �������� ������������, ���� ���������� ���� ������ ����
            return null;
        }
        Vector3 targetCoordinates = new(x, 0, y); // ������������ ������ ���������� � ���
        float searchRadius = 0.1f; // ����� ������

        int numColliders = Physics.OverlapSphereNonAlloc(targetCoordinates, searchRadius, collidersBuffer); // ��������� �� ���������� � �������� �����
        for (int i = 0; i < numColliders; i++) // ���������� ������� ����������
        {
            GameObject obj = collidersBuffer[i].gameObject; // �������� ��'��� � ����������
            if (obj.transform.position == targetCoordinates) // ����������, �� ������� ��'���� ������� � ��������� ������������
            {
                return obj; // ��������� ��'���, ���� ������� ����������
            }
        }
        Debug.Log("Object not found"); // �������� �����������, ���� ��'��� �� ���������
        return null; // ��������� null, ���� ��'��� �� ���������
    }
    public Node GetNode(int x, int y) // ����� ��� ��������� ����� �� ������������ � ����
    {
        if (x < 0 || x >= GridSize.x || y < 0 || y >= GridSize.y) // ��������, �� ���������� ����������� � ����� ����
        {
            Debug.LogWarning("Coordinates out of bounds"); // �������� ������������, ���� ���������� ���� ������ ����
            return null;
        }
        Node node = grid[x, y]; // �������� ����� � ���� �� �������� ������������
        if (node == null) // ����������, �� ����� ����
        {
            Debug.Log("Node not found at position: " + new Vector2Int(x, y)); // �������� �����������, ���� ����� �� ���������
            return null; // ��������� null, ���� ����� �� ���������
        }
        else
        {
            return node; // ��������� �����, ���� �� ���������
        }
    }
}
