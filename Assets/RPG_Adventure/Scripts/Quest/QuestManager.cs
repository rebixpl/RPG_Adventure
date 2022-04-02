using System.IO;
using UnityEngine;

namespace RPG_Adventure
{
    public class JsonHelper
    {
        private class Wrapper<T>
        {
            public T[] array;
        }

        public static T[] GetJsonArray<T>(string json)
        {
            string newJson = "{\"array\":" + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        // WE COULD USE <Quest> FOR JSON HELPER, BUT WE USE <T> GENERIC TYPE INSTEAD, BECAUSE WITH THAT WE CAN USE THIS CLASS
        // TO EXTRACT FROM JSON OTHER DATA NOT ONLY <Quest> TYPE

        //private class Wrapper<Quest>
        //{
        //    public Quest[] array;
        //}

        //public static Quest[] GetJsonArray<Quest>(string json)
        //{
        //    string newJson = "{\"array\":" + json + "}";
        //    Wrapper<Quest> wrapper = JsonUtility.FromJson<Wrapper<Quest>>(newJson);
        //    return wrapper.array;
        //}
    }

    public class QuestManager : MonoBehaviour
    {
        public Quest[] quests;

        private void Awake()
        {
            LoadQuestsFromDB();
        }

        private void LoadQuestsFromDB()
        {
            // we use 'using' when working with files
            using (StreamReader reader = new StreamReader("Assets/RPG_Adventure/DB/QuestDB.json"))
            {
                string json = reader.ReadToEnd();
                var loadedQuests = JsonHelper.GetJsonArray<Quest>(json); // <Quest> mean wi will replace all the <T>'s in
                                                                         // GetJsonArray method in JsonHelper with <Quest>
                quests = new Quest[loadedQuests.Length];
                quests = loadedQuests;
            }
        }
    }
}