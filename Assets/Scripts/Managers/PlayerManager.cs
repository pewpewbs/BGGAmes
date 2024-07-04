using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    
    [SerializeField]private float moveSpeed = 5.0f; // Швидкість руху гравця
    [SerializeField] private MainManager mainManager; // Для івенту

    private int currentNodeIndex = 0; // Індекс поточного вузла на шляху
    private bool isMoving = false; // Чи рухається гравець в даний момент
    public bool protectActivated = false; // Чи активований захист гравця

    private Coroutine moveCoroutine;//Посилання на корутину для руху

    public GameObject cubePrefab; // Префаб для дрібних кубів

    private Renderer playerRenderer;//Компонент який дає змогу змінювати колір
    private Rigidbody playerRigidbody;//Компонент що дає змогу керувати фіз. властивостями
    private Collider playerCollider;//Компонент який задає об'єкту фізичний об'єм

    public Color notProtect = new(255 / 255f, 255 / 255f, 14 / 255f, 255 / 255f);//збереження звичайного кольору гравця
    public Color protect = new(173 / 255f, 255 / 255f, 47 / 255f, 255 / 255f);//збереження кольору в стані захисту гравця

    private void Awake()
    {
        //Беремо посилання на всі потрібні компоненти
        playerRenderer = GetComponent<Renderer>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
    }
    private void Start()
    {
        if (mainManager == null)//Якщо менеджера не існує
        {
            Debug.LogError("mainManager == null");//Видаємо помилку
        }
    }
    public void InitializePath(List<Node> path)// Ініціалізація шляху для руху гравця
    {
        currentNodeIndex = 0; // Скидання індексу вузла
        isMoving = false; // Скидання стану руху
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);//Якщо корутина вже є то зупиняємо її
        moveCoroutine = StartCoroutine(MoveAlongPath(path)); // Запускаємо корутину для руху по шляху
    }
    public void SetPosition(Vector2Int position)// Встановлення позиції гравця за координатами сітки
    {
        gameObject.SetActive(true);//Робимо його активним для сцени
        Vector3 spawnPos = new(position.x, 1, position.y); // Встановлюємо позицію гравця
        gameObject.transform.position = spawnPos;//Спавнимо в певній позиції
        playerRigidbody.isKinematic = false; // Активуємо фізику
        playerCollider.enabled = true; // Вмикаємо колайдер
    }
    private IEnumerator MoveAlongPath(List<Node> path)// Корутина для руху гравця по шляху
    {
        yield return new WaitForSeconds(2); // Затримка перед початком руху

        isMoving = true; // Встановлюємо стан руху
        gameObject.tag = "Player"; // Встановлюємо тег гравця

        while (isMoving && currentNodeIndex < path.Count)// Рухаємося по шляху, поки є вузли
        {
            Node targetNode = path[currentNodeIndex];//Беремо ноду з шляху за індексом циклу
            Vector3 targetPosition = new(targetNode.Position.x, 0.75f, targetNode.Position.y);//Вибираємо в якості цілі позицію ноди але додаємо 0.75 щоб гравець був вище блоку

            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)// Рух до наступного вузла
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);//Йдемо до позиції
                yield return null;
            }
            currentNodeIndex++;// Перехід до наступного вузла
        }
        isMoving = false; // Завершення руху
    }

    
    public void Explode(int numberOfCubes = 20)// Метод для "вибуху" гравця
    {
        playerRigidbody.isKinematic = true;// Зупиняємо рух гравця
        playerCollider.enabled = false;//Вимикаємо його колайдер

        for (int i = 0; i < numberOfCubes; i++)// Програємо анімацію розриву на дрібні куби
        {
            CreateSmallCube();//Створюємо куби
        }
        gameObject.SetActive(false);// Робимо гравця неактивним після вибуху
    }
    private void CreateSmallCube()// Створення дрібного куба
    {
        GameObject smallCube = Instantiate(cubePrefab, transform.position, Random.rotation);// Створюємо дрібний куб

        Rigidbody rb = smallCube.AddComponent<Rigidbody>();//Беремо його компонент що відповідає за фізичні властивості
        rb.velocity = Random.insideUnitSphere * 5f;// Встановлюємо випадкову швидкість для дрібного куба
    }
    public void SetColor(Color color)// Функція для встановлення кольору
    {
        playerRenderer.material.color = color;// Встановлення кольору гравця
    }
    public void ResetPlayer()// Метод для скидання стану гравця після поразки
    {
        gameObject.SetActive(true);
        playerRigidbody.isKinematic = false;
        playerCollider.enabled = true;
        protectActivated = false;
        SetColor(notProtect);
    }
    public void Lost()// Метод для перезапуску гри після поразки
    {
        mainManager.onGameRestart?.Invoke();// Викликаємо івент перезапуску гри
        ResetPlayer();//Скидаємо налаштування гравця
    }
    public void ActivateShield()//Активація захисту
    {
        protectActivated = true;//Захист вмикаємо
        SetColor(protect);//Відповідна зміна кольору
    }
    public void DeactivateShield()//Деактивація захисту
    {
        protectActivated = false;//Захист вимикаємо
        SetColor(notProtect);//Відповідна зміна кольору
    }
}
