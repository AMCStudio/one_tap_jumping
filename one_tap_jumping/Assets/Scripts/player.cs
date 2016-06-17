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

    private GameController game_controller_script;

    private Vector3 new_pos;

	// Use this for initialization
	void Start () {
        InvokeRepeating("move_player", 1.0f, 0.01f);
        player_body = GetComponent<Rigidbody2D>();
        score_text.GetComponent<Text>().text = score.ToString();
        sprite_renderer = GetComponent<SpriteRenderer>();
        game_controller_script = GetComponent<GameController>();
        new_pos = new Vector3(transform.position.x + 50, transform.position.y + 25, 0);
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

        if (!dead)
        {
            //transform.position = new Vector3(transform.position.x + 0.50f, transform.position.y + 0.25f, transform.position.z)
            transform.position = Vector3.Lerp(transform.position, new_pos, Time.deltaTime / 50);
        }
       
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.y < -50){
            player_body.velocity = new Vector2(0, 0);
        }
	}

    public void start_moving(string direction)
    {
        InvokeRepeating("move_player", 1f, 0.3f);
        if (direction == "right")
        {
            player_direction = player_change_pos.right;
        }
        else
        {
            player_direction = player_change_pos.left;
        }
    }

    void move_player()
    {
        
        score += 1;
        score_text.GetComponent<Text>().text = score.ToString();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "first")
        {
            player_direction = player_change_pos.right;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (player_direction == player_change_pos.right)
        {
            new_pos = new Vector3(transform.position.x + 50, transform.position.y + 25, 0);
        }
        else
        {
            new_pos = new Vector3(transform.position.x - 50, transform.position.y + 25, 0);
        }
        print("ewfew");
    }

    /*void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "left" && player_direction == player_change_pos.right ||
            other.gameObject.tag == "right" && player_direction == player_change_pos.left)
        {
            CancelInvoke();
            player_body.velocity = new Vector2(0, -7);
            sprite_renderer.sortingLayerName = "Default";
            dead = true;
            foreach (GameObject ui_objects in active_ui_objects)
            {
                ui_objects.SetActive(true);
            }
        }
    }*/
}
