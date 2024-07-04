using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ���� ��� ������� ����, ���� ���������� ����������� ������
public class Shield : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private PlayerManager player; // ��������� �� PlayerManager ��� ��������� �������

    public float holdDuration = 2f; // ��������� ��������� ������ � ��������
    private bool isHolding = false; // ³��������, �� ���������� ������
    private bool holdLimit = false; // ˳�� ��������� (�������� ���������� ���� ������������)
    private float holdTime = 0f; // ��� ��������� ������

    private Button button; // ��������� �� ��������� ������
    void Start()
    {
        button = GetComponent<Button>(); // ��������� ��������� Button �� ����� ��'���
        if (button == null)
        {
            Debug.LogError("Button component not found on this GameObject."); // �������� �������, ���� ������ �� ��������
        }
    }

    void Update()
    {
        if (isHolding && !holdLimit) // ���� ������ ���������� � ���� ���� ���������
        {
            holdTime += Time.unscaledDeltaTime; // ���� ��� ��������� ��� ���������� Time.timeScale
            if (holdTime >= holdDuration) // ���� ��� ��������� �������� ��������� ���������
            {
                player.DeactivateShield(); // �������� ��� � ������
                holdLimit = true; // ���������� �������� �� ����� �� ���� ��������� ���������
            }
        }
        if (holdLimit) // ���� ���� ��������� ���������
        {
            holdTime += Time.unscaledDeltaTime; // ���� ��� ��������� ��� ���������� Time.timeScale
            if (holdTime >= holdDuration) // ���� ��� ��������� �������� ��������� ���������
            {
                holdLimit = false; // �������� ���
                holdTime = 0f; // ����� ��� ���������
                button.interactable = true; // ������ ������ ����� ��������� ��� ����������
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)//��� ������ ������
    {
        if (!isHolding && !holdLimit) // ���� ������ �� ���������� � ���� ���� ���������
        {
            player.ActivateShield(); // ������ ��� � ������
            isHolding = true; // ���������� ���� ���������
        }
    }
    public void OnPointerUp(PointerEventData eventData)//��� ������ ������
    {
        isHolding = false; // ������ ���� ���������
        player.DeactivateShield(); // �������� ��� � ������
        holdTime = 0f; // ����� ��� ���������
    }
    public void ResetShield()
    {
        isHolding = false; // ������ ���� ���������
        holdLimit = false; // ������ ��� ���������
        holdTime = 0f; // ����� ��� ���������
        button.interactable = true; // ������ ������ ����� ��������� ��� ����������
    }
}
