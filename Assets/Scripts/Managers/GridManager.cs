using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour//Код для управління сіткою
{
    #region variables

    [SerializeField] private GameObject trapPrefab; // Префаб для пасток
    [SerializeField] private GameObject wallPrefab; // Префаб для стін

    private Platform platform; // Наша платформа
    private Transform platformTransform; //Компонент трансформ нашої платформи
    #endregion
    private void Awake()
    {
        platform = GetComponent<Platform>(); // Отримуємо компонент платформи
        platformTransform = platform.GetComponent<Transform>();// Беремо компонент трансформ у платформи для знищення її дочірніх елементів
    }
    private void InitializeGrid(int size)// Ініціалізація сітки заданого розміру
    {
        platform.SetGridSize(new Vector2Int(size, size)); // Встановлюємо розмір сітки

        platform.grid = new Node[platform.GridSize.x, platform.GridSize.y]; // Ініціалізація масиву

        for (int x = 0; x < platform.GridSize.x; x++)
        {
            for (int y = 0; y < platform.GridSize.y; y++)
            {
                Vector3 blockPosition = new(x, 0, y);
                GameObject box = Instantiate(platform.blockPrefab, blockPosition, Quaternion.identity, platform.GetComponent<Transform>()); // Спавн блоку з префабу на відповідну позицію

                if (!box.TryGetComponent(out Node node))
                {
                    // Якщо компонент Node не знайдений, виводимо помилку
                    Debug.LogError($"Node not found at position [{x}, {y}]");
                }
                else
                {
                    node.Initialize(new Vector2Int(x, y)); // Виклик методу ініціалізації для встановлення позиції
                    platform.grid[x, y] = node; //додавання ноди до сітки

                    if (x == 0 || x == platform.GridSize.x - 1 || y == 0 || y == platform.GridSize.y - 1)// Додаємо стіни по периметру платформи
                    {
                        SpawnWall(x,y);//Cтворюємо стіну
                    }
                }
            }
        }
    }
    public void DestroyGrid()// Знищення сітки
    {
        if (platform == null)
        {
            // Якщо платформа не знайдена, виводимо помилку
            Debug.LogError("platformParent is null. Cannot destroy child objects.");
            return;
        }
        for (int i = platform.GetComponent<Transform>().childCount - 1; i >= 0; i--)// Перебираємо всіх дочірніх об'єктів і видаляємо їх
        {
            Destroy(platformTransform.GetChild(i).gameObject);
        }
        platform.grid = null;// Очищення масиву grid
        platform.SetGridSize(new Vector2Int(0,0));//Розміри платформу 0х0
    }
    public void ProcessPlatform(int wallSpawnPercent, int trapSpawnPercent)// Обробка платформи: додавання пасток і стін
    {
        for (int x = 0; x < platform.GridSize.x; x++)
        {
            for (int y = 0; y < platform.GridSize.y; y++)
            {
                Node node = platform.GetNode(x,y);// Дістаємо ноду

                if (!node.ChangebleData) // Якщо блок не можна змінювати
                {
                    continue;//пропускаємо його
                }

                int randomValue = Random.Range(1, 101);//Беремо рандомне число

                if (randomValue < wallSpawnPercent)//Якщо відсоток спавну відповідний до створеного числа
                {
                    SpawnWall(x,y);// Додавання стіни на вказаній ноді
                }
                else if (randomValue < trapSpawnPercent + wallSpawnPercent) //Якщо відсоток спавну відповідний до створеного числа
                {
                    SpawnTrap(x, y);// Додавання пастки на вказаній ноді
                }
            }
        }
    }
    public void CreatePlatform(int size)// Створення платформи
    {
        if (platform != null)//Якщо платформа вже існує
        {
            DestroyGrid(); // Знищення існуючої платформи
            InitializeGrid(size); // Ініціалізація нової сітки
        }
    }
    private void SpawnWall(int x,int y) //Спавн стіни за координатами сітки
    {
        Vector3 spawnPos = new(x, 1, y);//Конвертуємо позицію по сітці в позицію у світі
        Instantiate(wallPrefab, spawnPos, Quaternion.identity, platform.transform);//Спавнимо стіну
        Node node = platform.GetNode(x, y);//Беремо ноду за координатами
        node.SetWalkable(false);// Блок не прохідний
        node.SetChangebleData(false);// Блок не змінний
    }
    private void SpawnTrap(int x, int y)//Спавн пастки за координатами сітки
    {
        Vector3 spawnPos = new(x, 0.55f, y);//Конвертуємо позицію по сітці в позицію у світі
        Instantiate(trapPrefab, spawnPos, Quaternion.identity, platform.transform);//Спавнимо пастку
        Node node = platform.GetNode(x, y);//Беремо ноду за координатами
        node.SetWalkable(true);// Блок прохідний
        node.SetChangebleData(false);// Блок не змінний
    }
    public void SetColorPath(List<Node> path, Color color)// Зміна кольору шляху
    {
        foreach (Node node in path)
        {
            SetNodeColor(node, color);//Кожній ноді в масиві задаємо колір
        }
    }
    public void SetTraps(List<Node> path, int minDistance,int maxDistance)// Додавання пасток на певному шляху
    {
        int length = path.Count; // Кількість в масиві
        int index = Random.Range(minDistance, maxDistance);// Число що відповідає за періодичність пасток на шляху
        for (int i = 0; i < length; i++) // Пройдемось по всім блокам шляху
        {
            Node node = platform.GetNode(path[i].Position.x, path[i].Position.y);//Беремо посилання на ноду за координатами
            if (node == null || !node.ChangebleData)
            {
                continue;//Якщо ноди не існує або вона незмінна то пропускаємо її
            }
            if(i == index)//Якщо цикл дійшов до індексу
            {
                SpawnTrap(node.Position.x,node.Position.y);//Створюємо пастку
                index += Random.Range(minDistance, maxDistance);//Новий індекс
            }
            node.SetWalkable(true);//В будьякому випадку блок прохідний
            node.SetChangebleData(false);//В будьякому випадку блок вже не змінний(Щоб забезпечити як мінімум 1 прохідний шлях на карті)
        }
    }
    public void SetStart(Vector2Int position)// Встановлення початкової позиції
    {
        Node node = platform.GetNode(position.x, position.y);//Беремо ноду за координатами
        if (node == null)//Якщо ноди не існує
        {
            Debug.LogError("Start node is null");
            return;
        }
        SetNodeColor(node, Color.black);//Фарбуємо старт в чорний
        node.SetWalkable(true);//Робимо його прохідним
        node.SetChangebleData(false);//Робимо його незмінним
    }
    public void SetEnd(Vector2Int position)// Встановлення кінцевої позиції
    {
        Node node = platform.GetNode(position.x, position.y);//Беремо ноду за координатами
        if (node == null)//Якщо ноди не існує
        {
            Debug.LogError("End node is null");
            return;
        }
        SetNodeColor(node, Color.black);//Фарбуємо фініш в чорний
        node.SetWalkable(true);//Робимо його прохідним
        node.SetChangebleData(false);//Робимо його незмінним
    }
    private void SetNodeColor(Node node, Color color)// Встановлення кольору ноди
    {
        if (!node.TryGetComponent(out Renderer renderer))//Беремо компонент з допомогою якого і зможемо втановити колір
        {
            Debug.LogError("Renderer not found on the object.");
        }
        else
        {
            renderer.material.color = color;//Змінюємо колір на заданий
        }
    }
}
