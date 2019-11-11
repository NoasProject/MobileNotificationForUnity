using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noa
{
    /// <summary>
    /// SingletonMonoを管理するクラス
    /// </summary>
    public class SingletonManager : MonoBehaviour
    {
        protected Hashtable HashTable { get; private set; } = new Hashtable();

        /// <summary>
        /// １つのみ削除する関数
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        public void Remove<TClass>() where TClass : class, new()
        {
            string key = typeof(TClass).FullName;

            // 存在すれば
            if (this.HashTable.ContainsKey(key))
            {
                this.HashTable[key] = null;
            }
        }

        /// <summary>
        /// 取得する関数
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        public TClass Get<TClass>() where TClass : class, new()
        {
            string key = typeof(TClass).FullName;

            System.Object obj = null;

            // 存在すれば
            if (this.HashTable.ContainsKey(key))
            {
                obj = this.HashTable[key];
            }

            // ないならば生成を行う
            if (obj == null)
            {
                this.HashTable[key] = new TClass();
            }

            return (obj as TClass);
        }

        /// <summary>
        /// マネージャークラスを管理する１つのGameObject.
        /// </summary>
        private static SingletonManager m_Ins = null;
        public static SingletonManager Instance()
        {
            if (m_Ins == null)
            {
                m_Ins = mCreate(false);
            }

            return m_Ins;
        }

        public static void Destroy()
        {
            if (m_Ins != null)
            {
                GameObject.Destroy(m_Ins.gameObject);
            }
        }

        /// <summary>
        /// 全てを削除するメソッド
        /// </summary>
        public static void Reset()
        {

        }
        /// <summary>
        /// シーンを切り替えても削除されないオブジェクト
        /// </summary>
        public struct DontDestroy
        {
            private static SingletonManager m_Ins = null;
            public static SingletonManager Instance()
            {
                if (m_Ins == null)
                {
                    m_Ins = mCreate(true);
                }

                return m_Ins;
            }

            public static void Destroy()
            {
                if (m_Ins != null)
                {
                    GameObject.Destroy(m_Ins.gameObject);
                }
            }
        }

        /// <summary>
        /// Managerの生成
        /// </summary>
        /// <param name="isDontDestroy"></param>
        /// <returns></returns>
        private static SingletonManager mCreate(bool isDontDestroy)
        {
            GameObject go = new GameObject("Singleton Manager");

            SingletonManager manager = go.AddComponent<SingletonManager>();

            go.SetActive(true);

            if (isDontDestroy)
            {
                go.name += " - DontDestroy";
                DontDestroyOnLoad(go);
            }

            return manager;
        }
    }
}