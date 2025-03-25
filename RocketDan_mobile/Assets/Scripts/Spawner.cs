using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField]
    public float timeBetSpawnMin = 0.5f; // 다음 배치까지의 시간 간격 최솟값
    public float timeBetSpawnMax = 2.0f; // 다음 배치까지의 시간 간격 최댓값
    public GameObject zombiePrefab;
    private Queue<GameObject> zombiePool = new Queue<GameObject>();
    private float timeBetSpawn; // 다음 배치까지의 시간 간격
    private float lastSpawnTime;
    private int count = 3;

    void Awake()
    {
        Initialize(30);
        lastSpawnTime = 0f;
        timeBetSpawn = 0f;
    }

    void Update()
    {
        if(Time.time >= lastSpawnTime + timeBetSpawn){
            lastSpawnTime = Time.time;

            timeBetSpawn = Random.Range(timeBetSpawnMin,timeBetSpawnMax);

            var zombie = GetObject();
            zombie.transform.position = transform.position;

            if(gameObject.tag == "Top")
                zombie.layer = LayerMask.NameToLayer("TopZombie");
            else if(gameObject.tag == "Middle")
                zombie.layer = LayerMask.NameToLayer("MiddleZombie");
            else{
                zombie.layer = LayerMask.NameToLayer("BottomZombie");
            }
        }
    }

    private GameObject CreateNew(){
        GameObject newZombie = Instantiate(zombiePrefab,transform);
        newZombie.gameObject.SetActive(false);
        return newZombie;
    }

    private void Initialize(int count){
        for(int i = 0 ; i < count ; i++){
            zombiePool.Enqueue(CreateNew());
        }
    }

    public GameObject GetObject(){
        if(zombiePool.Count > 0){
            GameObject obj = zombiePool.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else{
            GameObject obj = CreateNew();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }

    }
    public void ReturnObject(GameObject obj){
        obj.SetActive(false);
        obj.transform.SetParent(gameObject.transform);
        zombiePool.Enqueue(obj);
    }
}
