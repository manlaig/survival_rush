using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    GameManager gameController;
    WallColliderPosition walls;

    void Start()
    {
        walls = GameObject.FindGameObjectWithTag("Walls").GetComponent<WallColliderPosition>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        Camera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        transform.position = new Vector3(cam.ScreenToWorldPoint(new Vector3(Screen.width / 2f, 0f, 0f)).x,
                                         0f, cam.ScreenToWorldPoint(new Vector3(0f, Screen.height / 2f, 0f)).z);
    }

    void FixedUpdate()
    {
        if (gameController.isGameStarted())
        {
            float x, y;

            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
            {
                x = Input.acceleration.x;

                if (PlayerPrefs.GetInt("deviceOrientation", 1) == (int)Orientations.FLAT)
                    y = Input.acceleration.y;
                else
                    y = Input.acceleration.y + 0.5f;

                limitBoundaries(ref x, ref y);
                MoveAndTurn(x, y, getMultiplier(x, y));
            }
            else
            {
                x = Input.acceleration.x;
                if (PlayerPrefs.GetInt("deviceOrientation", 1) == (int)Orientations.FLAT)
                    y = Input.acceleration.y;
                else
                    y = Input.acceleration.y + 0.5f;

                x = Input.GetAxis("Horizontal");
                y = Input.GetAxis("Vertical");

                limitBoundaries(ref x, ref y);
                MoveAndTurn(x, y, 10);
                //MoveAndTurn(x, y, getMultiplier(x, y));   // when testing on computer
            }
        }
    }

    void MoveAndTurn(float x, float y, float mult)
    {
        //GetComponent<Rigidbody>().velocity = (new Vector3(x, 0f, y)).normalized * mult * 2f;
        //transform.position += (new Vector3(x, 0f, y)).normalized * mult * 2f * Time.deltaTime;
        Vector3 vel = GetComponent<Rigidbody>().velocity;
        GetComponent<Rigidbody>().velocity = Vector3.Lerp(vel, (new Vector3(x, 0f, y).normalized) * mult, 0.5f);

        if (Mathf.Abs(x) > 0.05f || Mathf.Abs(y) > 0.05f)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, Mathf.Atan2(x, y) * Mathf.Rad2Deg, 0f), 0.5f);
    }

    float getMultiplier(float x, float y)
    {
        float sum = Mathf.Abs(x) + Mathf.Abs(y);
        float mult = Mathf.Clamp(sum * 27, 0f, 25f); // consider changing these values later, it was 25 and 15


        return mult;
    }

    void limitBoundaries(ref float x, ref float y)
    {
        float topEdge = walls.top.position.z;
        float bottomEdge = walls.bottom.position.z;
        float leftEdge = walls.left.position.x;
        float rightEdge = walls.right.position.x;

        if (transform.position.z - 1f <= bottomEdge && y < 0f)
            y = 0f;
        if (transform.position.z + 1f >= topEdge && y > 0f)
            y = 0f;
        if (transform.position.x + 1f >= rightEdge && x > 0f)
            x = 0f;
        if (transform.position.x - 1f <= leftEdge && x < 0f)
            x = 0f;
    }
}
