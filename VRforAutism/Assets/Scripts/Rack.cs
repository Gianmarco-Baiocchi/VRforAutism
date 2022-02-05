using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rack : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    private float distance_ = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.distance_ = obj.GetComponent<CapsuleCollider>().radius;
        this.fillRank();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void fillRank()
    {
        for (int i = 0; i < this.transform.GetChild(1).childCount; i++)
        {
            float n = this.GetComponent<BoxCollider>().size.z / ((obj.GetComponent<CapsuleCollider>().radius + this.distance_) * 2);
            for (int j = 1; j < n - 1; j++)
            {
                //Prima fila
                var bottle = Instantiate<GameObject>(obj);
                Vector3 start_pos = this.transform.GetChild(1).GetChild(i).position;
                Vector3 pos = start_pos + new Vector3(0, 0, ((obj.GetComponent<CapsuleCollider>().radius + this.distance_) * 2 * j));
                pos.z -= this.GetComponent<BoxCollider>().size.z / 2;
                pos.x -= this.GetComponents<BoxCollider>()[1].size.x / 8;
                bottle.transform.position = pos;
                //Seconda fila
                bottle = Instantiate<GameObject>(obj);
                start_pos = this.transform.GetChild(1).GetChild(i).position;
                pos = start_pos + new Vector3(0, 0, ((obj.GetComponent<CapsuleCollider>().radius + this.distance_) * 2 * j));
                pos.z -= this.GetComponent<BoxCollider>().size.z / 2;
                pos.x += this.GetComponents<BoxCollider>()[1].size.x / 8;
                bottle.transform.position = pos;

            }
        }
    }
}
