

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rigidbody;
    private float moveInput;
    public float moveSpeed = 6f;
    public float jumpForce = 8f;

    private Vector2 respawnPos;
    private bool isGrounded;
    public Transform groundCheck;

    private Collider2D _collider;
    [SerializeField] private bool _active = true;
    

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        Respawn((Vector2)transform.position);


    }
    private void FixedUpdate()
    {
        GroundChek();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_active)
        {
            return;
        }
        
        if (Input.GetButton("Horizontal"))
        {
            moveInput = Input.GetAxis("Horizontal");
            Vector3 direction = transform.right * moveInput;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, moveSpeed * Time.deltaTime);
        }
        
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void GroundChek()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
        isGrounded = colliders.Length > 1;
    }


    public void Die()
    {
        _active = false;
        _collider.enabled = false;
        StartCoroutine(routine: RespawnP());
        MiniJump();
    }
    public void Respawn(Vector3 position)
    {
        respawnPos = position;
    }
    private IEnumerator RespawnP()
    {
        yield return new WaitForSeconds(2f);
        transform.position = (Vector3)respawnPos;
        _active = true;
        _collider.enabled = true;
        MiniJump();
        SceneManager.LoadScene(0);
    }
    private void MiniJump()
    {
        rigidbody.AddForce(transform.up * jumpForce / 2, ForceMode2D.Impulse);
    }
}

