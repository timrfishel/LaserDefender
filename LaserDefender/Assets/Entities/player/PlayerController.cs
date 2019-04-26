using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 10.0f;
    public float padding = .5f;
    public GameObject projectile;
    public float projectileSpeed;
    public float firingRate = 0.3f;
    public float health = 1000f;

    public AudioClip fireSound;
    public AudioClip hitSound;

    float xmin;
    float xmax;
    float ymin;
    float ymax;

    private void Start()
    {
        float distance = transform.position.z - Camera.main.transform.position.z;

        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        Vector3 upmost = Camera.main.ViewportToWorldPoint(new Vector3(0, .5f, distance));
        Vector3 downmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));

        xmin = leftmost.x + padding;
        xmax = rightmost.x - padding;
        ymin = downmost.y + padding;
        ymax = upmost.y - padding;

    }
    void Update () {

        if (Input.GetKeyDown(KeyCode.Space)) {
            InvokeRepeating("Fire", 0.0000001f, firingRate);
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            CancelInvoke("Fire");
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //Time.deltaTime allows us to move and update the ships movement independent of the framerate of the game
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow)){
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            transform.position += Vector3.down * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
        //Restrict player ship to gamespace
        float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
        float newY = Mathf.Clamp(transform.position.y, ymin, ymax);
        transform.position = new Vector3(newX, newY, transform.position.z);
	}
    void Fire()
    {
        Vector3 offSet = new Vector3(0, 1, 0);
        GameObject beam = Instantiate(projectile, transform.position+offSet, Quaternion.identity) as GameObject;
        beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile missile = collision.gameObject.GetComponent<Projectile>();
        if (missile)
        {
            health -= missile.GetDamage();
            missile.Hit();
            AudioSource.PlayClipAtPoint(hitSound, transform.position);


            if (health <= 0)
            {
                Die();
            }
        }
    }
    void Die() {
        Destroy(gameObject);
        Debug.Log("Die and Load Score Screen please");

        LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        man.LoadLevel("Score Screen");

        Debug.Log("Debug Die End...");



    }

}
