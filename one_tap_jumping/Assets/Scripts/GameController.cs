using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;


public class GameController : MonoBehaviour {

    public GameObject new_terrain;
    public GameObject player;
    public List<GameObject> blocks;
    private GameObject first_terrain;

    
    public Color color1 = Color.red;
    public Color color2 = Color.blue;
    public float duration = 5.0F;

    Camera camera;

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

    float x_offset = 0.50f;
    float y_offset = 0.25f;
    float z_offset = 0.1f;

	// Use this for initialization
	void Start () {
        blocks = new List<GameObject>();
        InvokeRepeating("create_first_blocks", 0, 0.3f);
        camera = GetComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        first_terrain = GameObject.FindGameObjectWithTag("first");
        blocks.Add(first_terrain);
	}

    void Update()
    {
        //Change background color
        float t = Mathf.PingPong(Time.time, duration) / duration;
        camera.backgroundColor = Color.Lerp(color1, color2, t);

        //Camera smooth follow player
        if (!player.GetComponent<player>().dead)
        {
            Vector3 point = camera.WorldToViewportPoint(player.transform.position);
            Vector3 delta = player.transform.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.25f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
        else
        {
            CancelInvoke();
        }
    }

    void create_first_blocks()
    {
        
        GameObject terrain_clone = (GameObject)Instantiate(new_terrain, new Vector3(x_offset, y_offset, z_offset), Quaternion.identity);
        terrain_clone.tag = "right";
        blocks.Add(terrain_clone);
        
        x_offset += 0.5f;
        y_offset += 0.25f;
        z_offset += 0.1f;

        if (blocks.Count == 3)
        {
            InvokeRepeating("create_blocks", 0.3f, 0.3f);
            InvokeRepeating("remove_blocks", 1.2f, 0.3f);
            CancelInvoke("create_first_blocks");
        }
    }
	
	void create_blocks(){
        Vector3 last_item_pos = blocks[blocks.Count - 1].transform.position;
        
        float choose_direction = Random.Range(0f, 100f);

        if (choose_direction < 20f)
        {
            if (blocks[blocks.Count - 1].tag == "right")
            {
                blocks[blocks.Count - 1].tag = "left";
                GameObject terrain_clone = (GameObject)Instantiate(new_terrain, new Vector3(last_item_pos.x - 0.50f, last_item_pos.y + 0.25f, last_item_pos.z + 0.01f), Quaternion.identity);
                terrain_clone.tag = "left";
                blocks.Add(terrain_clone);
            }
            else
            {
                blocks[blocks.Count - 1].tag = "right";
                GameObject terrain_clone = (GameObject)Instantiate(new_terrain, new Vector3(last_item_pos.x + 0.50f, last_item_pos.y + 0.25f, last_item_pos.z + 0.01f), Quaternion.identity);
                terrain_clone.tag = "right";
                blocks.Add(terrain_clone);
            }
        }
        else
        {
            if (blocks[blocks.Count - 1].tag == "right")
            {
                GameObject terrain_clone = (GameObject)Instantiate(new_terrain, new Vector3(last_item_pos.x + 0.50f, last_item_pos.y + 0.25f, last_item_pos.z + 0.01f), Quaternion.identity);
                terrain_clone.tag = "right";
                blocks.Add(terrain_clone);
            }
            else
            {
                GameObject terrain_clone = (GameObject)Instantiate(new_terrain, new Vector3(last_item_pos.x - 0.50f, last_item_pos.y + 0.25f, last_item_pos.z + 0.01f), Quaternion.identity);
                terrain_clone.tag = "left";
                blocks.Add(terrain_clone);
            }
        }
    }

    void remove_blocks()
    {
        Destroy(blocks[0].gameObject);
        blocks.Remove(blocks[0]);
    }

    public void restart_level()
    {
        //SceneManager.LoadScene(0);
        player.GetComponent<SpriteRenderer>().sortingLayerName = "player";
        player.GetComponent<Transform>().transform.position = blocks[1].transform.position;
        player.GetComponent<player>().dead = false;
        player.GetComponent<player>().start_moving(blocks[1].tag);
        InvokeRepeating("create_blocks", 1f, 0.3f);
        InvokeRepeating("remove_blocks", 1.2f, 0.3f);
    }

    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideoZone"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideoZone", options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                // YOUR CODE TO REWARD THE GAMER
                /*player.GetComponent<SpriteRenderer>().sortingLayerName = "player";
                player.GetComponent<Transform>().transform.position = blocks[1].transform.position;
                player.GetComponent<player>().dead = false;
                player.GetComponent<player>().start_moving(blocks[1].tag);
                InvokeRepeating("create_blocks", 2f, 0.55f);
                InvokeRepeating("remove_blocks", 2.5f, 0.55f);*/
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
}
