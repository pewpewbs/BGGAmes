using UnityEngine;

public class Platform : MonoBehaviour// Клас для представлення платформи, яка складається з блоків (вузлів)
{
    public Node[,] grid; // Платформа, яка складається з блоків (вузлів)
    public Vector2Int GridSize { get; private set; } // Розмір сітки, властивість з приватним сеттером
    public GameObject blockPrefab; // Префаб блоку, який буде використовуватися для створення блоків

    private readonly Collider[] collidersBuffer = new Collider[100]; // Буфер для зберігання результатів OverlapSphereNonAlloc (максимум 100 результатів)

    public void SetGridSize(Vector2Int newSize)// Встановлюємо новий розмір сітки
    {
        GridSize = newSize;
    }

    public GameObject GetBlock(int x, int y) // Метод для отримання блоку за координатами в сітці
    {
        if (x < 0 || x >= GridSize.x || y < 0 || y >= GridSize.y) // Перевірка, чи координати знаходяться в межах сітки
        {
            Debug.LogWarning("Coordinates out of bounds"); // Виводимо попередження, якщо координати поза межами сітки
            return null;
        }
        Vector3 targetCoordinates = new(x, 0, y); // Встановлюємо цільові координати у світі
        float searchRadius = 0.1f; // Радіус пошуку

        int numColliders = Physics.OverlapSphereNonAlloc(targetCoordinates, searchRadius, collidersBuffer); // Знаходимо всі коллайдери в заданому радіусі
        for (int i = 0; i < numColliders; i++) // Перебираємо знайдені коллайдери
        {
            GameObject obj = collidersBuffer[i].gameObject; // Отримуємо об'єкт з коллайдера
            if (obj.transform.position == targetCoordinates) // Перевіряємо, чи позиція об'єкта співпадає з цільовими координатами
            {
                return obj; // Повертаємо об'єкт, якщо позиції співпадають
            }
        }
        Debug.Log("Object not found"); // Виводимо повідомлення, якщо об'єкт не знайдений
        return null; // Повертаємо null, якщо об'єкт не знайдений
    }
    public Node GetNode(int x, int y) // Метод для отримання вузла за координатами в сітці
    {
        if (x < 0 || x >= GridSize.x || y < 0 || y >= GridSize.y) // Перевірка, чи координати знаходяться в межах сітки
        {
            Debug.LogWarning("Coordinates out of bounds"); // Виводимо попередження, якщо координати поза межами сітки
            return null;
        }
        Node node = grid[x, y]; // Отримуємо вузол з сітки за заданими координатами
        if (node == null) // Перевіряємо, чи вузол існує
        {
            Debug.Log("Node not found at position: " + new Vector2Int(x, y)); // Виводимо повідомлення, якщо вузол не знайдений
            return null; // Повертаємо null, якщо вузол не знайдений
        }
        else
        {
            return node; // Повертаємо вузол, якщо він знайдений
        }
    }
}
