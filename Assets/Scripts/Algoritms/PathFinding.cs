using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour// ���� ��� ��������� ������ ���������� �����
{
    private Platform platform;// ����� ��� ��������� ��������� �� ���������

    void Awake()
    {
        platform = GetComponent<Platform>(); // �������� ��������� ���������
    }
    public List<Node> FindPath(Vector2Int startPos, Vector2Int targetPos)// ����� ��� ����������� ����� �� ����� �������
    {
        // �������� ���������� � ������� �����
        Node startNode = platform.GetNode(startPos.x, startPos.y);
        Node targetNode = platform.GetNode(targetPos.x, targetPos.y);

        if (startNode == null || targetNode == null)// �������� �������� �����
        {
            Debug.LogError("Start or target node is null");
            return null;
        }
        if (!startNode.IsWalkable || !targetNode.IsWalkable)// �������� ���������� �����
        {
            Debug.LogError("Start or target node is not walkable");
            return null;
        }

        List<Node> openSet = new();// ������ �� ���������� �����

        HashSet<Node> closedSet = new();// ������ ���������� �����
        openSet.Add(startNode);//�� �� ���������� ������ �����

        if (platform.grid == null)// �������� �������� grid
        {
            Debug.Log("Platform grid is not initialized");
            return null;
        }
        while (openSet.Count > 0)// �������� ���� ������ ����� (��� ���� ����������, ���� � ����� ��� ������� � ��������� ������)
        {
            Node currentNode = openSet[0];// ����������� ����� � ��������� F-������� � ��������� ������
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost))
                {
                    currentNode = openSet[i];
                }
            }

            // ���������� ��������� ����� � ��������� ������ �� ���������
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // ��������, �� ��������� �������� �����
            if (currentNode == targetNode) // ���� �������� ����� � �������� ������, ���� ������������ � �����������
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))// �������� ����� ��������� �����
            {
                // ������� ����������� ����� �� ����� � ��������� ������
                if (!neighbor.IsWalkable || closedSet.Contains(neighbor))
                {
                    continue; // ������������� ��������� ����� �� �����, �� ��� ����������� � ��������� ������
                }

                // ���������� ���� G-������ ��� ��������� �����
                float newGCost = currentNode.GCost + GetDistance(currentNode, neighbor);
                if (newGCost < neighbor.GCost || !openSet.Contains(neighbor)) // ���� ���� G-������� ����� �� ������� G-������� ��������� ����� ��� ����� �� �� � ��������� ������
                {
                    // ��������� ��������� ��������� ����� �� ������������ ������������ �����
                    neighbor.GCost = newGCost;
                    neighbor.HCost = GetDistance(neighbor, targetNode);
                    neighbor.Parent = currentNode;

                    // ��������� ��������� ����� �� ��������� ������, ���� �� �� ��� �� �����������
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        Debug.LogWarning("Path not found");//������ ������� ���� ���� �� ��������
        return null; // ��������� null, ���� ���� �� ���������
    }
    List<Node> RetracePath(Node startNode, Node endNode)// ����� ��� ���������� ����� �� �������� �� ����������� �����
    {
        List<Node> path = new(); // ��������� ������� ������ ��� ��������� �������� �����
        Node currentNode = endNode; // �������� � �������� �����

        while (currentNode != startNode)// ³�������� ���� �� �������� ����� �� �����������, �������������� ��'���� ����������� �����
        {
            path.Add(currentNode); // ������ �������� ����� �� �����
            currentNode = currentNode.Parent; // ���������� �� ������������ �����
        }
        path.Reverse(); // ��������� ������, ��� �������� ���� �� ����������� ����� �� ��������
        return path; // ��������� ������ �����, �� ����������� ��������� ����
    }

    private float GetDistance(Node nodeA, Node nodeB)// ����� ��� ���������� ������ �� ����� �������
    {
        // ���������� ������ ��������� �� �� X
        int dstX = Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
        // ���������� ������ ��������� �� �� Y
        int dstY = Mathf.Abs(nodeA.Position.y - nodeB.Position.y);

        // ��������� ������������� �������, ��������� �� 10
        // 10 ��������������� �� ������ ������� ������ �����
        return 10 * (dstX + dstY);
    }

    // ����� ��� ��������� ����� ��������� �����
    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new();

        Vector2Int[] directions = new Vector2Int[]//������ �������� 
        {
        new(0, 1),  // ������ ����
        new(1, 0),  // ������ ����
        new(0, -1), // ����� ����
        new(-1, 0)  // ���� ����
        };

        // ���������� ������� �����
        foreach (var direction in directions)
        {
            int checkX = node.Position.x + direction.x;
            int checkY = node.Position.y + direction.y;

            // ��������, �� ����������� ���� � ����� ����
            if (checkX >= 0 && checkX < platform.GridSize.x && checkY >= 0 && checkY < platform.GridSize.y)
            {
                Node neighbor = platform.grid[checkX, checkY];

                if (neighbor != null)// ���� ���� ���� � ��������
                {
                    neighbors.Add(neighbor);// ������ �����
                }
            }
        }
        return neighbors;//��������� ����� � ��� ����� ����� ��'����
    }
}
