using System.Collections.Generic;
using UnityEngine;

public class PathCreating : MonoBehaviour// Клас для створення шляху на платформі
{
    Platform platform;// Змінна для зберігання посилання на платформу

    void Awake()
    {
        platform = GetComponent<Platform>(); // Отримуємо компонент платформи
    }
    public List<Node> FindPath(Vector2Int startPos, Vector2Int targetPos)// Метод для знаходження шляху між двома точками
    {
        Node startNode = platform.GetNode(startPos.x, startPos.y);// Отримуємо початковий вузол
        Node targetNode = platform.GetNode(targetPos.x, targetPos.y);// Отримуємо кінцевий вузол

        if (startNode == null || targetNode == null)// Перевірка наявності вузлів
        {
            Debug.LogError("Start or target node is null");
            return null;
        }
        if (!startNode.IsWalkable || !targetNode.IsWalkable)// Перевірка прохідності вузлів
        {
            Debug.LogError("Start or target node is not walkable");
            return null;
        }
        
        List<Node> openSet = new();// Список не оброблених вузлів

        HashSet<Node> closedSet = new();// Список оброблених вузлів
        openSet.Add(startNode);//Додаємо старт до необроблених вузлів

        while (openSet.Count > 0)// Основний цикл пошуку шляху
        {
            Node currentNode = openSet[Random.Range(0, openSet.Count)];// Вибираємо випадковий вузол

            openSet.Remove(currentNode); // Видаляємо поточний вузол зі списку не оброблених вузлів
            closedSet.Add(currentNode);  // Додаємо поточний вузол до списку оброблених вузлів

            if (currentNode == targetNode)// Перевірка, чи досягнуто кінцевий вузол
            {
                Debug.Log("Path found");
                return RetracePath(startNode, targetNode); // Відтворюємо шлях
            }

            foreach (Node neighbour in GetNeighbors(currentNode))// Перебираємо сусідів поточного вузла
            {
                if (!neighbour.IsWalkable || closedSet.Contains(neighbour))// Пропускаємо непрохідні або вже оброблені вузли
                {
                    continue;
                }

                // Обчислюємо нову вартість руху до сусіднього вузла
                float newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newMovementCostToNeighbour; // Оновлюємо вартість руху до вузла
                    neighbour.HCost = GetDistance(neighbour, targetNode); // Оновлюємо евристичну вартість
                    neighbour.Parent = currentNode; // Встановлюємо батьківський вузол

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour); // Додаємо сусідній вузол до списку не оброблених вузлів
                }
            }
        }
        Debug.LogWarning("Path not found");//Видаємо помилку якщо шляху немає
        return null; // Повертаємо null, якщо шлях не знайдений
    }

    private List<Node> RetracePath(Node startNode, Node endNode)// Метод для відтворення шляху від кінцевого до початкового вузла
    {
        List<Node> path = new();
        Node currentNode = endNode;

        // Відтворюємо шлях, рухаючись від кінцевого вузла до початкового через батьківські вузли
        while (currentNode != startNode)
        {
            path.Add(currentNode);//Додаємо до шляху ноду
            currentNode = currentNode.Parent;
        }
        path.Reverse(); // Реверсуємо шлях, щоб отримати його від початкового до кінцевого вузла
        return path;//Повертаємо шлях
    }
    private float GetDistance(Node nodeA, Node nodeB)// Метод для обчислення відстані між двома вузлами
    {
        // Обчислюємо різницю координат по осі X
        int dstX = Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
        // Обчислюємо різницю координат по осі Y
        int dstY = Mathf.Abs(nodeA.Position.y - nodeB.Position.y);

        // Повертаємо манхеттенську відстань, помножену на 10
        // 10 використовується як умовна вартість одного кроку
        return 10 * (dstX + dstY);
    }
    private List<Node> GetNeighbors(Node node)// Метод для отримання сусідів поточного вузла
    {
        List<Node> neighbors = new();

        Vector2Int[] directions = new Vector2Int[]//Можливі напрямки
        {
        new(0, 1),  // верхній сусід
        new(1, 0),  // правий сусід
        new(0, -1), // нижній сусід
        new(-1, 0)  // лівий сусід
        };

        foreach (var direction in directions)// Перевіряємо кожного сусіда
        {
            int checkX = node.Position.x + direction.x;
            int checkY = node.Position.y + direction.y;

            // Перевірка, чи знаходиться сусід в межах сітки
            if (checkX >= 0 && checkX < platform.GridSize.x && checkY >= 0 && checkY < platform.GridSize.y)
            {
                Node neighbor = platform.GetNode(checkX, checkY);

                if (neighbor != null && neighbor.IsWalkable)//якщо нода існує і прохідна
                {
                    neighbors.Add(neighbor);// Додаємо ноду
                }
            }
        }
        return neighbors;//Повертаємо масив сусідніх нод
    }
}
