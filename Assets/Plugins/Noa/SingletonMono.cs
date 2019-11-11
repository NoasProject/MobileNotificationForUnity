using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noa
{
    public class Singleton<T> : SingletonManager where T : class, new()
    {
        public static T Ins { get { return SingletonManager.Instance().Get<T>(); } }

        public static new void Destroy()
        {
            SingletonManager.Instance().Remove<T>();
        }
    }

    public class BaseSingleton
    {

    }

    public class SingletonMonoBehaviour<T> : BaseSingletonMonoBehaviour where T : BaseSingletonMonoBehaviour
    {
        private static T mIns = null;
        public static T Ins
        {
            get
            {
                if (mIns == null)
                {
                    string name = typeof(T).FullName;

                    GameObject obj = new GameObject(name);

                    obj.SetActive(false);

                    mIns = obj.AddComponent<T>();

                    // 親オブジェクトの設定
                    Transform parent = SingletonManager.Instance().transform;

                    // DontDestroy専用のオブジェクトに親を設定する
                    if (mIns.IsDontDestroy)
                    {
                        parent = SingletonManager.DontDestroy.Instance().transform;
                    }

                    // 親オブジェクトに設定をする
                    mIns.gameObject.transform.SetParent(parent, false);

                    // 初期化処理を行う
                    mIns.Initialize(false);

                    // アクティブに変更する
                    mIns.gameObject.SetActive(true);
                }

                return mIns;
            }
        }

        // 単体削除
        public static void Destroy()
        {
            if (mIns != null)
            {
                Destroy(mIns.gameObject);
            }
            mIns = null;
        }
    }

    [System.Serializable]
    public class BaseSingletonMonoBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 初期化をしているかどうか
        /// </summary>
        protected bool IsInitialize { get; private set; }

        /// <summary>
        /// DontDestroyにするかどうか
        /// </summary>
        public virtual bool IsDontDestroy
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 初期化処理用のメソッド
        /// </summary>
        protected virtual void OnInitialize()
        {

        }

        public void Initialize(bool isForce = false)
        {
            if (!this.IsInitialize || isForce)
            {
                this.IsInitialize = true;

                this.OnInitialize();
            }
        }
    }
}