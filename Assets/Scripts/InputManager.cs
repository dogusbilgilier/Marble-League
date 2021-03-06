using UnityEngine;

public class InputManager : MonoBehaviour
{
    Touch touch;
    [HideInInspector]public bool boost;
    public static InputManager inst;

    void Start()
    {
        inst = this;
        boost = false;
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
                boost = true;
        }
        else if (Input.GetMouseButtonDown(0))
            boost = true;

    }
}
