using UnityEngine;

[ExecuteInEditMode]
[System.Serializable]
public class Cell : MonoBehaviour
{
    public enum OBJECTS : int
    {
        NONE,
        WALL,
        BEAM,
        SPEEDUP,
        SPEEDDOWN
    }
    public OBJECTS objectInFace;
    public bool hasFood;

    [SerializeField] Quaternion rotation;
    [SerializeField] ObjectsContainer ObjectesInCell;

    void Update()
    {
        for(int i = 0; i != ObjectesInCell.objects.Length; i++)
        {
            if ((int)objectInFace == i)
            {
                ObjectesInCell.objects[i].SetActive(true);
                ObjectesInCell.objects[i].transform.rotation = rotation;
            }
            else
                ObjectesInCell.objects[i].SetActive(false);

        }
    }
}