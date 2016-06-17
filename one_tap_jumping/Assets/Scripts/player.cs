using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class player : MonoBehaviour {

    private enum player_change_pos
    {
        left,
        right,
    };
    player_change_pos player_direction;

    private Rigidbody2D player_body;
    public bool dead = false;

    public GameObject score_text;
    private int score = 0;

    public GameObject[] active_ui_objects;

    private SpriteRenderer sprite_renderer;

	// Use this for initialization
	void Start () {
        player_direction = player_change_pos.right;
        InvokeRepeating("move_player", 2.0f, 0.55f);
        player_body = GetComponent<Rigidbody2D>();
        score_text.GetComponent<Text>().text = score.ToString();
        sprite_renderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        //change player direction when pressing space
        if (Input.GetKeyDown("space"))
        {
            if (player_direction == player_change_pos.right)
            {
                player_direction = player_change_pos.left;
            }
            else
            {
                player_direction = player_change_pos.right;
            }
        }
	}

    void move_player()
    {
        if (player_direction == player_change_pos.right)
        {
            transform.position = new Vector3(transform.position.x + 0.50f, transform.position.y + 0.25f, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x - 0.50f, transform.position.y + 0.25f, transform.position.z);
        }
        score += 1;
        score_text.GetComponent<Text>().text = score.ToString();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "left" && player_direction == player_change_pos.right ||
            other.gameObject.tag == "right" && player_direction == player_change_pos.left)
        {
            CancelInvoke();
            player_body.gravityScale = 1;
            dead = true;
            sprite_renderer.sortingLayerName = "Default";
            score_text.SetActive(false);
            foreach (GameObject ui_objects in active_ui_objects)
            {
                ui_objects.SetActive(true);
            }
        }
    }
}
