using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class RaycastDebug : MonoBehaviour
{
    void Update()
    {
        var mouse = Mouse.current;
        if (mouse == null) { Debug.Log("No Mouse.current"); return; }

        var data = new PointerEventData(EventSystem.current) { position = mouse.position.ReadValue() };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);

        Debug.Log($"Pos {data.position} | Hits: " + (results.Count > 0 ? results[0].gameObject.name : "none"));
    }
}