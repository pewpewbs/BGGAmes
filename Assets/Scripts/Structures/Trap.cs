using UnityEngine;
public class Trap : MonoBehaviour//  лас дл€ представленн€ пастки у гр≥
{
    private void OnTriggerEnter(Collider other)// ћетод, €кий викликаЇтьс€ при вход≥ коллайдера в тригер
    {
        if (other.CompareTag("Player"))// ѕерев≥р€Їмо, чи об'Їкт, що з≥ткнувс€ з пасткою, маЇ тег "Player"
        {
            PlayerManager player = other.gameObject.GetComponent<PlayerManager>(); // ќтримуЇмо компонент PlayerManager з об'Їкта гравц€

            if (player.protectActivated)// ѕерев≥р€Їмо, чи активований захист у гравц€
            {
                Debug.Log("Protect");// ¬иводимо пов≥домленн€, що захист активований, ≥ виходимо з методу
                return;
            }
            else
            {
                player.Explode();// якщо захист не активований, викликаЇмо метод вибуху гравц€

                Debug.Log("You died");// ¬иводимо пов≥домленн€, що гравець загинув

                player.Lost();// ¬икликаЇмо метод, €кий обробл€Ї програш гравц€
            }
        }
    }
}
