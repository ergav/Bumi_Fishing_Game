using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    public List<RectTransform> inventorySlots;
    public List<Draggable> draggableObjects;
    public float snapRange = 0.5f;


    private void OnDragEnded(Draggable draggable)
    {
        float closestDistance = -1f;
        Transform closestSlot = null;

        foreach (Transform inventorySlot in inventorySlots)
        {
            float currentDistance = Vector3.Distance(draggable.transform.localPosition, inventorySlot.localPosition);

            if (closestSlot == null || currentDistance < closestDistance)
            {
                closestSlot = inventorySlot;
                closestDistance = currentDistance;
            }
        }

        if (closestSlot != null && closestDistance <= snapRange)

        {
            draggable.transform.localPosition = closestSlot.localPosition;
        }
    }
}
