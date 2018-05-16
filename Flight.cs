using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

/* Hrafnkell Þorri Þrastarson Dec 2017
 * Flight Mechanics for The Grand Mosquito
 * Contents
 * 1 - Initialization & Update
 * 2 - Flight Related Functions 
 * 3 - Sounds & Effects
 * 4 - Collisions & Detections
*/

public class Flight : MonoBehaviour
{
    //Components
    private Rigidbody rb;
    private AudioSource audioSource;
    public AudioClip CrashSound;
    public GameObject CamerPivot;
    public TextMeshPro Throttle_Text;
    public TextMeshPro Altitude_Text;
    public TextMeshPro Fuel_Text;

    //Public ParticleSystems;
    public ParticleSystem Afterburner;
    public ParticleSystem Crash;
    public ParticleSystem Clouds;
    public ParticleSystem NearSeaEffect;
    public ParticleSystem R_Trail;
    public ParticleSystem L_Trail;

    public Transform pilot;
    public Transform CamReset;

    //Time
    float lastTime;

    //Public Player Atributes
    public bool alive = true;
    public static bool isAlive = true;
    public bool hasPilot = false;
    public static bool hasPilotStat = false;
    public bool breaks = false;
    public float blackOutval = 0;

    public bool gearDown = true;
    public bool allowGearMovement = false;
    public float timeToGearDown = 2f;

    public float speed = 50f;
    public float Fuel = 100f;
    public float FuelEconomyperframe = 100000f;
    public float acceleration = 0.37f;
    public float breakForce = 1;
    public float throttle = 0;

    public float Altitude = 0;
    
    public float dragForce = 1;
    public float drag = 1;
    public float angle;

    public float AileronAuthority = 600.0f;
    public float ElevatorAuthority = 1500.0f;
    public float RudderAuthority = 500f;

    public GameObject player;


    //Location
    public static Vector3 playerlocation;

    //Becomes false if the plane collides with ground
    private bool afterlife = true;

    //PPstack
    public PostProcessingProfile PPStack;

    // Use this for initialization
    void Start()
    {
        R_Trail.Pause();
        L_Trail.Pause();
        Afterburner.Stop();
        Clouds.Pause();
        NearSeaEffect.Pause();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        lastTime = Time.time;

        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        hasPilotStat = hasPilot;

        isAlive = alive;

        if(hasPilot)
        {
            player.transform.parent = gameObject.transform;
            player.transform.position = CamReset.transform.position;
            player.transform.rotation = CamReset.transform.rotation;
            
            player.SetActive(false);
        }


        if (alive && hasPilot)
        {
            playerlocation = transform.position;
            //EngineSoundPitch();
            PlayWingTrails();
            Gear();
            Fuel -= (throttle / FuelEconomyperframe) * Time.deltaTime;
            Altitude = transform.position.y + 48;
            AngleOfAttack();
        }


        if (Input.GetKey(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    void FixedUpdate()
    {
        if (alive && hasPilot)
        {
            PlayAfterburner();
            InputandGravity();
            if (Fuel < 0.5f)
            {
                throttle = 0;
            }
            Throttle();

            PlayNearSeaEffect();
        }
        else if(!alive)
        {
            CrashBehavioure();
        }

    }

    void Gear()
    {
        if (Time.time - lastTime > timeToGearDown)
        {
            allowGearMovement = true;
            if (Input.GetKey(KeyCode.G) && gearDown == true)
            {
                gearDown = false;
                lastTime = Time.time;
                GetComponent<Animator>().Play("Gears");
                breakForce = 1;
                AileronAuthority = AileronAuthority + 400;
                ElevatorAuthority = ElevatorAuthority + 300;
            }
            else if (Input.GetKey(KeyCode.G) && gearDown == false)
            {
                gearDown = true;
                lastTime = Time.time;
                GetComponent<Animator>().Play("Gears_Reverse");
                breakForce = 1.5f;
                AileronAuthority = AileronAuthority - 400;
                ElevatorAuthority = ElevatorAuthority - 300;
            }
        }
        else
        {
            allowGearMovement = false;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            breaks = true;
        }
        else if (Input.GetKeyUp(KeyCode.B))
        {
            breaks = false;
        }


    }

    void Throttle()
    {
        //Eventar
        if (Input.GetKey(KeyCode.LeftShift))
        {
            IncreaseSpeed();
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            DecreaseSpeed();
        }

        SteadySpeed();
    }

    //Control related functions
    void InputandGravity()
    {
        //rb.AddForce(Vector3.down * 150);

        //rb.AddRelativeForce(Vector3.right * -throttle  , ForceMode.Force);

        rb.transform.position -= transform.right * Time.deltaTime * speed;

        float x = Input.GetAxis("Vertical");
        float y = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Rudder");

        rb.AddRelativeTorque(y * AileronAuthority, x * -ElevatorAuthority, z * RudderAuthority);

        if (blackOutval < 1 && Input.GetKey(KeyCode.UpArrow) && speed > 230 || blackOutval < 100 && Input.GetKey(KeyCode.DownArrow) && speed > 230)
        {
            blackOutval += 0.0013f;
        }
        else if (blackOutval > 0.02f)
        {
            blackOutval -= 0.0095f;
        }

        

        //PP BlackoutEffect
        VignetteModel.Settings g = PPStack.vignette.settings;
        g.intensity = blackOutval;
        PPStack.vignette.settings = g;


        if(Input.GetKeyDown(KeyCode.S))
        {
            speed = 400f;
        }

        //Stop's rotational movement after an Key/trigger is released
        rb.angularVelocity = Vector3.zero;


    }


    //Speed related functions
    void DecreaseSpeed()
    {
        if (throttle > 0)
        {
            throttle -= 15f * Time.deltaTime; 
        }
        else
        {
            throttle = 0;
        }
    }

    void IncreaseSpeed()
    {
        if (throttle < 100)
        {
            throttle += 15 * Time.deltaTime;
        }
        else
        {
            throttle = 100;
        }
    }

    void AngleOfAttack()
    {
        //Vector3 AoA = transform.position;
        //angle = Vector3.Angle(Vector3.up, transform.forward) ;

        Vector3 myVec   = transform.forward; //aircraft forward vector
        Vector3 worldUp = new Vector3(0.0f, 1.0f, 0.0f); //world UP vector
        float Angle = Mathf.Asin(Vector3.Dot(myVec, worldUp)); //angle from horizon in radians
        Angle = Angle * Mathf.Rad2Deg; //angle in degrees
        angle = Angle;
    }

    void SteadySpeed()
    {
        speed -= dragForce * breakForce * Time.deltaTime;
        speed = speed + 0.2f * Mathf.Abs(throttle) * Time.deltaTime;

        if (throttle < 10 || speed < 50 && gearDown == false)
        {
            rb.AddForce(Vector3.down * 75);
            if (gearDown == false && speed < 120)
            {
                rb.AddRelativeTorque(750, 0, 750);
                rb.AddForce(Vector3.down * 1700);
            }
            if (gearDown == false && speed < 60)
            {
                rb.AddRelativeTorque(1500, 0, 1500);
                rb.AddForce(Vector3.down * 2300);
            }
        }
        else
        {
            if (speed < 75 && speed >= 0)
            {
                dragForce = 3;
            }

            else if (speed > 75 && speed < 150)
            {
                dragForce = 6;
                rb.drag = 1f;
            }
            else if (speed > 150 && speed < 300)
            {
                dragForce = 7;
                rb.drag = 5f;
            }
            else if (speed > 700 && speed < 950)
            {
                dragForce = 13;
            }
            else if (speed > 950)
            {
                dragForce = 19.7f;
            } 
        }
    }

    //Texts
    private void OnGUI()
    {
        if (!Throttle_Text){}
        else
        {
            Throttle_Text.GetComponent<TextMeshPro>().text = throttle.ToString("F0") + "%";
        }

        if (!Fuel_Text){}
        else
        {
            Fuel_Text.GetComponent<TextMeshPro>().text = Fuel.ToString("F0") + "%";
        }

        if(!Altitude_Text){}
        else
        {
            Altitude_Text.GetComponent<TextMeshPro>().text = (Altitude / 4).ToString("F0") + (" Meters");
        }


    }

    //Crash
    private void CrashBehavioure()
    {
        rb.AddForce(Vector3.down * -1);
        rb.transform.position -= transform.right * Time.deltaTime * speed;
        rb.AddRelativeTorque(0, 0, RudderAuthority / 2);
        rb.AddRelativeTorque(Random.Range(0,100), Random.Range(0, 100), Random.Range(0, 100));
        if (speed < 20)
        {
            speed = 0;
        }
        else
        {
            speed -= 0.1f;
        }
    }

    //Sounds
    /*void EngineSoundPitch()
    {
        if (throttle > 60 && throttle < 70)
        {
            audioSource.pitch = 0.45f;
        }
        else if (throttle > 70 && throttle < 80)
        {
            audioSource.pitch = 0.50f;
        }
        else if (throttle > 80 && throttle < 90)
        {
            audioSource.pitch = 0.525f;
        }
        else if (throttle > 90 && throttle < 100)
        {
            audioSource.pitch = 0.55f;
        }
    }*/

    void PlayAfterburner()
    {
        if (throttle < 75 && Afterburner.isPlaying)
        {
            Afterburner.Stop();
            //speed -= 10;
        }
        else if (throttle > 75 && Afterburner.isStopped)
        {
            Afterburner.Play();
            speed += 10;
        }
    }

    void PlayWingTrails()
    {

        if (speed > 100)
        {
            R_Trail.Play();
            L_Trail.Play();
        }
        else
        {
            R_Trail.Pause();
            L_Trail.Pause();
        }
    }

    void PlayNearSeaEffect()
    {
        if (gameObject.transform.position.y < 30 && gameObject.transform.position.y > -40)
        {
            NearSeaEffect.Play();
        }
        else
        {
            NearSeaEffect.Pause();
        }
    }

    //Collisions
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player");
        }
        else if (collision.gameObject.tag != "Runway"  || gearDown != true && (Time.time - lastTime > 2.0f))
        {
            alive = false;
            Crash.Play();
            Afterburner.Pause();
            Destroy(CamerPivot);
            lastTime = Time.time;

            if (speed > 10)
            {
                speed = speed - 0.2f;
                rb.AddForce(Vector3.up * 11150);
            }
            if (afterlife == true)
            {
                audioSource.PlayOneShot(CrashSound);
                afterlife = false;
                Destroy(gameObject, 10);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cloud") {}

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Cloud")
        {
            Clouds.Pause();
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.F) && hasPilot == false)
        {
            Debug.Log("Player Entering");
            pilot = other.transform;
            hasPilot = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (hasPilot)
        {
            if (Fuel < 101f)
            {
                Fuel += 1 * Time.deltaTime;
            }
            if (breaks == true && speed > 0)
            {
                breakForce = 4;
            }
            else
            {
                breakForce = 1.5f;
            }

            if (collision.gameObject.tag == "Runway")
            {

                if (speed < 0)
                {
                    speed = 0;
                }
            }
        }
    }
}
