using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform food;
    [SerializeField] Vector3 centerPoint;
    [SerializeField] Vector3 size;

    public ObjectPooler pooler;
    Color newCol;

    // Start is called before the first frame update
    void Start()
    {
        spawnable();
    }

    Vector3 spawnLocation()
    {
        float randomX = Random.Range(-size.x / 2, size.x / 2);
        float randomY = Random.Range(-size.y / 2, size.y / 2);
        float randomZ = Random.Range(-size.z / 2, size.z / 2);

        Vector3 position = centerPoint + new Vector3(randomX, randomY, randomZ);

        return position;
    }

    public void spawnable()
    {
        int randomColor = Random.Range(0, GameManager.instance.foods.Foods.Length);

        string color = GameManager.instance.foods.Foods[randomColor].color;
        ColorUtility.TryParseHtmlString(color, out newCol);

        Vector3 randomPoint = spawnLocation();
        randomPoint.y = 1;

        GameObject gObj = pooler.GetObject(food.name);
        gObj.GetComponent<MeshRenderer>().material.color = newCol;
        gObj.transform.position = randomPoint;
        gObj.transform.rotation = food.rotation;

        gObj.GetComponent<Collectible>().points = int.Parse(GameManager.instance.foods.Foods[randomColor].points);

        gObj.SetActive(true);

    }
}
