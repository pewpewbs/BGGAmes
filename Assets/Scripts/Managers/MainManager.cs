using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainManager : MonoBehaviour // �������� �� �������� ����
{
    [SerializeField] private Shield shield;//��������� �� ���

    public UnityEvent onGameRestart; // ����� ��� ����������� ���
    public UnityEvent onNextLevel;// ����� ��� �������� �� ����� �����

    [SerializeField] private PlayerManager player;// �������
    [SerializeField] private Target target;// ֳ��

    private PathCreating pathCreating;//��������� ����� ����� ������
    private Pathfinding pathFinding;//����� ���������� �����

    private Vector2Int startPosition = new(2, 2); // ������������ �������� �������
    private Vector2Int endPosition = new(27, 25);  // ������������ ������ �������

    private int sizePlatform = 30;//����� ���������

    private GridManager gridManager;//��������� �� ��������� ����
    
    private List<Node> pathForPlayer;//��������� ����
    private List<Node> path;//����� 1 ����

    private void Start()
    {
        //������ �������� ���������
        pathCreating = GetComponent<PathCreating>();
        pathFinding = GetComponent<Pathfinding>();
        gridManager = GetComponent<GridManager>();

        //ϳ��������� �� ������
        onGameRestart.AddListener(RestartGame);
        onNextLevel.AddListener(NextLevel);

        if (gridManager == null)//���� �������� ���� �������
        {
            Debug.LogError("Grid manager is null");//������ �������
            return;
        }
        else
        {
            GenerateLevel();//�������� �����

            StartGame();//��������� ���
        }
    }
    public void GenerateLevel()//��������� ����
    {
        gridManager.CreatePlatform(sizePlatform);//��������� ��������� size � size

        gridManager.SetStart(startPosition);//��������� ������
        gridManager.SetEnd(endPosition);//��������� �����

        path = pathCreating.FindPath(startPosition, endPosition);//����� ����� �� ������ �� �����(��� ��� ����� 1)
        if (path == null || path.Count == 0)
        {
            Debug.LogError("Path Creating not found");
            return;
        }

        gridManager.SetTraps(path, 5, 8);//��������� �������� �� ����� ����� 5-8 ����

        gridManager.ProcessPlatform(60, 20); //��� ���� ����� �������� ���������� ������ �� ������ �����

        target.SetPosition(endPosition);//ֳ�� �� ��� ����� ���������� �� ����������� �����
    }
    public void StartGame()//������ ���
    {
        player.SetPosition(startPosition);//������ ������� ������

        pathForPlayer = pathFinding.FindPath(startPosition, endPosition);//������ ��������� ���� ��� ������
        if (pathForPlayer == null || pathForPlayer.Count == 0)//���� ���� ����
        {
            Debug.LogError("Path for player not found");//������� �������
            return;
        }
        

        gridManager.SetColorPath(pathForPlayer, Color.blue); // ³������� ���������� ���� ������

        player.InitializePath(pathForPlayer);//³���������� ������ ��� �� ��������� ������
    }
    private void NextLevel()//��������� �����
    {
        GenerateLevel();//�������� ���������
        StartGame();//��������� ���
    }
    private void RestartGame()
    {
        player.ResetPlayer();//�������� ����������� ������ �� ���������� �����������
        shield.ResetShield();//�������� ����������� ������ �� ���������� �����������
        StartGame();//��������� ���
    }
}
