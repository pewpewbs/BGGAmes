using UnityEngine;

// ���� ��� ������������� ����� � ����
public class Node : MonoBehaviour
{
    #region variables
    public Vector2Int Position { get; private set; } // ������� � ����

    // ������� ���� ��� ��������� ������� ������������
    private bool isWalkable; // �� ��������� �����
    private bool changebleData; // �� ����� �������� ��� �����

    // ���������� � ���������� �������� ������������
    public bool IsWalkable
    {
        get => isWalkable; // ������� �������� ����������
        private set
        {
            isWalkable = value; // ���������� �������� ����������
        }
    }
    public bool ChangebleData
    {
        get => changebleData; // ������� �������� ������� �����
        private set
        {
            changebleData = value; // ���������� �������� ������� �����
        }
    }

    public Node Parent { get; set; } // ����� ��� ��������� ������������ ����� (��� ������ ��������� ������ �����)

    public float GCost { get; set; } // ������� �� ���������� ����� �� ���������
    public float HCost { get; set; } // ��������� ������� �� ��������� ����� �� ��������
    public float FCost => GCost + HCost; // �������� ������� (GCost + HCost)
    #endregion

    private void Awake()
    {
        // ������ �����������
        IsWalkable = true; // ������������ ����� �� ��������� �� �������������
        ChangebleData = true; // ������������ ����� �� �����, �� ����� �������� �� �������������
    }

    public void Initialize(Vector2Int position, bool isWalkable = true, bool changebleData = true)
    {
        // ���������� ����� � ������� �����������
        Position = position; // ������������ �������
        IsWalkable = isWalkable; // ������������ ����������
        ChangebleData = changebleData; // ������������ ������� �����
    }

    public void SetWalkable(bool isWalkable)
    {
        IsWalkable = isWalkable;// ������������ ���������� �����
    }

    public void SetChangebleData(bool changebleData)
    {
        ChangebleData = changebleData;// ������������ ������� ����� �����
    }
}
