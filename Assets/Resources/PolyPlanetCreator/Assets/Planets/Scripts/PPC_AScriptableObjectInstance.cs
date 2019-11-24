using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class PPC_AScriptableObjectInstance<T> : ScriptableObject
    where T : ScriptableObject
{
#if UNITY_EDITOR
    protected const string defaultResourcesPath = "PolyPlanetCreator/Resources";

    protected static T CreateScriptableObjectAsset()
    {
        T instance = CreateInstance<T>();

        string path = string.Format("{0}/{1}", Application.dataPath, defaultResourcesPath);
        Debug.Log("Creating new scriptable object asset '" + typeof(T).ToString() + "' at: " + path);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        
        AssetDatabase.CreateAsset(instance, string.Format("Assets/{0}/{1}.asset", defaultResourcesPath, typeof(T).Name));
        AssetDatabase.Refresh();
        return instance;
    }
#endif

    protected static T m_instance;
    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<T>();
                if (m_instance == null)
                {
                    m_instance = Resources.Load<T>(typeof(T).Name);
#if UNITY_EDITOR
                    if (m_instance == null)
                        m_instance = CreateScriptableObjectAsset();
#endif
                }
            }
            
            return m_instance;
        }
    }
}
