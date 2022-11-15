using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public float moveTime = 0.1f;            
    public LayerMask blockingLayer;            


    private BoxCollider2D boxCollider;         
    private Rigidbody2D rb2D;               
    private float inverseMoveTime;            //Used to make movement more efficient.
    private bool isMoving;//


    
    protected virtual void Start ()
    {
        boxCollider = GetComponent <BoxCollider2D> ();
        rb2D = GetComponent <Rigidbody2D> ();

        //By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
        inverseMoveTime = 1f / moveTime;
    }

    protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2 (xDir, yDir);
        boxCollider.enabled = false;

        hit = Physics2D.Linecast (start, end, blockingLayer);
        boxCollider.enabled = true;

        //Check if anything was hit
        if(hit.transform == null && !isMoving)
        {
            StartCoroutine (SmoothMovement (end));
            return true;
        }

        return false;
    }

    protected IEnumerator SmoothMovement (Vector3 end)
    {
        //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
        //Square magnitude is used instead of magnitude because it's computationally cheaper.
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        isMoving = true;

        while(sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition (newPostion);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }

        rb2D.MovePosition(end);
        isMoving = false;
    }

    protected virtual void AttemptMove <T> (int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move (xDir, yDir, out hit);
        if(hit.transform == null)
            return;

        //Get a component reference to the component of type T attached to the object that was hit
        T hitComponent = hit.transform.GetComponent <T> ();

        if(!canMove && hitComponent != null)
            OnCantMove (hitComponent);
    }

    protected abstract void OnCantMove <T> (T component)
        where T : Component;
}