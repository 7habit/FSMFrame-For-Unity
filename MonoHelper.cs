using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace FSMFrame
{
    public class MonoHelper : MonoBehaviour
    {
        //单例
        public static MonoHelper Instance;
        //更新事件列表
        private List<EventState> _updateEventList;
        [Header("更新事件时间间隔")] public float invokeInterval;

        
        /// <summary>
        /// 给更新事件列表添加事件
        /// </summary>
        /// <param name="eventState">更新事件</param>
        public void AddUpdateEvent(EventState eventState)
        {
            //如果包含
            if (_updateEventList.Contains(eventState))
            {
                Debug.LogWarning("更新事件列表已存在此事件，请不要重复添加");
                return;
            }
            //添加
            _updateEventList.Add(eventState);
        }

        /// <summary>
        /// 在更新事件列表移除事件
        /// </summary>
        /// <param name="eventState">更新事件</param>
        public void RemoveUpdateEvent(EventState eventState)
        {
            //如果不包含
            if (!_updateEventList.Contains(eventState))
            {
                Debug.LogWarning("更新事件列表不存在此事件，无法移除");
                return;
            }
            //移除
            _updateEventList.Remove(eventState);
        }


        private void Awake()
        {
            //初始化
            Instance = this;
            _updateEventList = new List<EventState>();
        }

        private IEnumerator Start()
        {
            //执行更新事件
            while (true)
            {
                //间隔时间大于0
                if (invokeInterval > 0)
                {
                    //等待间隔时间
                    yield return new WaitForSeconds(invokeInterval);
                }
                else
                {
                    //等待1帧
                    yield return 0;
                }
                //轮询执行
                for (int i = 0; i < _updateEventList.Count; i++)
                {
                    _updateEventList[i].action(_updateEventList[i].parameters);
                }
            }
        }
    }
}