using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePartManager : MonoBehaviour
{
    [System.Serializable]
    public class SnakePart
    {
        public Vector3 position;
        public Quaternion rotation;

        public SnakePart(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }

    public List<SnakePart> SnakeParts = new List<SnakePart>();

    void FixedUpdate()
    {
        UpdateSnakePartLocationList();    
    }

    private void UpdateSnakePartLocationList()
    {
        SnakeParts.Add(new SnakePart(transform.position, transform.rotation));
    }

    public void ClearUpdateSnakePartLocationList()
    {
        SnakeParts.Clear();
        UpdateSnakePartLocationList();
    }
}