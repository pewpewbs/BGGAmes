using UnityEngine;

public class Exit : MonoBehaviour
{
    public void OnExitButtonClick()//При нажатті кнопки
    {
        Application.Quit();// Викликаємо метод для виходу з гри

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;// Для виходу з гри у редакторі:
#endif
    }
}
