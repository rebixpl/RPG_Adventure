using System;
using UnityEngine;

namespace RPG_Adventure
{
    public class UniqueId : MonoBehaviour
    {
        // It works as a prefab id basically, when you create multiple instances of a prefab (like multiple enemies of the same
        // type) they will have the same ids.
        [SerializeField]
        private string m_Uid = Guid.NewGuid().ToString();

        public string Uid
        { get { return m_Uid; } }
    }
}