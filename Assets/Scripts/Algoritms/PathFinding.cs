using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour// Клас для алгоритму пошуку найменшого шляху
{
    private Platform platform;// Змінна для зберігання посилання на платформу

    void Awake()
    {
        platform = GetComponent<Platform>(); // Отримуємо компонент платформи
    }
    public List<Node> FindPath(Vector2Int startPos, Vector2Int targetPos)// Метод для знаходження шляху між двома точками
    {
        // Отримуємо початковий і кінцевий вузли
        Node startNode = platform.GetNode(startPos.x, startPos.y);
        Node targetNode = platform.GetNode(targetPos.x, targetPos.y);

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
        openSet.Add(startNode);//До не оброблених додаємо старт

        if (platform.grid == null)// Перевірка наявності grid
        {
            Debug.Log("Platform grid is not initialized");
            return null;
        }
        while (openSet.Count > 0)// Основний цикл пошуку шляху (цей цикл виконується, поки є вузли для обробки у відкритому списку)
        {
            Node currentNode = openSet[0];// Знаходження вузла з найменшою F-вартістю у відкритому списку
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost))
                {
                    currentNode = openSet[i];
                }
            }

            // Переміщення поточного вузла з відкритого списку до закритого
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // Перевірка, чи досягнуто цільовий вузол
            if (currentNode == targetNode) // Якщо поточний вузол є цільовим вузлом, шлях відновлюється і повертається
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))// Перевірка сусідів поточного вузла
            {
                // Пропуск непрохідних вузлів та вузлів у закритому списку
                if (!neighbor.IsWalkable || closedSet.Contains(neighbor))
                {
                    continue; // Пропускаються непрохідні вузли та вузли, що вже знаходяться у закритому списку
                }

                // Обчислення нової G-вартісті для сусіднього вузла
                float newGCost = currentNode.GCost + GetDistance(currentNode, neighbor);
                if (newGCost < neighbor.GCost || !openSet.Contains(neighbor)) // Якщо нова G-вартість менша за поточну G-вартість сусіднього вузла або вузол ще не у відкритому списку
                {
                    // Оновлення вартостей сусіднього вузла та встановлення батьківського вузла
                    neighbor.GCost = newGCost;
                    neighbor.HCost = GetDistance(neighbor, targetNode);
                    neighbor.Parent = currentNode;

                    // Додавання сусіднього вузла до відкритого списку, якщо він ще там не знаходиться
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        Debug.LogWarning("Path not found");//Видаємо помилку якщо шлях не знайдено
        return null; // Повертаємо null, якщо шлях не знайдений
    }
    List<Node> RetracePath(Node startNode, Node endNode)// Метод для відтворення шляху від кінцевого до початкового вузла
    {
        List<Node> path = new(); // Створюємо порожній список для зберігання шляхових вузлів
        Node currentNode = endNode; // Починаємо з кінцевого вузла

        while (currentNode != startNode)// Відстежуємо шлях від кінцевого вузла до початкового, використовуючи зв'язки батьківських вузлів
        {
            path.Add(currentNode); // Додаємо поточний вузол до шляху
            currentNode = currentNode.Parent; // Переходимо до батьківського вузла
        }
        path.Reverse(); // Реверсуємо список, щоб отримати шлях від початкового вузла до кінцевого
        return path; // Повертаємо список вузлів, що представляє знайдений шлях
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

    // Метод для отримання сусідів поточного вузла
    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new();

        Vector2Int[] directions = new Vector2Int[]//Можливі напрямки 
        {
        new(0, 1),  // верхній сусід
        new(1, 0),  // правий сусід
        new(0, -1), // нижній сусід
        new(-1, 0)  // лівий сусід
        };

        // Перевіряємо кожного сусіда
        foreach (var direction in directions)
        {
            int checkX = node.Position.x + direction.x;
            int checkY = node.Position.y + direction.y;

            // Перевірка, чи знаходиться сусід в межах сітки
            if (checkX >= 0 && checkX < platform.GridSize.x && checkY >= 0 && checkY < platform.GridSize.y)
            {
                Node neighbor = platform.grid[checkX, checkY];

                if (neighbor != null)// якщо нода існує і прохідна
                {
                    neighbors.Add(neighbor);// Додаємо сусіда
                }
            }
        }
        return neighbors;//Повертаємо масив з всіх сусідів цього об'єкту
    }
}
