using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField]
    public float timeBetSpawnMin = 5.0f; // 다음 배치까지의 시간 간격 최솟값
    public float timeBetSpawnMax = 30.0f; // 다음 배치까지의 시간 간격 최댓값
    public GameObject zombiePrefab;
    private Queue<GameObject> zombiePool = new Queue<GameObject>();//Object Pooling


    // void Update()
    // {
    //     //스페이스 누를때 마다 좀비 1마리 생성
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         GameObject zombie = Instantiate(zombiePrefab, transform.position, Quaternion.identity);
    //         SpriteRenderer sr = zombie.GetComponent<SpriteRenderer>();

    //         if (gameObject.CompareTag("Top"))
    //         {
    //             zombie.layer = LayerMask.NameToLayer("TopZombie");
    //             sr.sortingOrder = 1;
    //         }
    //         else if (gameObject.CompareTag("Middle"))
    //         {
    //             zombie.layer = LayerMask.NameToLayer("MiddleZombie");
    //             sr.sortingOrder = 2;
    //         }
    //         else
    //         {
    //             zombie.layer = LayerMask.NameToLayer("BottomZombie");
    //             sr.sortingOrder = 3;
    //         }
    //     }

    // }

    void Awake()
    {
        Initialize(200);
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine(){
        while(true){
            yield return new WaitForSeconds(Random.Range(timeBetSpawnMin,timeBetSpawnMax));

            var zombie = GetObject();
            zombie.transform.position = transform.position;

            SpriteRenderer sr = zombie.GetComponent<SpriteRenderer>();

            if(gameObject.tag == "Top"){
                zombie.layer = LayerMask.NameToLayer("TopZombie");
                sr.sortingOrder = 1;
            }
            else if(gameObject.tag == "Middle"){
                zombie.layer = LayerMask.NameToLayer("MiddleZombie");
                sr.sortingOrder = 2;
            }
            else{
                zombie.layer = LayerMask.NameToLayer("BottomZombie");
                sr.sortingOrder = 3;
            }
        }
    }

    IEnumerator ReturnAfterSeconds(GameObject obj,float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if(obj.activeSelf)
        {
            Debug.Log("풀에 들어간다");
            ReturnObject(obj);
        }
    }

    private GameObject CreateNew(){

        if(zombiePool.Count > 200){
            return null;
        }

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
        GameObject obj;

        if(zombiePool.Count > 0){
            obj = zombiePool.Dequeue();
        }
        else{
            obj = CreateNew();
        }

        obj.transform.SetParent(null);
        obj.gameObject.SetActive(true);

        StartCoroutine(ReturnAfterSeconds(obj,10f));

        return obj;
    }
    public void ReturnObject(GameObject obj){
        obj.SetActive(false);
        obj.transform.SetParent(gameObject.transform);
        zombiePool.Enqueue(obj);
    }
}
