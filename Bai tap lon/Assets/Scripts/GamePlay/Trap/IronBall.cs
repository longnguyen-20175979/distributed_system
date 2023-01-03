using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IronBall : Trap
{
    private enum RotateType { Rotate360, RotateByAngle }
    [SerializeField] private RotateType rotateType;
    private enum RotateDirection { Left, Right }
    [SerializeField] private RotateDirection direction;

    [SerializeField] private Ease ease;
    [SerializeField] private float speed;
    private float duration;
    private bool isRotate = true;

    [SerializeField] private Animator thornBall;

    private void Start()
    {
        duration = 1 / speed;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (distance <= rangeCheck)
        {
            Rotate(rotateType);
        }
    }

    private void Rotate(RotateType type)
    {
        if (type == RotateType.RotateByAngle)
        {
            if (isRotate)
            {
                isRotate = false;
                transform.DOLocalRotate(Vector3.forward * 45, duration, RotateMode.Fast).SetEase(ease).OnComplete(() =>
                {
                    thornBall.SetTrigger("Left");
                    transform.DOLocalRotate(Vector3.forward * -45, duration, RotateMode.Fast).SetEase(ease).OnComplete(() =>
                    {
                        isRotate = true;
                        thornBall.SetTrigger("Right");
                    });
                });
            }
        }
        else if (type == RotateType.Rotate360)
        {
            if (isRotate)
            {
                isRotate = false;
                transform.DOLocalRotate((direction == RotateDirection.Left) ? Vector3.back : Vector3.forward * 360, duration, RotateMode.FastBeyond360).SetEase(ease).OnComplete(() =>
                {
                    isRotate = true;
                });
            }
        }
    }
}
