using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : SnakeBase
{
    public List<SnakeBase> sections;
    public Transform rayCaster;

    private void Start()
    {
        sections.Add(this);
        head = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.tag == "Food")
            GetFood(other.GetComponent<Effect>());
        else if (other.tag == "Obstacle")
            Death();
        else if (other.tag == "Snake")
        {
            Debug.Log(other.GetComponent<SnakeBase>().pastSection);
            other.GetComponent<SnakeBase>().Death();
        }
    }

    public void GetFood(Effect effect)
    {
        effect.DoEffect();
        GameManagerScript.FoodInstantiate("Food");
    }

    //This method will be redone
    public void RotateHead(float rotate)
    {
        transform.Rotate(new Vector3(0, rotate, 0));
    }

    public int GetIndex(GameObject section)
    {
        for(int i = 0; i != head.sections.Count; i++)
        {
            if (head.sections[i].gameObject == section)
                return i;
        }
        return -1;
    }
}
