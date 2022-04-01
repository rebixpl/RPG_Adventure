using UnityEngine;

namespace RPG_Adventure
{
    public class Dissolve : MonoBehaviour
    {
        public float dissolveTime = 6.0f;

        private void Awake()
        {
            dissolveTime += Time.time;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Time.time >= dissolveTime)
            {
                Destroy(gameObject);
                //Transform parentTransform = gameObject.transform.parent;
                //Debug.Log(gameObject.transform.name);
                //Transform childToRemove = gameObject.transform.Find("BanditBody");
                //childToRemove.parent = null;

                //Transform[] children = transform.GetComponentsInChildren<Transform>();
                //foreach (var child in children)
                //{
                //    if (child.name == "BanditBody")
                //    {
                //        //do something with child
                //        Debug.Log(" Child found: " + child.name);
                //    }
                //}
            }
        }
    }
}