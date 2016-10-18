using UnityEngine;
using System.Collections;

public class CloudSpawner : MonoBehaviour {

    [SerializeField]
    private GameObject[] clouds;
    //distance between clouds in Y direction so that player can stand on the cloud
    private float distanceBetweenClouds = 3f;
    //variables needed to control that the clouds does not go out of camera bounds
    private float minX, maxX;
    //for spawning the last cloud and generating new ones
    private float lastCloudPositionY;
    //Random generation of position of clouds along the X axes
    private float controlX;

    [SerializeField]
    private GameObject[] collectables;// for different collectables in game

    private GameObject player;

	void Awake () {
        controlX = 0;
        SetMinAndMax();
        CreateClouds();
        player = GameObject.Find("Player");

        for(int i = 0; i < collectables.Length; i++)
        {
            collectables[i].SetActive(false);
        }
	}

    void Start()
    {
        PositionThePlayer();
    }

    void SetMinAndMax()
    {
        Vector3 bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        maxX = bounds.x - 0.5f;//Setting bounds for positive side and letting more than half of the clouds to remain in domain of camera
        minX = -bounds.x + 0.5f;//Setting bounds for negative side and letting more than half of the clouds to remain in domain of camera
    }

    void Shuffle(GameObject[] arrayToShuffle)//Randomize the position of clouds on device screen
    {
        for(int i = 0; i < arrayToShuffle.Length; i++)
        {
            GameObject temp = arrayToShuffle[i];
            int random = Random.Range(1, arrayToShuffle.Length);
            arrayToShuffle[i] = arrayToShuffle[random];
            arrayToShuffle[random] = temp;
        }//Shuffling of the clouds
    }

    void CreateClouds()
    {
        //Shuffling of the array of clouds
        Shuffle(clouds);

        float positionY = 0f;
        for(int i = 0; i < clouds.Length; i++)
        {
            Vector3 temp = clouds[i].transform.position;
            temp.y = positionY;
            //to give zig zag pattern
            if (controlX == 0)
            {
                temp.x = Random.Range(0.0f, maxX);
                controlX = 1;
            }else if (controlX == 1)
            {
                temp.x = Random.Range(0.0f, minX);
                controlX = 2;
            }else if (controlX == 2)
            {
                temp.x = Random.Range(1.0f, maxX);
                controlX = 3;
            }else if (controlX == 3)
            {
                temp.x = Random.Range(-1.0f, minX);
                controlX = 0;
            }

            lastCloudPositionY = positionY;
            clouds[i].transform.position = temp;
            positionY -= distanceBetweenClouds;

        }
    }

    void PositionThePlayer()
    {
        //Getting the cloud objects
        GameObject[] darkClouds = GameObject.FindGameObjectsWithTag("Deadly");
        GameObject[] cloudsInGame = GameObject.FindGameObjectsWithTag("Cloud");

        for(int i = 0; i < darkClouds.Length; i++)//for any of the dark clouds
        {
            if(darkClouds[i].transform.position.y == 0f)//swapping of dark cloud if it occurs at the first place, with the first white cloud
            {
                Vector3 t = darkClouds[i].transform.position;
                darkClouds[i].transform.position = new Vector3(cloudsInGame[0].transform.position.x,
                                                              cloudsInGame[0].transform.position.y,
                                                              cloudsInGame[0].transform.position.z);
                cloudsInGame[0].transform.position = t;
            }
        }

        Vector3 temp = cloudsInGame[0].transform.position;//position of the first white cloud

        for(int i = 1; i < cloudsInGame.Length; i++)//swapping that cloud which is positioned at the position we want are game to start from
        {
            if (temp.y < cloudsInGame[i].transform.position.y)
            {
                temp = cloudsInGame[i].transform.position;
            }
        }

        temp.y += 0.8f;//so that the player is standing on the first cloud
        player.transform.position = temp;//player set to the position of first cloud, thus player standing on the cloud
       
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if(target.tag=="Cloud" || target.tag == "Deadly")
        {
            if (target.transform.position.y == lastCloudPositionY)
            {
                //for regeberation of clouds and collectables
                Shuffle(clouds);
                Shuffle(collectables);

                Vector3 temp = target.transform.position;
                for(int i = 0; i < clouds.Length; i++)
                {
                    if (!clouds[i].activeInHierarchy)//since the spawned clouds are deactivated
                    {
                        if (controlX == 0)
                        {
                            temp.x = Random.Range(0.0f, maxX);
                            controlX = 1;
                        }
                        else if (controlX == 1)
                        {
                            temp.x = Random.Range(0.0f, minX);
                            controlX = 2;
                        }
                        else if (controlX == 2)
                        {
                            temp.x = Random.Range(1.0f, maxX);
                            controlX = 3;
                        }
                        else if (controlX == 3)
                        {
                            temp.x = Random.Range(-1.0f, minX);
                            controlX = 0;
                        }

                        temp.y -= distanceBetweenClouds;
                        lastCloudPositionY = temp.y;
                        clouds[i].transform.position = temp;
                        clouds[i].SetActive(true);

                        //For Randomizing the position of collectables
                        int random = Random.Range(0, collectables.Length);
                        if(clouds[i].tag != "Deadly")
                        {
                            if (!collectables[random].activeInHierarchy)
                            {
                                Vector3 temp2 = clouds[i].transform.position;
                                temp2.y += 0.7f;

                                if (collectables[random].tag == "Life")
                                {
                                    if (PlayerScore.lifeCount < 2)
                                    {
                                        collectables[random].transform.position = temp2;
                                        collectables[random].SetActive(true);
                                    }
                                }else
                                {
                                    collectables[random].transform.position = temp2;
                                    collectables[random].SetActive(true);
                                }
                            }
                        }
                    }
                }
                

            }
        }
    }
	
}//Cloud Spawner










