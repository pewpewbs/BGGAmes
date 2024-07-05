using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainManager : MonoBehaviour // Менеджер що управляє грою
{
    [SerializeField] private Shield shield;//Посилання на щит

    public UnityEvent onGameRestart; // Івент для перезапуску гри
    public UnityEvent onNextLevel;// Івент для переходу на новий рівень

    [SerializeField] private PlayerManager player;// Гравець
    [SerializeField] private Target target;// Ціль

    private PathCreating pathCreating;//Створення шляху хочаб одного
    private Pathfinding pathFinding;//Пошук найменшого шляху

    private Vector2Int startPosition = new(2, 2); // Встановлення стартової позиції
    private Vector2Int endPosition = new(27, 25);  // Встановлення кінцевої позиції

    private int sizePlatform = 30;//Розмір платформи

    private GridManager gridManager;//Посилання на менеджера сітки
    
    private List<Node> pathForPlayer;//Найменший шлях
    private List<Node> path;//хочаб 1 шлях

    private void Start()
    {
        //Беремо необхідні посилання
        pathCreating = GetComponent<PathCreating>();
        pathFinding = GetComponent<Pathfinding>();
        gridManager = GetComponent<GridManager>();

        //Підписуємось на івенти
        onGameRestart.AddListener(RestartGame);
        onNextLevel.AddListener(NextLevel);

        if (gridManager == null)//Якщо менеджер сітки відсутній
        {
            Debug.LogError("Grid manager is null");//Видаємо помилку
            return;
        }
        else
        {
            GenerateLevel();//Генеруємо рівень

            StartGame();//Запускаємо гру
        }
    }
    public void GenerateLevel()//Генерація рівня
    {
        gridManager.CreatePlatform(sizePlatform);//генерація платформи size х size

        gridManager.SetStart(startPosition);//створення старту
        gridManager.SetEnd(endPosition);//створення фінішу

        path = pathCreating.FindPath(startPosition, endPosition);//Пошук шляху від старту до фінішу(Щоб був хочаб 1)
        if (path == null || path.Count == 0)
        {
            Debug.LogError("Path Creating not found");
            return;
        }

        gridManager.SetTraps(path, 5, 8);//Створення перешкод на шляху кожен 5-8 блок

        gridManager.ProcessPlatform(60, 20); //Вся інша карта рандомно генерується стінами та зонами смерті

        target.SetPosition(endPosition);//Ціль до якої йдемо спавниться на координатах фінішу
    }
    public void StartGame()//Запуск гри
    {
        player.SetPosition(startPosition);//Задаємо позицію гравцю

        pathForPlayer = pathFinding.FindPath(startPosition, endPosition);//Шукаємо найменший шлях для гравця
        if (pathForPlayer == null || pathForPlayer.Count == 0)//Якщо його нема
        {
            Debug.LogError("Path for player not found");//Видаэмо помилку
            return;
        }
        

        gridManager.SetColorPath(pathForPlayer, Color.blue); // Візуально відображаємо шлях гравця

        player.InitializePath(pathForPlayer);//Відправляємо гравця йти за знайденим шляхом
    }
    private void NextLevel()//Наступний рівень
    {
        GenerateLevel();//Генеруємо платформу
        StartGame();//Запускаємо гру
    }
    private void RestartGame()
    {
        player.ResetPlayer();//Скидання налаштувань гравця до початкових налаштувань
        shield.ResetShield();//Скидання налаштувань кнопки до початкових налаштувань
        StartGame();//Запускаємо гру
    }
}
