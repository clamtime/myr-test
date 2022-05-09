using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public List<GameObject> projectiles = new List<GameObject>();
    public UnityEngine.UI.Text displayText;
    public float gunForce = 20.0f;
    private int selected = 0;

    void Start()
    {
        displayText.text = "Current Gun Type: " + projectiles[selected].name;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Projectile p = Instantiate(projectiles[selected], transform.position, transform.rotation, null).GetComponent<Projectile>();
            p.SetForceDir(Camera.main.transform.forward, gunForce);
        }

        if (Input.GetMouseButtonDown(1))
            cycleSelection(true);
    }

    private void cycleSelection(bool pos)
    {
        selected += pos ? 1 : -1;

        if (selected > projectiles.Count - 1)
            selected = 0;
        if (selected < 0)
            selected = projectiles.Count - 1;

        displayText.text = "Current Gun Type: " + projectiles[selected].gameObject.name;
    }
}
