using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    public float health = 150f;
    public GameObject projectile;
    public float projectileSpeed = 5f;
    public float shotsPerSecond = 0.1f;
    public int scoreValue = 15;
    private ScoreKeeper scoreKeeper;

    public AudioClip fireSound;
    public AudioClip deathSound;

    private void Start()
    {
       scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
    }
    private void Update()
    {
        shotsPerSecond += 0.001f;
        float probabilityOfFire = Time.deltaTime * shotsPerSecond;
        if (Random.value < probabilityOfFire)
        {
            Fire();
        }
    }
    //Collision with Missiles
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile missile = collision.gameObject.GetComponent<Projectile>();
        if (missile) {
            health -= missile.GetDamage();
            missile.Hit();
            if (health <= 0) {
                Die();

            }
        }
    }
    void Die() {
        Destroy(gameObject);
        scoreKeeper.Score(scoreValue);
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
    }    
    void Fire() {
        GameObject enemyMissile = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        enemyMissile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);

    }

}
