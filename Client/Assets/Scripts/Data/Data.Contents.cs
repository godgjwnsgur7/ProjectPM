using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Data
{
    #region TestData
    [Serializable]
    public class TestData
    {
        public int DataId;
        public string TestString;
        public float TestFloat;
        public bool TestBool;
    }

    [Serializable]
    public class TestDataLoader : ILoader<int, TestData>
    {
        public List<TestData> tests = new List<TestData>();
        public Dictionary<int, TestData> MakeDict()
        {
            Dictionary<int, TestData> dict = new Dictionary<int, TestData>();
            foreach (TestData test in tests)
                dict.Add(test.DataId, test);
            return dict;
        }
    }
    #endregion

    #region CharacterGearData
    [Serializable]
    public class CharacterGearData
    {
        // ĳ��������ϳ��� ���� ������
        
        public string Name; // Key
        public int Key;
        // � ������ �������?
        // � ĳ���Ͱ� ������ �� �ִ� �������?
        // �󸶳� ������ ��ĥ ������?
    }

    [Serializable]
    public class CharacterGearDataaLoader : ILoader<string,  CharacterGearData>
    {
        public List<CharacterGearData> characterGearDatas = new List<CharacterGearData>();
        public Dictionary<string,  CharacterGearData> MakeDict()
        {
            Dictionary<string, CharacterGearData> dict = new Dictionary<string, CharacterGearData>();
            foreach (CharacterGearData data in characterGearDatas)
                dict.Add(data.Name, data);
            return dict;
        }
    }
    #endregion
}