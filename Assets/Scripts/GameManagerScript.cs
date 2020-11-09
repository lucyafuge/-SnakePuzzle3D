using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    RaycastHit hit;

    public GameObject cellSectionPrefab;

    public static int size;
    private float timer = 0;

    [SerializeField][Range(0.2f, 1)] private float speedTicCall;
    [SerializeField][Range(0, 0.2f)] private float smoothMoveSpeed;

    private void Start()
    {
        cellSectionPrefab = Resources.Load<GameObject>("Prefabs/SnakeSection");
    }

    private void Update()
    {
        if (timer >= speedTicCall)
        {
            Tic();
            timer = 0;
        }
        else
            timer += Time.deltaTime;
    }

    private void Tic()
    {
        //Index of the cell the head is located
        var positionIndex = LevelData.GetIndex(GetUnderHead(SnakeHead.head).transform.parent.gameObject);

        //The object of the cell face on which the head is located
        var face = GetUnderHead(SnakeHead.head);

        if (Physics.Raycast(face.transform.position, SnakeHead.head.transform.TransformDirection(Vector3.back), out hit, 1))
        {
            if (face.name == hit.collider.gameObject.name)
                SnakeHead.head.Move(hit.collider.transform.position, smoothMoveSpeed, SnakeHead.head.transform.rotation);
            else
                SnakeHead.head.Move(hit.collider.transform.position, smoothMoveSpeed, -90);
        }
    }

    /// <summary>
    /// Get cell face under snake head
    /// </summary>
    /// <param name="head"></param>
    /// <returns></returns>
    public GameObject GetUnderHead(SnakeHead head)
    {
        RaycastHit hit;
        if (Physics.Raycast(head.rayCaster.position, head.transform.TransformDirection(Vector3.down), out hit, 1))
        {
            return hit.collider.gameObject;
        }
        return null;
    }
    /// <summary>
    /// Method for instantiate food in level
    /// </summary>
    /// <param name="foodIndex"></param>
    public static void FoodInstantiate(string foodIndex)
    {
        var randomCell = LevelData.cellsArray[Random.Range(0, LevelData.cellsArray.GetUpperBound(0) + 1),
                    Random.Range(0, LevelData.cellsArray.GetUpperBound(1) + 1),
                    Random.Range(0, LevelData.cellsArray.GetUpperBound(2) + 1)];

        if (randomCell != null)
        {
            foreach (Transform face in randomCell.transform)
            {
                if (face.gameObject.tag != "Face") continue;

                var cell = face.GetComponent<Cell>();
                if (cell.objectInFace == Cell.OBJECTS.NONE && !cell.hasFood && face.gameObject.activeSelf)
                {
                    Instantiate(LevelData.foods[foodIndex], face.transform.position, face.transform.rotation, face.transform);
                    cell.hasFood = true;
                    return;
                }
            }
        }
        else
            FoodInstantiate(foodIndex);
    }
}