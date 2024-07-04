using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    
    [SerializeField]private float moveSpeed = 5.0f; // �������� ���� ������
    [SerializeField] private MainManager mainManager; // ��� ������

    private int currentNodeIndex = 0; // ������ ��������� ����� �� �����
    private bool isMoving = false; // �� �������� ������� � ����� ������
    public bool protectActivated = false; // �� ����������� ������ ������

    private Coroutine moveCoroutine;//��������� �� �������� ��� ����

    public GameObject cubePrefab; // ������ ��� ������ ����

    private Renderer playerRenderer;//��������� ���� �� ����� �������� ����
    private Rigidbody playerRigidbody;//��������� �� �� ����� �������� ���. �������������
    private Collider playerCollider;//��������� ���� ���� ��'���� �������� ��'��

    public Color notProtect = new(255 / 255f, 255 / 255f, 14 / 255f, 255 / 255f);//���������� ���������� ������� ������
    public Color protect = new(173 / 255f, 255 / 255f, 47 / 255f, 255 / 255f);//���������� ������� � ���� ������� ������

    private void Awake()
    {
        //������ ��������� �� �� ������ ����������
        playerRenderer = GetComponent<Renderer>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
    }
    private void Start()
    {
        if (mainManager == null)//���� ��������� �� ����
        {
            Debug.LogError("mainManager == null");//������ �������
        }
    }
    public void InitializePath(List<Node> path)// ����������� ����� ��� ���� ������
    {
        currentNodeIndex = 0; // �������� ������� �����
        isMoving = false; // �������� ����� ����
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);//���� �������� ��� � �� ��������� ��
        moveCoroutine = StartCoroutine(MoveAlongPath(path)); // ��������� �������� ��� ���� �� �����
    }
    public void SetPosition(Vector2Int position)// ������������ ������� ������ �� ������������ ����
    {
        gameObject.SetActive(true);//������ ���� �������� ��� �����
        Vector3 spawnPos = new(position.x, 1, position.y); // ������������ ������� ������
        gameObject.transform.position = spawnPos;//�������� � ����� �������
        playerRigidbody.isKinematic = false; // �������� ������
        playerCollider.enabled = true; // ������� ��������
    }
    private IEnumerator MoveAlongPath(List<Node> path)// �������� ��� ���� ������ �� �����
    {
        yield return new WaitForSeconds(2); // �������� ����� �������� ����

        isMoving = true; // ������������ ���� ����
        gameObject.tag = "Player"; // ������������ ��� ������

        while (isMoving && currentNodeIndex < path.Count)// �������� �� �����, ���� � �����
        {
            Node targetNode = path[currentNodeIndex];//������ ���� � ����� �� �������� �����
            Vector3 targetPosition = new(targetNode.Position.x, 0.75f, targetNode.Position.y);//�������� � ����� ��� ������� ���� ��� ������ 0.75 ��� ������� ��� ���� �����

            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)// ��� �� ���������� �����
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);//����� �� �������
                yield return null;
            }
            currentNodeIndex++;// ������� �� ���������� �����
        }
        isMoving = false; // ���������� ����
    }

    
    public void Explode(int numberOfCubes = 20)// ����� ��� "������" ������
    {
        playerRigidbody.isKinematic = true;// ��������� ��� ������
        playerCollider.enabled = false;//�������� ���� ��������

        for (int i = 0; i < numberOfCubes; i++)// �������� ������� ������� �� ���� ����
        {
            CreateSmallCube();//��������� ����
        }
        gameObject.SetActive(false);// ������ ������ ���������� ���� ������
    }
    private void CreateSmallCube()// ��������� ������� ����
    {
        GameObject smallCube = Instantiate(cubePrefab, transform.position, Random.rotation);// ��������� ������ ���

        Rigidbody rb = smallCube.AddComponent<Rigidbody>();//������ ���� ��������� �� ������� �� ������ ����������
        rb.velocity = Random.insideUnitSphere * 5f;// ������������ ��������� �������� ��� ������� ����
    }
    public void SetColor(Color color)// ������� ��� ������������ �������
    {
        playerRenderer.material.color = color;// ������������ ������� ������
    }
    public void ResetPlayer()// ����� ��� �������� ����� ������ ���� �������
    {
        gameObject.SetActive(true);
        playerRigidbody.isKinematic = false;
        playerCollider.enabled = true;
        protectActivated = false;
        SetColor(notProtect);
    }
    public void Lost()// ����� ��� ����������� ��� ���� �������
    {
        mainManager.onGameRestart?.Invoke();// ��������� ����� ����������� ���
        ResetPlayer();//������� ������������ ������
    }
    public void ActivateShield()//��������� �������
    {
        protectActivated = true;//������ �������
        SetColor(protect);//³������� ���� �������
    }
    public void DeactivateShield()//����������� �������
    {
        protectActivated = false;//������ ��������
        SetColor(notProtect);//³������� ���� �������
    }
}
