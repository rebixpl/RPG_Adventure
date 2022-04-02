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
            // Assign Quests to all of our QuestGivers
            AssignQuests();
        }

        private void LoadQuestsFromDB()
        {
            // we don't use 'using' with StreamReader in the example below, thus we use try,catch,finally statement to be sure
            // the code will not crash, also we need to manually dispose of reader in finally block
            //StreamReader reader = new StreamReader("Assets/RPG_Adventure/DB/QuestDB.json");

            //try
            //{
            //    string json = reader.ReadToEnd();
            //    var loadedQuests = JsonHelper.GetJsonArray<Quest>(json); // <Quest> mean wi will replace all the <T>'s in
            //*                                                           // GetJsonArray method in JsonHelper with <Quest>
            //    quests = new Quest[loadedQuests.Length];
            //    quests = loadedQuests;
            //}
            //finally
            //{
            //    // when not using 'using' with StreamReader, we need to manually dispose of the reader
            //    reader.Dispose();
            //}

            // we use 'using' when working with files, when 'using' ends, it will automatically dispose the reader, this
            // is much easier than the example up without 'using' before StreamReader
            using StreamReader reader = new StreamReader("Assets/RPG_Adventure/DB/QuestDB.json");

            string json = reader.ReadToEnd();
            var loadedQuests = JsonHelper.GetJsonArray<Quest>(json); // <Quest> mean wi will replace all the <T>'s in
                                                                     // GetJsonArray method in JsonHelper with <Quest>
            quests = new Quest[loadedQuests.Length];
            quests = loadedQuests;
        }

        private void AssignQuests()
        {
            QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>(); // Gets a list of all loaded objects of Type QuestGiver.

            // Check if quest givers were found
            if (questGivers != null && questGivers.Length > 0)
            {
                // Iterate through every questgiver that was found
                foreach (QuestGiver questGiver in questGivers)
                {
                    // Assign quest to it
                    AssignQuestTo(questGiver);
                }
            }
        }

        private void AssignQuestTo(QuestGiver questGiver)
        {
            // Iterate through all quests and check the questgiver ID in quest json with this questGiver Uid.
            foreach (Quest quest in quests)
            {
                if (quest.questGiver == questGiver.GetComponent<UniqueId>().Uid)
                {
                    questGiver.quest = quest;
                }
            }
        }
    }
}