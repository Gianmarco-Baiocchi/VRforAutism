using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RackRow : MonoBehaviour
{
    [SerializeField] private int nRacks_ = 0;
    [SerializeField] private GameObject rackPrefab_;
    [SerializeField] private float distance_ = 0.1f;
    private List<string> objects_ = new List<string>() { "Prefabs/Bottiglietta", "Prefabs/Flacone Shampoo", "Prefabs/Lattina 1", 
                                                         "Prefabs/Bottiglietta", "Prefabs/Flacone Shampoo" };

    // Start is called before the first frame update
    void Start()
    {
        if(this.rackPrefab_ == null)
            this.rackPrefab_ = (GameObject) Resources.Load("Prefabs/SupermarketRack");
        this.createRacks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void createRacks()
    {
        int objIndex = 0;
        for(int i = 0; i < this.nRacks_; i++)
        {
            var rack = Instantiate<GameObject>(this.rackPrefab_);
            Vector3 pos = this.transform.position;
            if (i < this.nRacks_ / 2)
                pos.x -= (rack.GetComponent<ISupermarketContainer>().GetLength() + this.distance_) * (this.nRacks_ / 2 - i);
            else
                pos.x += (rack.GetComponent<ISupermarketContainer>().GetLength() + this.distance_) * (i - this.nRacks_ / 2);
            rack.transform.position = pos;
            rack.transform.RotateAround(this.transform.position, new Vector3(0, 1, 0), this.transform.rotation.eulerAngles.y);
            rack.GetComponent<ISupermarketContainer>().SetObjects(this.objects_.ConvertAll(s => (GameObject)Resources.Load(s, typeof(GameObject))));
            
            objIndex = ++objIndex % this.objects_.Count;
        }
    }
}
