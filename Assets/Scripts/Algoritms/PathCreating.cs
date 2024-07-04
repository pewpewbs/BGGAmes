using System.Collections.Generic;
using UnityEngine;

public class PathCreating : MonoBehaviour// ���� ��� ��������� ����� �� ��������
{
    Platform platform;// ����� ��� ��������� ��������� �� ���������

    void Awake()
    {
        platform = GetComponent<Platform>(); // �������� ��������� ���������
    }
    public List<Node> FindPath(Vector2Int startPos, Vector2Int targetPos)// ����� ��� ����������� ����� �� ����� �������
    {
        Node startNode = platform.GetNode(startPos.x, startPos.y);// �������� ���������� �����
        Node targetNode = platform.GetNode(targetPos.x, targetPos.y);// �������� ������� �����

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
        openSet.Add(startNode);//������ ����� �� ������������ �����

        while (openSet.Count > 0)// �������� ���� ������ �����
        {
            Node currentNode = openSet[Random.Range(0, openSet.Count)];// �������� ���������� �����

            openSet.Remove(currentNode); // ��������� �������� ����� � ������ �� ���������� �����
            closedSet.Add(currentNode);  // ������ �������� ����� �� ������ ���������� �����

            if (currentNode == targetNode)// ��������, �� ��������� ������� �����
            {
                Debug.Log("Path found");
                return RetracePath(startNode, targetNode); // ³��������� ����
            }

            foreach (Node neighbour in GetNeighbors(currentNode))// ���������� ����� ��������� �����
            {
                if (!neighbour.IsWalkable || closedSet.Contains(neighbour))// ���������� ��������� ��� ��� �������� �����
                {
                    continue;
                }

                // ���������� ���� ������� ���� �� ��������� �����
                float newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newMovementCostToNeighbour; // ��������� ������� ���� �� �����
                    neighbour.HCost = GetDistance(neighbour, targetNode); // ��������� ���������� �������
                    neighbour.Parent = currentNode; // ������������ ����������� �����

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour); // ������ ������ ����� �� ������ �� ���������� �����
                }
            }
        }
        Debug.LogWarning("Path not found");//������ ������� ���� ����� ����
        return null; // ��������� null, ���� ���� �� ���������
    }

    private List<Node> RetracePath(Node startNode, Node endNode)// ����� ��� ���������� ����� �� �������� �� ����������� �����
    {
        List<Node> path = new();
        Node currentNode = endNode;

        // ³��������� ����, ��������� �� �������� ����� �� ����������� ����� ��������� �����
        while (currentNode != startNode)
        {
            path.Add(currentNode);//������ �� ����� ����
            currentNode = currentNode.Parent;
        }
        path.Reverse(); // ��������� ����, ��� �������� ���� �� ����������� �� �������� �����
        return path;//��������� ����
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
    private List<Node> GetNeighbors(Node node)// ����� ��� ��������� ����� ��������� �����
    {
        List<Node> neighbors = new();

        Vector2Int[] directions = new Vector2Int[]//������ ��������
        {
        new(0, 1),  // ������ ����
        new(1, 0),  // ������ ����
        new(0, -1), // ����� ����
        new(-1, 0)  // ���� ����
        };

        foreach (var direction in directions)// ���������� ������� �����
        {
            int checkX = node.Position.x + direction.x;
            int checkY = node.Position.y + direction.y;

            // ��������, �� ����������� ���� � ����� ����
            if (checkX >= 0 && checkX < platform.GridSize.x && checkY >= 0 && checkY < platform.GridSize.y)
            {
                Node neighbor = platform.GetNode(checkX, checkY);

                if (neighbor != null && neighbor.IsWalkable)//���� ���� ���� � ��������
                {
                    neighbors.Add(neighbor);// ������ ����
                }
            }
        }
        return neighbors;//��������� ����� ������ ���
    }
}
