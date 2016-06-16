using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public GameObject new_terrain;
    public List<GameObject> blocks;

    private GameObject player;
    private enum player_change_pos
    {
        left,
        right,
    };
    player_change_pos player_direction;

    public Color color1 = Color.red;
    public Color color2 = Color.blue;
    public float duration = 3.0F;

    Camera camera;

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
        blocks = new List<GameObject>();
        InvokeRepeating("create_blocks", 0.20f, 0.55f);
        InvokeRepeating("move_player", 0.55f, 0.55f);
        InvokeRepeating("remove_blocks", 1.5f, 0.75f);
        GameObject terrain_clone = (GameObject)Instantiate(new_terrain, new Vector3(0, 0, 0), Quaternion.identity);
        terrain_clone.tag = "right";
        blocks.Add(terrain_clone);
        player = GameObject.FindGameObjectWithTag("Player");
        player_direction = player_change_pos.right;
        camera = GetComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
	}

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if(player_direction == player_change_pos.right){
                player_direction = player_change_pos.left;
            }
            else
            {
                player_direction = player_change_pos.right;
            }
        }

        float t = Mathf.PingPong(Time.time, duration) / duration;
        camera.backgroundColor = Color.Lerp(color1, color2, t);

        Vector3 point = camera.WorldToViewportPoint(player.transform.position);
        Vector3 delta = player.transform.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
    }
	
	void create_blocks(){
        Vector3 last_item_pos = blocks[blocks.Count - 1].transform.position;
        
        float choose_direction = Random.Range(0f, 100f);

        if (choose_direction < 10f)
        {
            if (blocks[blocks.Count - 1].tag == "right")
            {
                GameObject terrain_clone = (GameObject)Instantiate(new_terrain, new Vector3(last_item_pos.x - 0.50f, last_item_pos.y + 0.25f, last_item_pos.z + 0.01f), Quaternion.identity);
                terrain_clone.tag = "left";
                blocks.Add(terrain_clone);
            }
            else
            {
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

    void move_player()
    {
        if (player_direction == player_change_pos.right)
        {
            player.transform.position = new Vector3(player.transform.position.x + 0.50f, player.transform.position.y + 0.25f, player.transform.position.z);
        }
        else
        {
            player.transform.position = new Vector3(player.transform.position.x - 0.50f, player.transform.position.y + 0.25f, player.transform.position.z);
        }
        
    }
}
