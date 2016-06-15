using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class terrain_blocks : MonoBehaviour
{

    private Vector3 pos;
    private Vector3 prev_block_pos;
    public GameObject new_terrain;
    public List<GameObject> blocks;

    // Use this for initialization
    void Start()
    {
        pos = this.transform.position;
        Invoke("blocks_creation", 0.5f);
        InvokeRepeating("move_blocks", 0.5f, 0.5f);
        blocks = new List<GameObject>();
        blocks.Add(this.gameObject);
    }

    void blocks_creation()
    {
        float choose_direction = Random.Range(0f, 100f);

        if (choose_direction < 10f)
        {
            if (this.gameObject.tag == "right")
            {
                GameObject terrain_clone = (GameObject)Instantiate(new_terrain, new Vector3(pos.x - 0.50f, pos.y + 0.25f, pos.z + 0.01f), Quaternion.identity);
                terrain_clone.tag = "left";
            }
            else
            {
                GameObject terrain_clone = (GameObject)Instantiate(new_terrain, new Vector3(pos.x + 0.50f, pos.y + 0.25f, pos.z + 0.01f), Quaternion.identity);
                terrain_clone.tag = "right";
            }
        }
        else
        {
            if (this.gameObject.tag == "right")
            {
                GameObject terrain_clone = (GameObject)Instantiate(new_terrain, new Vector3(pos.x + 0.50f, pos.y + 0.25f, pos.z + 0.01f), Quaternion.identity);
                terrain_clone.tag = "right";
            }
            else
            {
                GameObject terrain_clone = (GameObject)Instantiate(new_terrain, new Vector3(pos.x - 0.50f, pos.y + 0.25f, pos.z + 0.01f), Quaternion.identity);
                terrain_clone.tag = "left";
            }
        }
    }

    void move_blocks()
    {

    }
    //GameObject.Destroy(this);
}
