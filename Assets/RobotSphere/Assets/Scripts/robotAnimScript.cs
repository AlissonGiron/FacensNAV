using UnityEngine;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using TouchControlsKit;
using UnityEngine.UI;

public class robotAnimScript : MonoBehaviour {

    public float Speed = 5.0f;
    public Text ScoreText;

    public float RotationSpeed = 240.0f;

    public ProgressBar bar;

    private float Gravity = 20.0f;

    private Vector3 _moveDir = Vector3.zero;

    public int Score = 0;
    public Text score;
    public GameObject menu;

    public GameObject bullet;

    //Vector3 rot = Vector3.zero;
    //float rotSpeed = 300f;
    Animator anim;
    //   float speed = 0;

    //// Use this for initialization
    //void Awake () {
    //	anim = gameObject.GetComponent<Animator> ();
    //	gameObject.transform.eulerAngles = rot;
    //}

    //// Update is called once per frame
    //void Update () {
    //	CheckKey ();
    //	gameObject.transform.eulerAngles = rot;
    //       transform.Translate(0, 0, speed * Time.deltaTime, Space.Self);
    //}

    //   void CheckKey(){
    //	// Walk
    //	if (Input.GetKey (KeyCode.W)) {
    //		anim.SetBool ("Walk_Anim", true);
    //           speed = 5f;
    //       }
    //       else if (Input.GetKeyUp (KeyCode.W)) {
    //		anim.SetBool ("Walk_Anim", false);
    //           speed = 0;
    //	}
    //       else if (Input.GetKey(KeyCode.S))
    //       {
    //           anim.SetBool("Walk_Anim", true);
    //           speed = -5f;
    //       }
    //       else if (Input.GetKeyUp(KeyCode.S))
    //       {
    //           anim.SetBool("Walk_Anim", false);
    //           speed = 0;
    //       }

    //       // Rotate Left
    //       if (Input.GetKey(KeyCode.A)){
    //		rot[1] -= rotSpeed * Time.fixedDeltaTime;
    //	}

    //	// Rotate Right
    //	if(Input.GetKey(KeyCode.D)){
    //		rot[1] += rotSpeed * Time.fixedDeltaTime;
    //	}

    //	// Roll
    //	if (Input.GetKeyDown (KeyCode.Space)) {
    //		if (speed == 10f) {
    //			anim.SetBool ("Roll_Anim", false);
    //               speed = 0;
    //           }
    //		else {
    //			anim.SetBool ("Roll_Anim", true);

    //               if(speed == 0)
    //               {
    //                   StartCoroutine("StartRoll");
    //               }
    //		}
    //	} 

    //	// Close
    //	if(Input.GetKeyDown(KeyCode.LeftControl)){
    //		if (!anim.GetBool ("Open_Anim")) {
    //			anim.SetBool ("Open_Anim", true);
    //		} 
    //		else {
    //			anim.SetBool ("Open_Anim", false);
    //		}
    //	}
    //}

    //   private IEnumerator StartRoll()
    //   {
    //       yield return new WaitForSeconds(2.5f);

    //       speed = 10f;
    //   }

    Transform myTransform;
    CharacterController controller;
    bool canJump, jumped, canShoot;
    float jump = 0;

    void Awake()
    {
        anim = gameObject.GetComponent<Animator>(); 
        jumped = false;
        canJump = true;
        canShoot = true;
        myTransform = transform;
        controller = GetComponent<CharacterController>();
        Score = 0;
        ScoreText.text = string.Format("{0:000}", Score);
    }

    void FixedUpdate()
    {
        if (!bar.init)
        {
            bar.BarValue = (100);
            bar.init = true;
        }

        if (bar.init && bar.BarValue <= 0)
        {
            GameOver();
            return;
        }

        Vector2 move = TCKInput.GetAxis("Joystick0");
        bool grounded = Physics.Raycast(transform.position, Vector3.down, 2f);

        if (grounded && TCKInput.GetAction("Button0", EActionEvent.Click))
        {
            if(jump == 0)
            {
                jump = 200;
            }
        }

        if (TCKInput.GetAction("Button1", EActionEvent.Click) && canShoot)
        {
            StartCoroutine("ShootDelay");

            var position = transform.position;
            position.y += 1f;

            var forward = transform.forward * 5000;
            forward.y = 1000;

            GameObject b = Instantiate(bullet, position, transform.rotation);
            b.GetComponent<Explosion>().Parent = transform;
            b.GetComponent<Rigidbody>().AddForce(forward);
        }

        PlayerMovement(move.x, move.y);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Fire")
        {
            bar.BarValue -= 0.3f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Void")
        {
            GameOver();
        }

        if (other.tag == "Coin")
        {
            Score++;
            ScoreText.text = string.Format("{0:000}", Score);
            Destroy(other.gameObject);
        }
    }

    private void PlayerMovement(float horizontal, float vertical)
    {
        Vector3 camForward_Dir = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 move = vertical * camForward_Dir + horizontal * Camera.main.transform.right;

        if (move.magnitude > 1f) move.Normalize();

        move = transform.InverseTransformDirection(move);

        float turnAmount = Mathf.Atan2(move.x, move.z);

        transform.Rotate(0, turnAmount * RotationSpeed * Time.deltaTime, 0);

        anim.SetBool("Walk_Anim", true);
        _moveDir = transform.forward * move.magnitude;

        _moveDir *= Speed;
        
        if(_moveDir == Vector3.zero)
        {
            anim.SetBool("Walk_Anim", false);
        }

        if (jump > 0)
        {
            jump -= 5;
            _moveDir.y += 5f;
        }
        else
        {
            _moveDir.y -= Gravity * Time.deltaTime;
        }

        controller.Move(_moveDir * Time.deltaTime);


        //controller.Move(-moveDirection * Time.fixedDeltaTime);

        //// Calculate the rotation for the player
        //moveDirection = transform.InverseTransformDirection(moveDirection);

        //// Get Euler angles
        //float turnAmount = Mathf.Atan2(moveDirection.x, moveDirection.z);

        //transform.Rotate(0, turnAmount * 10f * Time.deltaTime, 0);
    }

    private IEnumerator EndJump()
    {
        yield return new WaitForSeconds(10);
        canJump = true;
        jumped = false;
    }

    private IEnumerator ShootDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }

    public void GameOver()
    {
        Time.timeScale = 0;

        score.text = Score.ToString() + " moedas";
        menu.SetActive(true);
    }
}
