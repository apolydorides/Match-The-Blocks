using UnityEngine;
using TMPro;
using System.Collections;

public class GameHandler : MonoBehaviour
{
    public TMP_Text Score;
    public int sucessTries = 0;
    public int totalTries = 0;
    public Camera cam;
    public bool spawner;
    public int state;
    public GameObject rotationBlock;
    public GameObject cardinalBlock;
    public GameObject activeBlock;
    SpriteRenderer cardinalSprite;
    SpriteRenderer rotationSprite;
    Vector3 startingPosition;
    Vector3 finalPosition = new Vector3( 0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        cardinalSprite = cardinalBlock.GetComponent<SpriteRenderer>();
        rotationSprite = rotationBlock.GetComponent<SpriteRenderer>();
        // gets the x and y dimensions of the arrow boxes
        float blockWidth = cardinalSprite.bounds.size.x;
        float blockHeight = cardinalSprite.bounds.size.y;
        // gets the x and y dimensions of the camera view
        cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        // sets starting position so that whole arrow block is out of the camera's view
        startingPosition = new Vector3( (width/2) + (blockWidth/2), 0, 0);
        rotationBlock.transform.position = startingPosition;
        cardinalBlock.transform.position = startingPosition;
        activeBlock = cardinalBlock;
        // hide both arrow blocks
        cardinalSprite.enabled = false;
        rotationSprite.enabled = false;

        activeBlock.transform.eulerAngles = Vector3.forward * 0;
        activeBlock.GetComponent<SpriteRenderer>().flipY = false;
        activeBlock.GetComponent<SpriteRenderer>().enabled = false;
        spawner = true;
        // Subscribing functions to event system
        Events.current.onMoveToCenter += StartMotion;
        Events.current.onGenerateArrow += RandomGenerator;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawner)
        {
            BlockReset();
            spawner = false;
            Events.current.GenerateArrow();
            Events.current.MoveToCenter();
        }
    }

    void BlockReset()
    {
        activeBlock.transform.eulerAngles = Vector3.forward * 0;
        activeBlock.transform.position = startingPosition;
        activeBlock.GetComponent<SpriteRenderer>().flipY = false;
        activeBlock.GetComponent<SpriteRenderer>().enabled = false;
        activeBlock.GetComponent<SpriteRenderer>().color = Color.white;
        state = 0;
    }

    void RandomGenerator()
    {
        int rand = Random.Range(1,7);
        state = rand;
        switch (rand)
        {
            case 1: // arrow up
                cardinalSprite.enabled = true;
                cardinalBlock.transform.eulerAngles = Vector3.forward * 90;
                activeBlock = cardinalBlock;
                break;
            case 2: // arrow down
                cardinalBlock.transform.eulerAngles = Vector3.forward * -90;
                cardinalSprite.enabled = true;
                activeBlock = cardinalBlock;
                break;
            case 3: // arrow right
                cardinalSprite.enabled = true;
                activeBlock = cardinalBlock;
                break;
            case 4: // arrow left
                cardinalSprite.enabled = true;
                cardinalBlock.transform.eulerAngles = Vector3.forward * -180;
                activeBlock = cardinalBlock;
                break;
            case 5: // arrow clockwise
                rotationSprite.enabled = true;
                activeBlock = rotationBlock;
                break;
            case 6: // arrow anti-clockwise
                rotationSprite.enabled = true;
                rotationSprite.flipY = true;
                activeBlock = rotationBlock;
                break;
            default:
                break;
        }
    }

    void StartMotion()
    {
        StartCoroutine("MoveToCenter");
    }

    IEnumerator MoveToCenter()
    {
        activeBlock.GetComponent<SpriteRenderer>().color = new Color( 1, 1, 1, 0.3f);
        while (activeBlock.transform.position.x > 0.01)
        {
            activeBlock.transform.position -= new Vector3((3f * Time.deltaTime), 0, 0);
            yield return new WaitForFixedUpdate();
        }
        activeBlock.transform.position = new Vector3( 0, 0, 0);
        activeBlock.GetComponent<SpriteRenderer>().color = new Color( 1, 1, 1, 1);
        InputManager.current.inputState = 0;
        StartInputListener(5f);
        yield return null;
    }

    void StartInputListener(float timer)
    {
        IEnumerator coroutine = Timer(timer);
        StartCoroutine(coroutine);
    }

    IEnumerator Timer(float timer)
    {
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (InputManager.current.inputState == state)
            {
                activeBlock.GetComponent<SpriteRenderer>().color = Color.green;
                timer = 0f;
                sucessTries++;
            }
            yield return new WaitForEndOfFrame();
        }
        if (activeBlock.GetComponent<SpriteRenderer>().color != Color.green)
        {
            activeBlock.GetComponent<SpriteRenderer>().color = Color.red;
        }
        totalTries++;
        // fading to transparent for a second
        while (timer < 1f)
        {
             Color tmp = activeBlock.GetComponent<SpriteRenderer>().color;
             tmp.a = 1 - timer;
             activeBlock.GetComponent<SpriteRenderer>().color = tmp;
             timer += Time.deltaTime;
             yield return new WaitForEndOfFrame();
        }
        Score.text = sucessTries + "/" + totalTries;
        spawner = true;
    }

    void OnDestroy()
    {
        Events.current.onMoveToCenter -= StartMotion;
        Events.current.onGenerateArrow -= RandomGenerator;
    }

}