using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public enum FOOD: int
    {
        FOOD,
        POISON,
        SPEEDUP,
        SPEEDDOWN
    }
    public string[] foodsNames = new string[4] { "Food", "Poison", "SpeedUp", "SpeedDown" };

    public int x;
    public int y;
    public int z;

    public static GameObject[,,] cellsArray;
    public static Dictionary<string, GameObject> foods = new Dictionary<string, GameObject>(4);


    private void Start()
    {
        List<Transform> array = new List<Transform>();

        foreach (Transform child in transform)
        {
            array.Add(child);
        }
        cellsArray = new GameObject[x, y, z];

        for(int i = 0; i != array.Count; i++)
        {
            cellsArray[(int)array[i].localPosition.x,
                (int)array[i].localPosition.y,
                (int)array[i].localPosition.z] = array[i].gameObject;
        }

        for(int i = 0; i != foodsNames.Length; i++)
        {
            string path = ($"Prefabs/Foods/{foodsNames[i]}");
            foods.Add(foodsNames[i], Resources.Load<GameObject>(path));
        }

    }

    /// <summary>
    /// Return index of cellsArray
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    public static (int, int, int) GetIndex(GameObject cell)
    {
        for (int i = 0; i != cellsArray.GetUpperBound(0) + 1; i++)
        {
            for (int j = 0; j != cellsArray.GetUpperBound(1) + 1; j++)
            {
                for (int k = 0; k != cellsArray.GetUpperBound(2) + 1; k++)
                {
                    if (cellsArray[i, j, k] == null)
                        continue;
                    if (cellsArray[i, j, k].Equals(cell))
                    {
                        return (i, j, k);
                    }
                }
            }
        }
        return (-1, -1, -1);
    }
}
