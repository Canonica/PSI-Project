using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Fish : MonoBehaviour {
    [Header("Stats")]
    public int maxLife;
    private int currentLife;
    public int moneyToGive;
    
    public Rigidbody rb;


    [Header("Movement")]
    public float minSpeed;
    public float maxSpeed;
    public bool isRightDirection;
    public bool isChangingDirection;
    public bool beenCaught;
    private float speed;
    private Boundaries boundaries;

    public GameObject moneyTextPrefab;

    bool checkingMass;
    PlayerHandler playerHandler;
	// Use this for initialization
	void Start () {
        speed = Random.Range(minSpeed, maxSpeed);
        rb = GetComponent<Rigidbody>();
        isRightDirection = true;
        boundaries = GetComponent<Boundaries>();
        playerHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHandler>();
        currentLife = maxLife;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!beenCaught)
        {
            if (isRightDirection)
            {
                transform.Translate(new Vector3(1, 0, 0) * speed * Time.deltaTime, Space.Self);
            }
            else
            {
                transform.Translate(new Vector3(-1, 0, 0) * speed * Time.deltaTime, Space.Self);
            }
        }

        if (boundaries.isOnBoundaries && !isChangingDirection && !beenCaught)
        {
            StartCoroutine(ChangeDirection());
        }

        if (ContextManager.instance.CompareContext(ContextManager.GameContext.Shoot))
        {
            transform.Rotate(Vector3.forward * Random.Range(100, 1000) *  Time.deltaTime);
            if (beenCaught && !checkingMass)
            {
                //StartCoroutine("EnableMass");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(ContextManager.instance.CompareContext(ContextManager.GameContext.Diving) &&  other.tag == "Player")
        {
            other.GetComponent<PlayerHandler>().lifeScript.SubstractLife(this,  1);
        }

        if(ContextManager.instance.CompareContext(ContextManager.GameContext.Ascent) && other.tag == "Player")
        {
            GetCaught(other.transform);
        }
    }

    public void GetCaught(Transform player)
    {
        beenCaught = true;
        transform.DORotate(new Vector3(0, 0, 90), 0.2f);
        rb.isKinematic = false;

        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        transform.parent = player.GetChild(1).transform;
        transform.position = transform.parent.position;
        playerHandler.fishCaughtList.Add(this);
    }

    IEnumerator ChangeDirection()
    {
        isChangingDirection = true;
        isRightDirection = !isRightDirection;
        if (!isRightDirection)
        {
            transform.GetChild(0).DORotate(new Vector3(0, 180, 0), 0);
        }
        else
        {
            transform.GetChild(0).DORotate(new Vector3(0, 0, 0), 0);
        }
        yield return new WaitForSeconds(0.5f);
        isChangingDirection = false;
    }


    #region Damage & Death
    public void TakeDamage(int amount)
    {
        currentLife -= amount;
        currentLife = Mathf.Max(0, currentLife);
        if (currentLife <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        VFXManager.instance.PlayVFX(VFXManager.instance.GetVFX(VFXNames.VFX_Combat_Hit), transform.position);
        SpawnTextPrefab();
        playerHandler.fishCaughtList.Remove(this);
        playerHandler.moneyScript.AddMoney(moneyToGive);
        Destroy(gameObject);
        
    }

    void SpawnTextPrefab()
    {
        GameObject tempMoneyText = Instantiate(moneyTextPrefab, new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity);
        tempMoneyText.GetComponent<TextMeshPro>().text = "<color=#04B505FF>$</color>" + (moneyToGive + playerHandler.moneyScript.bonusMoney);
        moneyTextPrefab.transform.DOMoveX(1f, 1f).OnComplete(() => Destroy(tempMoneyText));
    }
    #endregion

    IEnumerator EnableMass()
    {
        checkingMass = true;
        yield return new WaitForSeconds(0.2f);
        while(rb.velocity.sqrMagnitude > 55)
        {
            rb.drag = 50;
        }
        checkingMass = false;
    }

}
