using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour {

	public NavMeshSurface surfaceNav;

	public float width = 10;
	public float height = 10;

	public float wallProbability = 0.6f;
	public float bombProbability = 0.9f;
	public float enemySpawnerProbability = 1f;
	public float timeTrapProbability = 0.4f;

	public GameObject wall;
	public GameObject player;
	public GameObject bomb;
	public GameObject enemySpawner;
    public GameObject boss;

	public GameObject timeTrap;

	public int currentBombs = 0;
	public int maxBombs = 5;

	public int currentEnemySpawners = 0;
	public int maxEnemySpawners = 6;
	public int currentTimeTraps = 0;
	public int maxTimeTraps = 4;

	private bool playerSpawned = false;
    private bool bossSpawned = false;

    private void Awake()
    {
		surfaceNav = GameObject.Find("Navmesh").gameObject.GetComponent<NavMeshSurface>();
		GenerateLevel();
		surfaceNav.BuildNavMesh();
	}
    // Use this for initialization
    void Start () {
		
	}
	
	// Create a grid based level
	void GenerateLevel()
	{
		// Loop over the grid
		for (float x = 0; x <= width; x += 2f)
		{
			for (float y = 0; y <= height; y+= 2f)
			{

                int random = Random.Range(0, 20);

                //Debug.Log(random);
                switch (random)
                {
                    case 0:
                        // DO NOTHING

                        break;
                    case 1:
                        // DO NOTHING

                        break;
                    case 2:
                        // DO NOTHING

                        break;
                    case 3:
                        // DO NOTHING

                        break;
                    case 4:
                        // DO NOTHING

                        break;

                    case 5:
                        // Spawn a wall
                        //Vector3 pos = new Vector3(x - width / 2f, 1f, y - height / 2f);
                        Vector3 wallPos_ = new Vector3(x - width / 2f, y - height / 2f, -0.63f);
                        Instantiate(wall, wallPos_, wall.transform.rotation, transform);
                        break;


                    case 6:
                        // Spawn a wall
                        //Vector3 pos = new Vector3(x - width / 2f, 1f, y - height / 2f);
                        Vector3 wallPos = new Vector3(x - width / 2f, y - height / 2f, -0.63f);
                        Instantiate(wall, wallPos, wall.transform.rotation, transform);
                        break;

                    case 7:
                        if(currentBombs < maxBombs)
                        {
                            //SPAWN A BOMB
                            Vector3 bombPos = new Vector3(x - width / 2f, y - height / 2f, bomb.transform.position.z);
                            Instantiate(bomb, bombPos, bomb.transform.rotation, transform); //HIJO DE LEVEL GENERATOR, OJO!
                            currentBombs++;
                        }
                       
                        break;

                    case 8:
                        if(currentEnemySpawners < maxEnemySpawners)
                        {
                            //SPAWN A ENEMYSPAWNER
                            Vector3 enemySpawnerPos = new Vector3(x - width / 2f, y - height / 2f, enemySpawner.transform.position.z);
                            Instantiate(enemySpawner, enemySpawnerPos, enemySpawner.transform.rotation, transform); //HIJO DE LEVEL GENERATOR, OJO!
                            currentEnemySpawners++;
                        }
                      
                        break;

                    case 9:
                        if(currentTimeTraps < maxTimeTraps)
                        {
                            //SPAWN A TIMETRAP
                            Vector3 timeTrapPos = new Vector3(x - width / 2f, y - height / 2f, timeTrap.transform.position.z);
                            Instantiate(timeTrap, timeTrapPos, timeTrap.transform.rotation, transform); //HIJO DE LEVEL GENERATOR, OJO!
                            currentTimeTraps++;
                        }
                       
                        break;
                    case 10:
                        if(!playerSpawned)
                        {
                            // Spawn the player
                            //Vector3 pos = new Vector3(x - width / 2f, 1.25f, y - height / 2f);
                            Vector3 playerPos = new Vector3(x - width / 2f, y - height / 2f, -0.15f);
                            Instantiate(player, playerPos, player.transform.rotation);
                            playerSpawned = true;
                        }
                       
                        break;

                    case 15:
                        if (!bossSpawned)
                        {
                            // Spawn the player
                            //Vector3 pos = new Vector3(x - width / 2f, 1.25f, y - height / 2f);
                            Vector3 bossPos = new Vector3(x , y , -0.15f);
                            Instantiate(boss, bossPos, boss.transform.rotation);
                            bossSpawned = true;
                        }

                        break;
                }

                        //float random = Random.value;

                        //            // Should we place a wall?
                        //            if ((random < (1 - wallProbability)  && (random > 1 - bombProbability) && (random > 1 - timeTrapProbability) && (random > 1 - enemySpawnerProbability)))
                        //            {
                        //                // Spawn a wall
                        //                //Vector3 pos = new Vector3(x - width / 2f, 1f, y - height / 2f);
                        //                Vector3 pos = new Vector3(x - width / 2f, y - height / 2f, -0.63f);
                        //                Instantiate(wall, pos, wall.transform.rotation, transform);
                        //            }
                        //            if ((random < (1 - bombProbability))  && (currentBombs < maxBombs)) //&& (random > 1 - timeTrapProbability) && (random > 1-enemySpawnerProbability) && (x >= width / 2 || x <= width / 2)
                        //{
                        //                //SPAWN A BOMB
                        //                Vector3 pos = new Vector3(x - width / 2f, y - height / 2f, bomb.transform.position.z);
                        //                Instantiate(bomb, pos, bomb.transform.rotation, transform); //HIJO DE LEVEL GENERATOR, OJO!
                        //                currentBombs++;
                        //            }
                        //if ((random < (1 - timeTrapProbability)) && (currentTimeTraps < maxTimeTraps)) //&& (random > 1 - bombProbability) && (random > 1 - enemySpawnerProbability) && (x >= width / 2 || x <= width / 2)
                        //{
                        //	//SPAWN A TIMETRAP
                        //	Vector3 pos = new Vector3(x - width / 2f, y - height / 2f, timeTrap.transform.position.z);
                        //	Instantiate(timeTrap, pos, timeTrap.transform.rotation, transform); //HIJO DE LEVEL GENERATOR, OJO!
                        //	currentTimeTraps++;
                        //}


                        //if ((random <= (1 - enemySpawnerProbability) && (currentEnemySpawners < maxEnemySpawners))) //&& (random > 1 - bombProbability) && (random > 1 - timeTrapProbability)  && (x >= width / 2 || x <= width / 2) 
                        //{
                        //                //SPAWN A ENEMYSPAWNER
                        //                Vector3 pos = new Vector3(x - width / 2f, y - height / 2f, -enemySpawner.transform.position.z);
                        //                Instantiate(enemySpawner, pos, enemySpawner.transform.rotation, transform); //HIJO DE LEVEL GENERATOR, OJO!
                        //                currentEnemySpawners++;
                        //            }

                        //else if
    //                    float random = Random.value;
    //            if ((random > 0.6) && (random > (1-wallProbability)) && (!playerSpawned) && (x >= width/2)) // Should we spawn a player? //&& (random > 1 - bombProbability) && (random > 1 - enemySpawnerProbability) && (random > 1 - timeTrapProbability) && 
				//{
    //                // Spawn the player
    //                //Vector3 pos = new Vector3(x - width / 2f, 1.25f, y - height / 2f);
    //                Vector3 pos = new Vector3(x - width / 2f, y - height / 2f, -0.15f);
    //                Instantiate(player, pos, player.transform.rotation);
    //                playerSpawned = true;
    //            }
            }
		}
	}

}
