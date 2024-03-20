using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour {
    public float speed = 2f;
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private bool isGrounded; 

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        Vector2 movement = Vector2.zero;

        // Mover hacia la derecha con la tecla "d"
        if (Input.GetKey("d")) {
            movement += Vector2.right * speed * Time.deltaTime;
        }

        // Mover hacia la izquierda con la tecla "a"
        if (Input.GetKey("a")) {
            movement += Vector2.left * speed * Time.deltaTime;
        }

        // Aplicar el movimiento al objeto
        transform.Translate(movement);

        // Saltar con la tecla de espacio si está en el suelo
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded ) {
            Jump();
        }
    }

    private void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    // Método llamado cuando el personaje colisiona con otro objeto
    private void OnCollisionEnter2D(Collision2D collision) {
        // Colision con Tilemap
        if (collision.gameObject.GetComponent<TilemapCollider2D>() != null) {
            isGrounded = true;
        }
    }

    // Método llamado cuando el personaje deja de colisionar con otro objeto
    private void OnCollisionExit2D(Collision2D collision) {
        // Colision con Tilemap
        if (collision.gameObject.GetComponent<TilemapCollider2D>() != null) {
            isGrounded = false;
        }
    }
}