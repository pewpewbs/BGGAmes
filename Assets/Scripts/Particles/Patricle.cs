using UnityEngine;
public class Patricle : MonoBehaviour//Код для кубиків що утворюються від гравця при вибуху
{
    private void OnCollisionEnter(Collision collision)//При зіткненні з будь чим фізичним
    {
        Destroy(gameObject, 2f);//Знищуємо кубики після розпаду гравця
    }
}
