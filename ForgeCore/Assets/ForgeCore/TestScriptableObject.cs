using System.Collections.Generic;
using UnityEngine;

namespace ForgeCore
{
    [CreateAssetMenu(fileName = "TestScriptableObject", menuName = "ScriptableObjects/TestScriptableObject")]
    public class TestScriptableObject : ScriptableObject
    {
        public Dictionary<int, bool> TestBoolDictionary = new();
    }
}