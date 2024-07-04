using UnityEngine;

// Клас для представлення вузла в сітці
public class Node : MonoBehaviour
{
    #region variables
    public Vector2Int Position { get; private set; } // Позиція в сітці

    // Приватні поля для зберігання значень властивостей
    private bool isWalkable; // Чи прохідний вузол
    private bool changebleData; // Чи можна змінювати дані вузла

    // Властивості з приватними методами встановлення
    public bool IsWalkable
    {
        get => isWalkable; // Повертає значення прохідності
        private set
        {
            isWalkable = value; // Встановлює значення прохідності
        }
    }
    public bool ChangebleData
    {
        get => changebleData; // Повертає значення змінності даних
        private set
        {
            changebleData = value; // Встановлює значення змінності даних
        }
    }

    public Node Parent { get; set; } // Змінна для зберігання батьківського вузла (для роботи алгоритму пошуку шляху)

    public float GCost { get; set; } // Вартість від стартового вузла до поточного
    public float HCost { get; set; } // Приблизна вартість від поточного вузла до кінцевого
    public float FCost => GCost + HCost; // Загальна вартість (GCost + HCost)
    #endregion

    private void Awake()
    {
        // Базова ініціалізація
        IsWalkable = true; // Встановлюємо вузол як прохідний за замовчуванням
        ChangebleData = true; // Встановлюємо вузол як такий, що можна змінювати за замовчуванням
    }

    public void Initialize(Vector2Int position, bool isWalkable = true, bool changebleData = true)
    {
        // Ініціалізуємо вузол з певними параметрами
        Position = position; // Встановлюємо позицію
        IsWalkable = isWalkable; // Встановлюємо прохідність
        ChangebleData = changebleData; // Встановлюємо змінність даних
    }

    public void SetWalkable(bool isWalkable)
    {
        IsWalkable = isWalkable;// Встановлюємо прохідність вузла
    }

    public void SetChangebleData(bool changebleData)
    {
        ChangebleData = changebleData;// Встановлюємо змінність даних вузла
    }
}
