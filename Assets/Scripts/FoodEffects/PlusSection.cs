using UnityEngine;

public class PlusSection : Effect
{

    public override void DoEffect()
    {
        var sections = SnakeHead.head.sections;
        var newSection = Instantiate(GameObject.Find("GameManager").GetComponent<GameManagerScript>().cellSectionPrefab,
            sections[sections.Count - 1].transform.Find("InstPosition").position, sections[0].transform.rotation)
            .AddComponent<SnakeSection>();

        sections[sections.Count - 1].pastSection = newSection;
        sections.Add(newSection);

        Destroy(gameObject);
    }
}
