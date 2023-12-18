using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GeneralController generalController;

    public List<GameObject> grids;
    public List<GameObject> shells;
    public List<GameObject> grass;

    public float positionXbound;
    public float positionZbound;

    public GameObject grassHolder;
    public GameObject shark;
    public Rigidbody rbShark;
    public GameObject chosenShell;
    public float speedShark;

    private int grassInstantiate;
    private int sharkRandom;
    private bool playerTouched;
    private void Start()
    {
        grids[Random.Range(0, grids.Count)].SetActive(true);

        if (generalController.initial == 0) // first platform empty
        {
            generalController.initial = 1;
            Destroy(shark);
        }
        else
        {
            // shell spawn
            chosenShell = Instantiate(shells[Random.Range(0, shells.Count)], transform);
            if (generalController.player.punchStudied == 0) // for tutorial
            {
                chosenShell.transform.localPosition = new Vector3(0.16f, 1, -0.06f);
                Instantiate(grass[1], grassHolder.transform);
            }
            else
            {
                chosenShell.transform.localPosition = new Vector3(Random.Range(-positionXbound, positionXbound), 1, Random.Range(-positionZbound, positionZbound));
                // grass spawn
                grassInstantiate = Random.Range(0, grass.Count);
                for (int i = 0; i < grassInstantiate; i++)
                {
                    Instantiate(grass[i], grassHolder.transform);
                    //grass[i].transform.localPosition, Quaternion.identity
                }
            }
            grassHolder.transform.localPosition = chosenShell.transform.localPosition;
        }


        // decorations

    }
    private void Update()
    {
        if(chosenShell != null && grassHolder != null && grassHolder.transform.childCount == 0 && chosenShell.layer == LayerMask.NameToLayer("Ignore Raycast"))
        {
            chosenShell.layer = LayerMask.NameToLayer("Default");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !playerTouched)
        {
            playerTouched = true;

            // shark spawn
            if(shark != null)
            {
                if (generalController.player.sharkStudied == 0) // for tutorial
                {
                    speedShark = speedShark * 1.5f;
                    shark.transform.eulerAngles = new Vector3(0, 60, 0);
                    shark.transform.localPosition = new Vector3(1f, shark.transform.localPosition.y, 0.3f);
                    rbShark.velocity = new Vector3(-1 * speedShark, 0, -1 * speedShark);
                }
                else
                {
                    if (generalController.mode >= 5 || generalController.mode == 0)
                    {
                        speedShark = speedShark * 1.5f;
                    }
                    if (generalController.mode == 2)
                    {
                        sharkRandom = 0;
                    }
                    else if (generalController.mode >= 3 && generalController.mode < 6)
                    {
                        sharkRandom = Random.Range(0, 2);
                    }
                    else
                    {
                        sharkRandom = Random.Range(0, 4);
                    }
                    switch (sharkRandom)
                    {
                        case 0:
                            shark.transform.eulerAngles = new Vector3(0, 60, 0);
                            shark.transform.localPosition = new Vector3(1.3f, shark.transform.localPosition.y, Random.Range(0.3f, 1f));
                            rbShark.velocity = new Vector3(-1 * speedShark, 0, -1 * speedShark);
                            break;
                        case 1:
                            shark.transform.eulerAngles = new Vector3(0, -60, 0);
                            shark.transform.localPosition = new Vector3(-1.3f, shark.transform.localPosition.y, Random.Range(0.3f, 1f));
                            rbShark.velocity = new Vector3(1 * speedShark, 0, -1 * speedShark);
                            break;
                        case 2:
                            shark.transform.eulerAngles = new Vector3(0, -120, 0);
                            shark.transform.localPosition = new Vector3(-1.3f, shark.transform.localPosition.y, Random.Range(-1f, -0.3f));
                            rbShark.velocity = new Vector3(1 * speedShark, 0, speedShark);
                            break;
                        default:
                            shark.transform.eulerAngles = new Vector3(0, 120, 0);
                            shark.transform.localPosition = new Vector3(1.3f, shark.transform.localPosition.y, Random.Range(-1f, -0.3f));
                            rbShark.velocity = new Vector3(-1 * speedShark, 0, speedShark);
                            break;
                    }
                }
                shark.transform.SetParent(null);
                shark.SetActive(true);


            }
            generalController.InstantiatePlatform(transform.localPosition);                      
            generalController.DestroyPlatform();
            generalController.lastPlatform = generalController.previousPlatform;
            generalController.previousPlatform = this;

        }
    }
}
