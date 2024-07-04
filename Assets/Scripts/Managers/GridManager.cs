using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour//��� ��� ��������� �����
{
    #region variables

    [SerializeField] private GameObject trapPrefab; // ������ ��� ������
    [SerializeField] private GameObject wallPrefab; // ������ ��� ���

    private Platform platform; // ���� ���������
    private Transform platformTransform; //��������� ��������� ���� ���������
    #endregion
    private void Awake()
    {
        platform = GetComponent<Platform>(); // �������� ��������� ���������
        platformTransform = platform.GetComponent<Transform>();// ������ ��������� ��������� � ��������� ��� �������� �� ������� ��������
    }
    private void InitializeGrid(int size)// ����������� ���� �������� ������
    {
        platform.SetGridSize(new Vector2Int(size, size)); // ������������ ����� ����

        platform.grid = new Node[platform.GridSize.x, platform.GridSize.y]; // ����������� ������

        for (int x = 0; x < platform.GridSize.x; x++)
        {
            for (int y = 0; y < platform.GridSize.y; y++)
            {
                Vector3 blockPosition = new(x, 0, y);
                GameObject box = Instantiate(platform.blockPrefab, blockPosition, Quaternion.identity, platform.GetComponent<Transform>()); // ����� ����� � ������� �� �������� �������

                if (!box.TryGetComponent(out Node node))
                {
                    // ���� ��������� Node �� ���������, �������� �������
                    Debug.LogError($"Node not found at position [{x}, {y}]");
                }
                else
                {
                    node.Initialize(new Vector2Int(x, y)); // ������ ������ ����������� ��� ������������ �������
                    platform.grid[x, y] = node; //��������� ���� �� ����

                    if (x == 0 || x == platform.GridSize.x - 1 || y == 0 || y == platform.GridSize.y - 1)// ������ ���� �� ��������� ���������
                    {
                        SpawnWall(x,y);//C�������� ����
                    }
                }
            }
        }
    }
    public void DestroyGrid()// �������� ����
    {
        if (platform == null)
        {
            // ���� ��������� �� ��������, �������� �������
            Debug.LogError("platformParent is null. Cannot destroy child objects.");
            return;
        }
        for (int i = platform.GetComponent<Transform>().childCount - 1; i >= 0; i--)// ���������� ��� ������� ��'���� � ��������� ��
        {
            Destroy(platformTransform.GetChild(i).gameObject);
        }
        platform.grid = null;// �������� ������ grid
        platform.SetGridSize(new Vector2Int(0,0));//������ ��������� 0�0
    }
    public void ProcessPlatform(int wallSpawnPercent, int trapSpawnPercent)// ������� ���������: ��������� ������ � ���
    {
        for (int x = 0; x < platform.GridSize.x; x++)
        {
            for (int y = 0; y < platform.GridSize.y; y++)
            {
                Node node = platform.GetNode(x,y);// ĳ����� ����

                if (!node.ChangebleData) // ���� ���� �� ����� ��������
                {
                    continue;//���������� ����
                }

                int randomValue = Random.Range(1, 101);//������ �������� �����

                if (randomValue < wallSpawnPercent)//���� ������� ������ ��������� �� ���������� �����
                {
                    SpawnWall(x,y);// ��������� ���� �� ������� ���
                }
                else if (randomValue < trapSpawnPercent + wallSpawnPercent) //���� ������� ������ ��������� �� ���������� �����
                {
                    SpawnTrap(x, y);// ��������� ������ �� ������� ���
                }
            }
        }
    }
    public void CreatePlatform(int size)// ��������� ���������
    {
        if (platform != null)//���� ��������� ��� ����
        {
            DestroyGrid(); // �������� ������� ���������
            InitializeGrid(size); // ����������� ���� ����
        }
    }
    private void SpawnWall(int x,int y) //����� ���� �� ������������ ����
    {
        Vector3 spawnPos = new(x, 1, y);//���������� ������� �� ���� � ������� � ���
        Instantiate(wallPrefab, spawnPos, Quaternion.identity, platform.transform);//�������� ����
        Node node = platform.GetNode(x, y);//������ ���� �� ������������
        node.SetWalkable(false);// ���� �� ���������
        node.SetChangebleData(false);// ���� �� ������
    }
    private void SpawnTrap(int x, int y)//����� ������ �� ������������ ����
    {
        Vector3 spawnPos = new(x, 0.55f, y);//���������� ������� �� ���� � ������� � ���
        Instantiate(trapPrefab, spawnPos, Quaternion.identity, platform.transform);//�������� ������
        Node node = platform.GetNode(x, y);//������ ���� �� ������������
        node.SetWalkable(true);// ���� ���������
        node.SetChangebleData(false);// ���� �� ������
    }
    public void SetColorPath(List<Node> path, Color color)// ���� ������� �����
    {
        foreach (Node node in path)
        {
            SetNodeColor(node, color);//����� ��� � ����� ������ ����
        }
    }
    public void SetTraps(List<Node> path, int minDistance,int maxDistance)// ��������� ������ �� ������� �����
    {
        int length = path.Count; // ʳ������ � �����
        int index = Random.Range(minDistance, maxDistance);// ����� �� ������� �� ����������� ������ �� �����
        for (int i = 0; i < length; i++) // ���������� �� ��� ������ �����
        {
            Node node = platform.GetNode(path[i].Position.x, path[i].Position.y);//������ ��������� �� ���� �� ������������
            if (node == null || !node.ChangebleData)
            {
                continue;//���� ���� �� ���� ��� ���� ������� �� ���������� ��
            }
            if(i == index)//���� ���� ����� �� �������
            {
                SpawnTrap(node.Position.x,node.Position.y);//��������� ������
                index += Random.Range(minDistance, maxDistance);//����� ������
            }
            node.SetWalkable(true);//� ��������� ������� ���� ���������
            node.SetChangebleData(false);//� ��������� ������� ���� ��� �� ������(��� ����������� �� ����� 1 ��������� ���� �� ����)
        }
    }
    public void SetStart(Vector2Int position)// ������������ ��������� �������
    {
        Node node = platform.GetNode(position.x, position.y);//������ ���� �� ������������
        if (node == null)//���� ���� �� ����
        {
            Debug.LogError("Start node is null");
            return;
        }
        SetNodeColor(node, Color.black);//������� ����� � ������
        node.SetWalkable(true);//������ ���� ���������
        node.SetChangebleData(false);//������ ���� ��������
    }
    public void SetEnd(Vector2Int position)// ������������ ������ �������
    {
        Node node = platform.GetNode(position.x, position.y);//������ ���� �� ������������
        if (node == null)//���� ���� �� ����
        {
            Debug.LogError("End node is null");
            return;
        }
        SetNodeColor(node, Color.black);//������� ���� � ������
        node.SetWalkable(true);//������ ���� ���������
        node.SetChangebleData(false);//������ ���� ��������
    }
    private void SetNodeColor(Node node, Color color)// ������������ ������� ����
    {
        if (!node.TryGetComponent(out Renderer renderer))//������ ��������� � ��������� ����� � ������� ��������� ����
        {
            Debug.LogError("Renderer not found on the object.");
        }
        else
        {
            renderer.material.color = color;//������� ���� �� �������
        }
    }
}
