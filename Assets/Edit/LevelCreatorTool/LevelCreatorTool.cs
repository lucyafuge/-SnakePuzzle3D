using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelCreatorTool : MonoBehaviour
{
    /// <summary>
    /// Distance between cells in level
    /// </summary>
    [SerializeField] float step;

    /// <summary>
    /// Cell prefab
    /// </summary>
    [SerializeField] GameObject cell;

    private Dictionary<string, (string, int)> activeFaces = new Dictionary<string, (string, int)>
    {
        ["Back"] = ("z", 0),
        ["Right"] = ("x", 1),
        ["Front"] = ("z", 1),
        ["Left"] = ("x", 0),
        ["Top"] = ("y", 1),
        ["Bottom"] = ("y", 0),
    };

    void Awake()
    {
        gameObject.SetActive(false);
    }
    void Update()
    {
        var tmp = GameObject.FindGameObjectsWithTag("Cell");
        for (int i = 0; i != tmp.Length; i++)
            DestroyImmediate(tmp[i]);

        var levelData = LevelParent().GetComponent<LevelData>();

        levelData.x = (int)Math.Abs(transform.localScale.x);
        levelData.y = (int)Math.Abs(transform.localScale.y);
        levelData.z = (int)Math.Abs(transform.localScale.z);
        for (int i = 0; i != levelData.x; i++)
        {
            for (int j = 0; j != levelData.y; j++)
            {
                for (int k = 0; k != levelData.z; k++)
                {
                    if ((i != 0 && i != levelData.x - 1) && (j != 0 && j != levelData.y - 1) && (k != 0 && k != levelData.z - 1)) continue;
                    var newCell = Instantiate(cell, new Vector3(i + step * i, j + step * j, k + step * k), Quaternion.identity, LevelParent().transform);

                    foreach(Transform face in newCell.transform)
                    {
                        switch(activeFaces[face.name].Item1)
                        {
                            case "x":
                                if ((int)newCell.transform.position.x == 0 && activeFaces[face.name].Item2 == 0 ||
                                    (int)newCell.transform.position.x == levelData.x -1 && activeFaces[face.name].Item2 == 1)
                                    face.gameObject.SetActive(true);
                                break;
                            case "y":
                                if ((int)newCell.transform.position.y == 0 && activeFaces[face.name].Item2 == 0 ||
                                    (int)newCell.transform.position.y == levelData.y -1 && activeFaces[face.name].Item2 == 1)
                                    face.gameObject.SetActive(true);
                                break;
                            case "z":
                                if ((int)newCell.transform.position.z == 0 && activeFaces[face.name].Item2 == 0 ||
                                    (int)newCell.transform.position.z == levelData.z -1 && activeFaces[face.name].Item2 == 1)
                                    face.gameObject.SetActive(true);
                                break;
                        }
                    }
                }
            }
        }
    }

    GameObject LevelParent()
    {
        if (GameObject.Find("Level") == null)
            return new GameObject("Level", typeof(LevelData));
        else return GameObject.Find("Level");
    }
}