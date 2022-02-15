using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLeaf : MonoBehaviour
{
    [SerializeField] private float _animationTime;
    [SerializeField] private bool _toRight;
    private bool _isOpening = false;
    private bool _isOpen = false;
    private Vector3 _originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        this._originalPosition = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenEntrance()
    {
        if (this._isOpening || this._isOpen)
            return;
        var targetPosition = this._toRight ? this._originalPosition + new Vector3(this.GetComponent<BoxCollider>().size.x * this.transform.localScale.x, 0, 0) 
                                           : this._originalPosition - new Vector3(this.GetComponent<BoxCollider>().size.x * this.transform.localScale.x, 0, 0);
        StartCoroutine(this.AnimateDoor(targetPosition));
    }

    public void CloseEntrance()
    {
        if (this._isOpening || !this._isOpen)
            return;
        StartCoroutine(this.AnimateDoor(this._originalPosition));
    }

    private IEnumerator AnimateDoor(Vector3 endPosition)
    {
        this._isOpening = true;

        float animationTimer = 0;
        Vector3 startPosition = this.transform.localPosition;
        while (animationTimer < this._animationTime)
        {
            animationTimer += Time.deltaTime;
            this.transform.localPosition = (endPosition - startPosition) * (animationTimer / this._animationTime) + startPosition;
            yield return null;
        }

        this._isOpening = false;
        this._isOpen = !this._isOpen;
    }
}
