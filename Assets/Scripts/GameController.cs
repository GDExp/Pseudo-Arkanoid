using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController init;
    private BricksType bricks_type;
    private UIcontroller ui_controller;
    private Transform bricks_pool;
    private Transform game_field;
    
    private AssetBundle current_bundle;

    private int player_scores;
    public float step_time;
    private bool game_run;

    private void Awake()
    {
        init = this;   
    }

    private void Start()
    {
        Setup();
        LoadBundles();
        Invoke("StartGame", 0.1f);//late start
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)//bricks or ball = GameOver
            GameOver();
    }

    private void Setup()
    {
        bricks_pool = transform.Find("Bricks");
        game_field = GameObject.Find("GameField").transform;

        ui_controller = FindObjectOfType<UIcontroller>();
        PlayerPrefs.SetInt("Score", 0);
    }

    private void LoadBundles()
    {
        current_bundle = AssetBundle.LoadFromFile(System.IO.Path.Combine(Application.streamingAssetsPath, "gameassets"));
        //type brick select in main menu
        print(PlayerPrefs.GetString("Type"));
        bricks_type = current_bundle.LoadAsset(PlayerPrefs.GetString("Type")) as BricksType;
    }

    public void UnloadBundles()
    {
        current_bundle.Unload(true);
    }

#region GAME CONTROLLER

    public void StartGame()
    {
        //OMG! if the player won :)
        if (PlayerPrefs.GetInt("Score") != 0)
            player_scores = PlayerPrefs.GetInt("Score");
        StopAllCoroutines();
        ui_controller.RefreshTimeUI(step_time);
        ui_controller.RefreshScoreUI(player_scores);
        game_field.localPosition = Vector3.zero;
        GenerateNewField();
    }

    private void GameOver()
    {
        game_run = false;
        ui_controller.OpenPanel(UIcontroller.MenuType.Lose);
        player_scores = 0;
        PlayerPrefs.SetInt("Score", player_scores);
    }

    private void GenerateNewField()
    {
        GameObject brick;
        int row = 4;//by grid
        int column = 9;//by grid
        Vector2 start_position = new Vector2(-7f, 4.55f);//by grid;
        float x_random = 0f;
        while(column > 0)
        {
            int count = 0;
            while(count < row)
            {
                x_random = Random.Range(-1f, 1f);
                count++;
                if (x_random > 0f)
                {
                    brick = GetObjInPool();
                    brick.transform.SetParent(game_field);
                    brick.transform.localPosition = start_position;
                    start_position += Vector2.down * 0.7f;
                }
            }
            start_position += Vector2.right * 1.75f;
            start_position.y = 4.55f;
            column--;
        }

        //start game
        game_run = true;
        StartCoroutine(BriksStepDown());
    }

#endregion

#region POOL

    public GameObject GetObjInPool()
    {
        GameObject obj = null;

        if (bricks_pool.childCount > 0)
            obj = bricks_pool.GetChild(0).gameObject;
        else
            CreateNewObject(out obj);
        SetupBrick(obj);

        return obj;
    }
    //reuse bricks
    public void ContainAllBricks()
    {
        while(game_field.childCount > 0)
            ReturnObjInPool(game_field.GetChild(0).gameObject);
    }

    private void ReturnObjInPool(GameObject obj)
    {
        obj.transform.SetParent(bricks_pool);
        obj.transform.localPosition = Vector3.zero;
        obj.SetActive(false);
    }

    private void CreateNewObject(out GameObject new_obj)
    {
        new_obj = Instantiate(bricks_type.brick_prefab, game_field);
    }

#endregion

#region BRICK
    //set color + text
    private void SetupBrick(GameObject brick)
    {
        SpriteRenderer b_sprite = brick.GetComponentInChildren<SpriteRenderer>();
        TextMeshPro text_mesh = brick.GetComponentInChildren<TextMeshPro>();
        text_mesh.faceColor = Color.black;

        int random_index = Random.Range(0, bricks_type.color_type.Length);
        b_sprite.color = bricks_type.color_type[random_index];
        text_mesh.text = bricks_type.hit_type[random_index].ToString();

        brick.SetActive(true);
    }
    
    public void BrickHit(GameObject hit_brick)
    {
        SpriteRenderer b_sprite = hit_brick.GetComponentInChildren<SpriteRenderer>();
        TextMeshPro text_mesh = hit_brick.GetComponentInChildren<TextMeshPro>();
        int value = System.Convert.ToInt32(text_mesh.text) - 1;
        text_mesh.text = value.ToString();
        switch (value)
        {
            case (0):
                ReturnObjInPool(hit_brick);
                ui_controller.RefreshScoreUI(++player_scores);
                if (game_field.childCount == 0)
                    ui_controller.OpenPanel(UIcontroller.MenuType.Win);
                break;
            case (1):
                b_sprite.color = bricks_type.color_type[0];
                break;
            case (2):
                b_sprite.color = bricks_type.color_type[1];
                break;
            case (4):
                b_sprite.color = bricks_type.color_type[2];
                break;
            default:
                break;
        }
    }
    //1 step down by grid
    IEnumerator BriksStepDown()
    {
        float time = step_time;
        yield return new WaitForSeconds(0.3f);//late start
        while (game_run)
        {
            if(Time.timeScale == 1)
            {
                if (time > 0) time -= Time.deltaTime;
                else
                {
                    yield return ClampMove(game_field.localPosition + Vector3.down * 1f);
                    time = step_time;
                }
                ui_controller.RefreshTimeUI(time);
            }
            yield return new WaitForFixedUpdate();
        }
        print("GameOver");
    }
   
    IEnumerator ClampMove(Vector3 value)
    {
        while (game_field.localPosition.y > value.y)
        {
            game_field.localPosition = Vector3.Lerp(game_field.localPosition, value, 0.15f);
            if (Mathf.Abs(game_field.localPosition.y - value.y) < 0.075f)
                game_field.localPosition = value;
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }

#endregion

}
