using System.Collections.Generic;
using UnityEngine;

namespace RPG_Adventure
{
    public enum QuestStatus
    {
        ACTIVE,
        FAILED,
        COMPLETED
    }

    public class AcceptedQuest : Quest
    {
        // All of 'Quest' properties are inherited from a Quest class
        // AcceptedQuest should have all properties of Quest model + QuestStatus
        public QuestStatus questStatus;
    }

    public class QuestLog : MonoBehaviour
    {
        public List<AcceptedQuest> quests; // Dynamic list, will grow as you add quests to it
    }

}