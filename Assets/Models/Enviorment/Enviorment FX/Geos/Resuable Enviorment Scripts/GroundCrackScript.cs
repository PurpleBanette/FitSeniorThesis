using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCrackScript : MonoBehaviour
{
    public int lenght; //4
    public int BlendShapeCount; //5 BlendShape Ammount
    public List<Transform> CornerPoints;

    [SerializeField] SkinnedMeshRenderer _Crack;
    [SerializeField] SkinnedMeshRenderer _CrackMask;


    public float GetBlendShape(int index)
    {
        return 100 - _Crack.GetBlendShapeWeight(index);
    }

    public void SetBlendShape(int index, float value)
    {
        _Crack.SetBlendShapeWeight(index, 100 - value);
        _CrackMask.SetBlendShapeWeight(index, 100 - value);
    }
}
