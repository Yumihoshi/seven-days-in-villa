using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseNpcEntity : InteractableItem
{
   public Animator animator;
   public int grade;
   
   public Seeker seeker;

   public bool IsMove;
   
   public List<Vector3> waypoints;

   public float radius = 0.5f;
   
   [SerializeField] private float Speed = 1.5f;
   
   public Rigidbody2D rb;

   int currentIndex = 0;

  
   [SerializeField] BaseRoom closestRoom = null;
   
   
   
   [SerializeField] private string MovingCoroId;

   [SerializeField] private float waitTimeMin = 1.4f;
   [SerializeField] private float waitTimeMax = 3.5f;
   
   public virtual void Awake()
   {
      seeker = GetComponent<Seeker>();
      if (seeker == null)
      {
         seeker = gameObject.AddComponent<Seeker>();
      }

      animator = GetComponentInChildren<Animator>();

      IsMove = false;

      rb = GetComponent<Rigidbody2D>();
      if (rb == null)
      {
         rb = gameObject.AddComponent<Rigidbody2D>();
      }
      rb.gravityScale = 0;

   }

   
   
   private void OnEnable()
   {
      CoroutineFactory.Instance.RunCoroutine(initPoint());
   }

   public void InstianceWays(Vector3 target)
   {
      seeker.StartPath(transform.position, target,PathGotten);
   }


   
   private void Update()
   {
      // Vector3? last = null;
      // if (closestRoom != null)
      // {
      //    foreach (var pt in closestRoom.MyPolyEdge)
      //    {
      //       if (pt == Vector3.positiveInfinity)
      //       {
      //          last = null; // ??????
      //          continue;
      //       }
      //
      //       if (last.HasValue)
      //          Debug.DrawLine(last.Value, pt, Color.cyan);
      //       last = pt;
      //    }
      //
      // }


   }


   IEnumerator initPoint()
   {
      yield return null;
      yield return null;
      yield return null;
      yield return null;
      yield return null;
      GenerateWaypoints();
   }
   
   protected Vector3 GetRandomPoint()
   {
      
      closestRoom=RoomManager.Instance.GetClosestRoom(transform.position);
      var position = closestRoom.GetRomdomPosition();
      return position;
   }

   public override void Interact()
   {
      Debug.LogWarning(" i am npc");
   }

   private void OnTriggerStay2D(Collider2D other)
   {
      
      if (other.gameObject.CompareTag("Player"))
      {
         PlayerAction.Instance.SetInteract(this);
         CoroutineFactory.Instance.HaltCoroutine(MovingCoroId);
      }
   }


   public override void OnTriggerEnter2D(Collider2D other)
   {
      base.OnTriggerEnter2D(other);
   }

   public override void OnTriggerExit2D(Collider2D other)
   {
      base.OnTriggerExit2D(other);
      if (other.gameObject.CompareTag("Player"))
      {
         PlayerAction.Instance.SetInteract(null);
         Move();
      }
   }
   
   

   public void Move()
   {
      if(waypoints==null||waypoints.Count==0)
         return;
      IsMove = true;
      if (MovingCoroId != null)
         CoroutineFactory.Instance.HaltCoroutine(MovingCoroId);
      MovingCoroId = CoroutineFactory.Instance.RunCoroutine(MoveCoroutine);
   }

   IEnumerator MoveCoroutine()
   {
      
      while (currentIndex < waypoints.Count)
      {
         
         Vector3 nowtarget = waypoints[currentIndex];
         Vector3 current = transform.position ;
         if (Vector2.Distance(nowtarget, current) < Mathf.Epsilon)
         {
            currentIndex++;
         }

         if (currentIndex >= waypoints.Count)
         {
            currentIndex = 0;
            InstianceWays(GetRandomPoint());
            float waitTime = Random.Range(waitTimeMin, waitTimeMax);
            animator.Play($"idle_{grade}");
            
            yield return new WaitForSeconds(waitTime);
            animator.Play($"{grade}Grade");
         }
         nowtarget = waypoints[currentIndex];
         
         var collider = Physics2D.OverlapCircle(transform.position, radius);
         
         if (collider != null && collider.CompareTag("Link"))
         {
           
         }
         else
         {
            Vector3 nextPosition=Vector2.MoveTowards(current, nowtarget
               , Speed * Time.deltaTime);
            
            rb.MovePosition(nextPosition);
            Vector3 dir=nextPosition-transform.position;
            animator.SetFloat("Xvelocity",dir.x*50);
            animator.SetFloat("Yvelocity",dir.y*100);
            
         }
         
         yield return null;
      }
   }

  
   public void GenerateWaypoints()
   {
      InstianceWays(GetRandomPoint());
   
   }

   [Button("test generate waypoints")]
   public void TestWaypoints()
   {
      GenerateWaypoints();
   }
   
   [Button("Move")]
   public void StartMoving()
   {
      Move();
   }
   void PathGotten(Path path)
   {
      waypoints = path.vectorPath;
   }

}
