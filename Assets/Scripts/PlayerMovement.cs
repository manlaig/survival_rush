using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float smoothnessRotate = 15f; /*speed = 1f, lerpSpeed = 20f;*/

    GameManager gameController;
	WallColliderPosition walls;
	Camera cam;
	Rigidbody rb;

    Vector3 prev;

	void Start ()
	{
		walls = GameObject.FindGameObjectWithTag ("Walls").GetComponent<WallColliderPosition> ();
        gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager>();
		rb = GetComponent<Rigidbody> ();
		cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
		transform.position = new Vector3(cam.ScreenToWorldPoint(new Vector3(Screen.width / 2f, 0f, 0f)).x, 0f, cam.ScreenToWorldPoint(new Vector3(0f, Screen.height / 2f, 0f)).z);
	}

	void FixedUpdate ()
	{
        if (gameController.isGameStarted())
        {
            float x, y;

            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
            {
                x = Input.acceleration.x;
                y = Input.acceleration.y;
                limitBoundaries(ref x, ref y);
                MoveAndTurn(x, y, getMultiplier(x, y));
            }
            else
            {
                x = Input.GetAxis("Horizontal");
                y = Input.GetAxis("Vertical");
                //x = Input.acceleration.x;   // when testing
                //y = Input.acceleration.y;   // when testing

                limitBoundaries(ref x, ref y);
                MoveAndTurn(x, y, 5);
                //MoveAndTurn(x, y, getMultiplier(x, y));   // when testing on computer
            }
        }
	}

	void limitBoundaries(ref float x, ref float y)
    {
		float topEdge = walls.top.position.z;
		float bottomEdge = walls.bottom.position.z;
		float leftEdge = walls.left.position.x;
		float rightEdge = walls.right.position.x;

        if (transform.position.z - 0.6f <= bottomEdge && y < 0f)
			y = 0f;
        if (transform.position.z + 0.6f >= topEdge && y > 0f)
			y = 0f;
        if (transform.position.x + 0.6f >= rightEdge && x > 0f)
			x = 0f;
        if (transform.position.x - 0.6f <= leftEdge && x < 0f)
			x = 0f;
	}

	void MoveAndTurn(float x, float y, float mult)
	{
        rb.velocity = (new Vector3(x, 0f, y)).normalized * mult * 2f;
        if(x != 0f || y != 0f)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, Mathf.Atan2(x, y) * Mathf.Rad2Deg, 0f), Time.deltaTime * smoothnessRotate);
	}

    float getMultiplier(float x, float y)
    {
        float mult = 1f;
        if (Mathf.Abs(x) > 0.1f && Mathf.Abs(x) < 0.2f || Mathf.Abs(y) > 0.1f && Mathf.Abs(y) < 0.2f)
            mult = 4f;
        else if (Mathf.Abs(x) >= 0.2f && Mathf.Abs(x) < 0.3f || Mathf.Abs(y) >= 0.2f && Mathf.Abs(y) < 0.3f)
            mult = 5f;
        else if (Mathf.Abs(x) >= 0.3f && Mathf.Abs(x) <= 0.4f || Mathf.Abs(y) >= 0.3f && Mathf.Abs(y) <= 0.4f)
            mult = 6f;
        else if (Mathf.Abs(x) > 0.4f && Mathf.Abs(x) <= 0.5f || Mathf.Abs(y) > 0.4f && Mathf.Abs(y) <= 0.5f)
            mult = 7f;
        else if (Mathf.Abs(x) > 0.5f && Mathf.Abs(x) <= 0.6f || Mathf.Abs(y) > 0.5f && Mathf.Abs(y) <= 0.6f)
            mult = 8f;
        else if (Mathf.Abs(x) > 0.6f && Mathf.Abs(x) < 0.7f || Mathf.Abs(y) > 0.6f && Mathf.Abs(y) < 0.7f)
            mult = 9f;
        else if (Mathf.Abs(x) >= 0.7f && Mathf.Abs(x) < 0.8f || Mathf.Abs(y) >= 0.7f && Mathf.Abs(y) < 0.8f)
            mult = 10f;
        else if (Mathf.Abs(x) >= 0.8f && Mathf.Abs(x) < 0.9f || Mathf.Abs(y) >= 0.8f && Mathf.Abs(y) < 0.9f)
            mult = 11f;
        else if (Mathf.Abs(x) >= 0.9f || Mathf.Abs(y) >= 0.9f)
            mult = 12f;
       
        return mult;
    }
}
