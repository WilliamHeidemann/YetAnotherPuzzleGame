using System.Collections.Generic;
using Components;
using ScriptableObjects;
using UnityEngine;
using UnityUtils;
using UtilityToolkit.Editor;

namespace Systems
{
    public class SaveSystemEditorTool : MonoBehaviour
    {
        [Button]
        public void ResetProgress()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
    
    public static class SaveSystem
    {
        // Purpose: query which levels have been completed
        // It does not know the context of worlds and which levels are required to play other levels.
        // That is the responsibility of the level manager
        
        // Levels may
        // - get new positions in each world
        // - be changed to a different world 
        // - be deleted
        // - be changed internally 
        
        // the progress tracker should only keep track of whether the level has been completed
        // not the context in which it was completed
        
        // Solution
        // Save the id of all levels that have been completed. 

        public static void SetComplete(Level level)
        {
            PlayerPrefs.SetInt(level.id, 1);
            PlayerPrefs.Save();
        }

        public static bool HasBeenCompleted(Level level) => PlayerPrefs.HasKey(level.id);
    }
}