using System.Collections;
using UnityEngine;

public abstract class SnakeBase : MonoBehaviour, ISnake
{
    public SnakeBase pastSection;
    public static SnakeHead head;

    public void Death()
    {
        Debug.Log(head.GetIndex(gameObject));
        if (pastSection != null)
            pastSection.Death();

        head.sections.RemoveAt(head.GetIndex(gameObject));
       
        for(int i = 0; i != head.sections.Count; i++)
        Debug.Log(head.sections.Count);
        Destroy(gameObject);
    }

    public void Move(Vector3 position, float smoothMoveSpeed, Quaternion rotation)
    {
        if (pastSection != null)
            pastSection.Move(transform.position, smoothMoveSpeed, transform.rotation);

        StartCoroutine(SmoothMove(transform.position, position, smoothMoveSpeed));
        transform.rotation = rotation;
    }

    public void Move(Vector3 position, float smoothMoveSpeed, float xRotation)
    {
        if (pastSection != null)
            pastSection.Move(transform.position, smoothMoveSpeed, transform.rotation);

        StartCoroutine(SmoothMove(transform.position, position, smoothMoveSpeed));
        transform.Rotate(new Vector3(xRotation, 0, 0));
    }

    IEnumerator SmoothMove(Vector3 startPositin, Vector3 endPosition, float time)
    {
        float currentTime = 0;
        do
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPositin, endPosition, (currentTime / time));
            yield return null;
        }
        while (currentTime <= time);
    }
}
