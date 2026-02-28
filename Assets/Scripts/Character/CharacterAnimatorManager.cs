using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    float vertical;
    float horizontal;
    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    public void UpdateAnimatorMovementParameter(float horizontalValue, float verticalValue)
    {
        character.animator.SetFloat("Horizontal", horizontalValue, 0.2f, Time.deltaTime);
        character.animator.SetFloat("Vertical", verticalValue, 0.2f, Time.deltaTime);
    }
}
