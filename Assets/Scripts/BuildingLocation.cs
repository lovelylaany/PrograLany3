using UnityEngine;
using UnityEngine.Events;

public class BuildingLocation : MonoBehaviour
{
    public UnityEvent OnSelected = new UnityEvent();
    public bool IsOccupied { get; private set; }

    public void Select()
    {
        OnSelected.Invoke();
    }

    public void MarkOccupied()
    {
        IsOccupied = true;
        GetComponent<Collider>().enabled = false;
    }
}
