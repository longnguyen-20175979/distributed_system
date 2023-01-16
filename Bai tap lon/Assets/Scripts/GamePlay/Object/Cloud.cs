using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class Cloud : MonoBehaviour
{
    SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState animationState;
    public Skeleton skeleton;
    private BoxCollider2D cloudCol;
    [SerializeField] private float idleDelay;
    [SerializeField] private float inDelay;
    [SerializeField] private float outDelay;

    private float distance;
    [SerializeField] private float cloudRange;

    [SpineAnimation]
    public string idleAnimationName;

    [SpineAnimation]
    public string inAnimationName;

    [SpineAnimation]
    public string outAnimationName;

    private void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        animationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.skeleton;
        cloudCol = GetComponent<BoxCollider2D>();
        skeletonAnimation.enabled = false;

        Action();
        animationState.End += delegate (TrackEntry trackEntry)
        {
            if (trackEntry.ToString() == idleAnimationName)
            {
                cloudCol.enabled = false;
            }
        };

        animationState.Event += delegate (TrackEntry trackEntry, Spine.Event e)
        {
            if (e.Data.Name == "end")
            {
                cloudCol.enabled = true;
                Action();
            }
        };       
    }

    private void Update()
    {
        distance = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
        if (distance <= cloudRange)
        { 
            skeletonAnimation.enabled = true;
        }
        else
        {
            skeletonAnimation.enabled = false;
        }
    }

    private void Action()
    {
        animationState.AddAnimation(0, idleAnimationName, false, idleDelay);
        animationState.AddAnimation(0, outAnimationName, false, outDelay);     
        animationState.AddAnimation(0, inAnimationName, false, inDelay);
    }
}
