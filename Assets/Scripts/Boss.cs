using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Boss : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth {
        get;
        private set;
    }

    public Transform target;
    public float fireRate;
    public GameObject projectile;
    public float moveSpeed = 8;
    public List<Transform> waypoints;

    public UnityEvent OnDeath;
    public UnityEvent OnHurt;

    private SceneTransitions st;
    private Animator anim;

    private bool invincible = false;

    public bool isAlive {
        get {
            return currentHealth > 0;
        }
    }

    private void Start() {
        if (target == null) {
            target = FindObjectOfType<PlayerMovement>().transform;
        }
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        st = FindObjectOfType<SceneTransitions>();
        OnHurt.AddListener(OnHurted);
        OnDeath.AddListener(OnDied);
        StartCoroutine(TraverseWaypoints());
    }

    private IEnumerator Shoot() {
        // TODO: Make a triangle of bullets
        Quaternion rot1 = Quaternion.Euler(0, 0, 120);
        Quaternion rot2 = Quaternion.Euler(0, 0, -120);
        while (isAlive) {
            Vector3 dirToPlayer = (target.position - transform.position).normalized;
            Vector3 spawnPos = transform.position + dirToPlayer * 1.5f;
            List<GameObject> bullets = new List<GameObject> {
                Instantiate(projectile, spawnPos + dirToPlayer, Quaternion.identity),
                Instantiate(projectile, spawnPos + (rot1 * dirToPlayer), Quaternion.identity),
                Instantiate(projectile, spawnPos + (rot2 * dirToPlayer), Quaternion.identity)
            };
            float startTime = Time.time;
            foreach (GameObject bullet in bullets) {
                bullet.transform.up = dirToPlayer;
                Destroy(bullet, fireRate * 5);
            }
            while (Time.time - startTime <= fireRate * 4.9f) {
                foreach (GameObject bullet in bullets) {
                    bullet.transform.Translate(bullet.transform.up.normalized * moveSpeed * 2 * Time.deltaTime, Space.World);
                }
                yield return null;
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

    private IEnumerator TraverseWaypoints() {
        yield return new WaitForSeconds(3f);
        StartCoroutine(Shoot());
        while (isAlive) {
            foreach (Transform pos in waypoints) {
                Vector3 dirToPos = (transform.position - pos.position).normalized;
                while (Vector3.Distance(transform.position, pos.position) > 1f) {
                    transform.Translate(dirToPos * moveSpeed * Time.deltaTime);
                    yield return null;
                }
            }
            yield return null;
        }
    }

    public void TakeDamage(int damage) {
        if (invincible)
            return;
        currentHealth -= damage;
        if (currentHealth <= 0) {
            if (OnDeath != null)
                OnDeath.Invoke();
        } else {
            if (OnHurt != null)
                OnHurt.Invoke();
        }
    }

    private void OnDied() {
        Destroy(gameObject);
    }

    private void OnHurted() {
        invincible = true;
        anim.SetTrigger("Hurt");
        Invoke("ResetInvincibility", 1);
    }

    private void ResetInvincibility() {
        invincible = false;
        anim.ResetTrigger("Hurt");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            st.Respawn();
        }
    }
}
