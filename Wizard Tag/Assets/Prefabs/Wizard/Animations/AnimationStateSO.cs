using UnityEngine;

[CreateAssetMenu(fileName = "AnimationStateSO", menuName = "Scriptable Objects/AnimationStateSO")]
public class AnimationStateSO : ScriptableObject
{
    public AnimationClip patrol;
    public AnimationClip chase;
    public AnimationClip attack;
}
