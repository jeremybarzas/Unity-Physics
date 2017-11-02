using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public struct Pair
{
    public AABB_A a;
    public AABB_A b;
}

public class SortAndSweep : MonoBehaviour
{
    // list of all objects that can collide
    public List<AABB_A> axisList = new List<AABB_A>();  

    // list of all collisions
    public List<Pair> reportedPairs = new List<Pair>();

    public void BroadPhase()
    {
        // fill axisList with all objects in world
        FillAxisList();

        // sort axisList by MinX = XAxis / MinY = YAxis
        SortAxisList();      
  // list of objects that will collide
            List<AABB_A> activeList = new List<AABB_A>();

        int current = 0;
        int next = current + 1;
        foreach (AABB_A axisItem in axisList)
        {
            // add first item to active list
            var currentItem = axisList[current];

            // look at next item            
            var newItem = axisList[current];
            activeList.Add(newItem);
            // compare it with all in active list            
            foreach (var activeItem in activeList)
            {  
                if (newItem.left > activeItem.right)
                {
                    activeList.Remove(activeItem);
                }
                else
                {                    
                    reportedPairs.Add(new Pair() { a = newItem, b = activeItem });
                    activeList.Add(newItem);
                }
            }

            current++;
        }
        
    }

    public void FillAxisList()
    {
        //axisList.Add(new AABB(new Vector2(1, 1), new Vector2(2, 2)));

        axisList.Add(new AABB_A(new Vector2(3, 3), 2, 2));
        axisList.Add(new AABB_A(new Vector2(6, 7), 2, 2));
        axisList.Add(new AABB_A(new Vector2(1, 2), 2, 2));
        axisList.Add(new AABB_A(new Vector2(10, 8), 2, 2));
        axisList.Add(new AABB_A(new Vector2(5, 2), 2, 2));
    }

    public void SortAxisList()
    {
        axisList.Sort((AABB_A obj1, AABB_A obj2) => { return obj1.left.CompareTo(obj2.left); });     
    }


    // Use this for initialization
    void Start ()
    {
        BroadPhase();
        reportedPairs.Count();
        return;
    }

    [CustomEditor(typeof(SortAndSweep))]
    public class EditorSortAndSweep : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var mytarget = target as SortAndSweep;
            if(GUILayout.Button("BroadPhase Sweep"))
            {
                mytarget.BroadPhase();
            }
        }
    }

    
}
