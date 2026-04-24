using UnityEngine;

public class MenuSpawnWizard : MonoBehaviour
{
    public float speed = 15f;
    void Update()
    {
        transform.Translate(new Vector3(-6.7f, 0f, -23.7f).normalized * -speed * Time.deltaTime);
    }

    public void Destroyable(float time)
    {
        Destroy(gameObject, time);
    }

}
