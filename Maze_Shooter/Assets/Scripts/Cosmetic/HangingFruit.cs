using UnityEngine;
using Sirenix.OdinInspector;

public class HangingFruit : MonoBehaviour
{
    public Transform vine;
    public Transform fruit;
    public bool randomizeOnStart = true;
    public float vineBaseLength = 1;
    [HorizontalGroup("vine", Title = "Vine Length"), LabelText("min"), LabelWidth(30)]
    public float minVineLength = 1;
    [HorizontalGroup("vine"), LabelText("max"), LabelWidth(30)]
    public float maxVineLength = 3;
    [HorizontalGroup("fruit", Title = "Fruit Scale"), LabelText("min"), LabelWidth(30)]
    public float minFruitScale = 1;
    [HorizontalGroup("fruit"), LabelText("min"), LabelWidth(30)]
    public float maxFruitScale = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        if(randomizeOnStart) Randomize();
    }

    [Button]
    void Randomize()
    {
        float vineLength = Random.Range(minVineLength, maxVineLength);
        vine.localScale = new Vector3(1, vineLength, 1);
        fruit.localPosition = Vector3.down * vineLength * vineBaseLength;

        float fruitScale = Random.Range(minFruitScale, maxFruitScale);
        fruit.localScale = Vector3.one * fruitScale;
    }
}
