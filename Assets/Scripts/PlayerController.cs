using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private CharacterController characterController;
    private GameObject playergraphics;

    // movement
    public float speed;
    public float rotationSpeed;
    public float leftBound;
    public float rightBound;

    private Vector3 startPosition;
    private Vector3 touchPosition;
    private bool noTouches = true;
    private float horizontal;
    private float vertical;
    [HideInInspector] public float verticalMinimum;
    private Vector3 horizontalMove;

    // bonuses
    [HideInInspector] public bool swipesBlocked;
    [HideInInspector] public bool shield;
    [HideInInspector] public bool claws;
    [HideInInspector] public bool punch;

    public float pearlValue;

    // effects
    [HideInInspector] public GameObject shieldBubble;
    [HideInInspector] public GameObject playAgain;
    public GameObject winScreen;

    public List<AudioSource> sounds;
    public List<GameObject> particlePrefabs;

    private GameObject particles;
    private GameObject particlesGrass;
    private GameObject particlesPearl;

    // claw
    [HideInInspector] public LineRenderer lineRenderer;
    private Vector3 startPos;

    [HideInInspector] public float newPearls;
    [HideInInspector] public int points;

    // tutorial
    private float swipesStudied;
    [HideInInspector] public float punchStudied;
    [HideInInspector] public float sharkStudied;
    private float bonusesStudied;
    private bool sharkHere;
    private bool clawHere;
    [HideInInspector] public bool blocked;
    [HideInInspector] public Animator pointer;
    [HideInInspector] public GameObject pointerObject;
    [HideInInspector] public GameObject tutorialPart;

    private void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        characterController = transform.GetComponent<CharacterController>();
        playergraphics = transform.GetChild(0).gameObject;

        swipesStudied = PlayerPrefs.GetFloat("swipesStudied");
        punchStudied = PlayerPrefs.GetFloat("punchStudied");
        sharkStudied = PlayerPrefs.GetFloat("sharkStudied");
        bonusesStudied = PlayerPrefs.GetFloat("bonusesStudied");
        pointerObject.SetActive(false);
        
        if (swipesStudied == 0)
        {
            pointerObject.SetActive(true);
            pointer.Play("PoinerSwipe");
        }
        bonusesCheck();
    }   

    public void FixedUpdate()
    {
        MoveToTarget();
    }
    public void Update()
    {
        if (!punch && !winScreen.activeSelf)
        {
            CheckSwipeInput(); // movement

            //if (Input.GetMouseButtonDown(0))
            //{
            //    noTouches = false;
            //    startPosition = Input.mousePosition;
            //}
            //if (Input.GetMouseButton(0))
            //{
            //    touchPosition = Input.mousePosition;
            //    float xDelta = touchPosition.x - startPosition.x;
            //    float yDelta = touchPosition.y - startPosition.y;
            //    if (xDelta < yDelta && yDelta > 0)
            //    {
            //        swipeUp();
            //    }
            //    else // horisontal
            //    {
            //        if (xDelta > 0)
            //        {
            //            swiperight();

            //        }
            //        else
            //        {
            //            swipeleft();
            //        }
            //    }
            //    if (swipesBlocked)
            //    {
            //        horizontal = 0;
            //        vertical = 0;
            //    }
            //}
            //if (Input.GetMouseButtonUp(0))
            //{
            //    swipesBlocked = false;
            //    noTouches = true;
            //}

        }
        else if(punch)
        {
            CheckSwipeInput2(); // dragging

            //if (Input.GetMouseButtonDown(0))
            //{
            //    startPosition = Input.mousePosition;
            //    startPos = Camera.main.ScreenToWorldPoint(new Vector3(startPosition.x, startPosition.y, 10f));
            //    lineRenderer.SetPosition(0, startPos);
            //    lineRenderer.SetPosition(1, startPos);
            //}
            //if (Input.GetMouseButton(0))
            //{
            //    touchPosition = Input.mousePosition;
            //    Vector3 currentPos = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 10f));
            //    lineRenderer.SetPosition(1, currentPos);
            //}
            //if (Input.GetMouseButtonUp(0))
            //{
            //    punch = false;
            //    lineRenderer.gameObject.SetActive(false);

            //    Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            //    RaycastHit hitInfo;
            //    if (Physics.Raycast(ray, out hitInfo))
            //    {
            //        GameObject hitObject = hitInfo.collider.gameObject;

            //        if (hitObject.tag == "Dot")
            //        {
            //            if (particlesGrass != null)
            //            {
            //                Destroy(particlesGrass);
            //            }
            //            particlesGrass = Instantiate(particlePrefabs[1], transform.position, Quaternion.identity);
            //            particlesGrass.transform.position = hitInfo.point;
            //            if (!claws)
            //            {
            //                Destroy(hitObject);
            //            }
            //            else
            //            {
            //                GameObject otherhitObject = hitObject.transform.parent.parent.gameObject.GetComponent<Platform>().chosenShell;
            //                Destroy(hitObject.transform.parent.gameObject);
            //                otherhitObject.transform.SetParent(null);
            //                otherhitObject.GetComponent<Shell>().animator.enabled = true;
            //                sounds[2].Play();
            //                if (particlesPearl != null)
            //                {
            //                    Destroy(particlesPearl);
            //                }
            //                particlesPearl = Instantiate(particlePrefabs[2], transform.position, Quaternion.identity);
            //                particlesPearl.transform.position = hitInfo.point;
            //                newPearls += pearlValue;
            //            }
            //        }
            //        else if (hitObject.tag == "Shell")
            //        {
            //            hitObject.transform.SetParent(null);
            //            hitObject.gameObject.GetComponent<Shell>().animator.enabled = true;
            //            sounds[2].Play();
            //            if (particlesPearl != null)
            //            {
            //                Destroy(particlesPearl);
            //            }
            //            particlesPearl = Instantiate(particlePrefabs[2], transform.position, Quaternion.identity);
            //            particlesPearl.transform.position = hitInfo.point;
            //            newPearls += pearlValue;
            //            points += 1;
            //            if (clawHere && punchStudied == 0)
            //            {
            //                clawHere = false;
            //                punchStudied = 1;
            //                pointerObject.SetActive(false);
            //                PlayerPrefs.SetFloat("punchStudied", punchStudied);
            //                PlayerPrefs.Save();
            //                bonusesCheck();
            //            }
            //        }
            //    }
            //}
        }

        if (noTouches || punch)
        {
            horizontal = Mathf.Lerp(horizontal, 0, Time.deltaTime);
            vertical = Mathf.Lerp(vertical, verticalMinimum, Time.deltaTime);
        }
    }
    void CheckSwipeInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                noTouches = false;
                   startPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                touchPosition = touch.position;
                float xDelta = touchPosition.x - startPosition.x;
                float yDelta = touchPosition.y - startPosition.y;
                if (xDelta < yDelta && yDelta > 0)
                {
                    swipeUp();
                }
                else // horisontal
                {
                    if (xDelta > 0)
                    {
                        swiperight();

                    }
                    else
                    {
                        swipeleft();
                    }
                }
                if (swipesBlocked)
                {
                    horizontal = 0;
                    vertical = 0;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                swipesBlocked = false;
                noTouches = true;
            }
        }
    }
    void CheckSwipeInput2()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startPosition = Input.mousePosition;
                startPos = Camera.main.ScreenToWorldPoint(new Vector3(startPosition.x, startPosition.y, 10f));
                lineRenderer.SetPosition(0, startPos);
                lineRenderer.SetPosition(1, startPos);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                touchPosition = Input.mousePosition;
                Vector3 currentPos = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 10f));
                lineRenderer.SetPosition(1, currentPos);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                punch = false;
                lineRenderer.gameObject.SetActive(false);

                Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    GameObject hitObject = hitInfo.collider.gameObject;
                    if (hitObject.tag == "Dot")
                    {
                        if (particlesGrass != null)
                        {
                            Destroy(particlesGrass);
                        }
                        particlesGrass = Instantiate(particlePrefabs[1], transform.position, Quaternion.identity);
                        particlesGrass.transform.position = hitInfo.point;
                        if (!claws)
                        {
                            Destroy(hitObject);
                        }
                        else
                        {
                            GameObject otherhitObject = hitObject.transform.parent.parent.gameObject.GetComponent<Platform>().chosenShell;
                            Destroy(hitObject.transform.parent.gameObject);
                            otherhitObject.transform.SetParent(null);
                            otherhitObject.GetComponent<Shell>().animator.enabled = true;
                            sounds[2].Play();
                            if (particlesPearl != null)
                            {
                                Destroy(particlesPearl);
                            }
                            particlesPearl = Instantiate(particlePrefabs[2], transform.position, Quaternion.identity);
                            particlesPearl.transform.position = hitInfo.point;
                            newPearls += pearlValue;
                        }
                    }
                    else if (hitObject.tag == "Shell")
                    {
                        hitObject.transform.SetParent(null);
                        hitObject.gameObject.GetComponent<Shell>().animator.enabled = true;
                        sounds[2].Play();
                        if (particlesPearl != null)
                        {
                            Destroy(particlesPearl);
                        }
                        particlesPearl = Instantiate(particlePrefabs[2], transform.position, Quaternion.identity);
                        particlesPearl.transform.position = hitInfo.point;
                        newPearls += pearlValue;
                        points += 1;
                        if (clawHere && punchStudied == 0)
                        {
                            clawHere = false;
                            punchStudied = 1;
                            pointerObject.SetActive(false);
                            PlayerPrefs.SetFloat("punchStudied", punchStudied);
                            PlayerPrefs.Save();
                            bonusesCheck();
                        }
                    }
                }
            }
        }
    }

    void MoveToTarget()
    {
        Vector3 move = new Vector3(horizontal, 0, vertical);
        characterController.Move(move * speed * Time.deltaTime);

        // rotation
        if (horizontal == 1 || horizontal == -1)
        {
            horizontalMove = Vector3.Lerp(horizontalMove, new Vector3(horizontal, -90, 1), Time.deltaTime);
        }
        else
        {
            horizontalMove = Vector3.Lerp(horizontalMove, new Vector3(0, -90, 1), Time.deltaTime);
        }
        playergraphics.transform.forward = Vector3.Lerp(playergraphics.transform.forward, horizontalMove, rotationSpeed * Time.deltaTime);

        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, leftBound, rightBound), transform.position.y, transform.position.z);

    }
    public void swiperight()
    {
        horizontal = 1;
    }
    public void swipeleft()
    {
        horizontal = -1;
    }
    public void swipeUp()
    {
        horizontal = Mathf.Lerp(horizontal, 0, speed * Time.deltaTime);
        vertical = 1;
        swipeCheck();
    }

    public void swipeCheck()
    {
        if (swipesStudied == 0)
        {
            pointerObject.SetActive(false);
            swipesStudied = 1;
            PlayerPrefs.SetFloat("swipesStudied", swipesStudied);
            PlayerPrefs.Save();
        }
    }

    public void swipeCheckShark()
    {
        if (sharkStudied == 0 && sharkHere)
        {
            pointerObject.SetActive(false);
            sharkStudied = 1;
            PlayerPrefs.SetFloat("sharkStudied", sharkStudied);
            PlayerPrefs.Save();
            if (punchStudied == 0 && !clawHere)
            {
                clawHere = true;
                pointerObject.SetActive(true);
                pointer.Play("PointerPunch");
            }
        }
    }

    public void bonusesCheck()
    {
        if (!blocked && bonusesStudied == 0 && punchStudied == 1)
        {
            tutorialPart.SetActive(true);
            bonusesStudied = 1;
            PlayerPrefs.SetFloat("bonusesStudied", bonusesStudied);
            PlayerPrefs.Save();
        }
    }

    public void Punching()
    {
        punch = true;
    }
    public void ShieldOff()
    {
        shieldBubble.SetActive(false);
        shield = false;
    }

    public void LineOn()
    {
        startPos = Vector3.zero;
        lineRenderer.positionCount = 2;
        lineRenderer.gameObject.SetActive(true);

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Shark") && !winScreen.activeSelf && !playAgain.activeSelf)
        {
            sounds[1].Play();
            if (particles != null)
            {
                Destroy(particles);
            }
            particles = Instantiate(particlePrefabs[0], transform.position, Quaternion.identity);
            particles.transform.position = collision.contacts[0].point;

            if (!shield && !playAgain.activeSelf)
            {
                playAgain.SetActive(true);
            }
            else if(shield)
            {
                ShieldOff();
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ViewZone") && sharkStudied == 0 && !sharkHere)
        {
            sharkHere = true;
            pointerObject.SetActive(true);
            pointer.Play("PointerShark");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ViewZone"))
        {
            swipeCheckShark();
        }
    }

}
