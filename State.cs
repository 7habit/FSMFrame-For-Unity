using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace FSMFrame
{
    public class State
    {
        //封装状态进出事件
        protected List<EventState> OnEnterStateEventList;
        protected List<EventState> OnUpdateStateEventList;
        protected List<EventState> OnExitStateEventList;
        //是否正在运行
        public bool isRun { get; set; }
        
        //构造
        public State()
        {
            OnEnterStateEventList = new List<EventState>();
            OnUpdateStateEventList = new List<EventState>();
            OnExitStateEventList = new List<EventState>();
            //初始化函数
            InitState();
        }

        /// <summary>
        /// 初始化函数
        /// </summary>
        private void InitState()
        {
            //初始化isRun
            EventState enterSetIsRun = new EventState((objects) => { isRun = true; }, null);
            EventState exitSetIsRun = new EventState((objects) => { isRun = false; }, null);
            OnEnterStateEventList.Add(enterSetIsRun);
            OnExitStateEventList.Add(exitSetIsRun);
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="actionType">事件类型</param>
        /// <param name="action">事件</param>
        /// <param name="parameters">参数</param>
        public void AddAction(ActionType actionType, Action<object[]> action, object[] parameters)
        {
            if (isRun)
            {
                Debug.LogWarning("状态正在运行中，不允许添加事件");
                return;
            }
            switch (actionType)
            {
                case ActionType.Enter:
                    OnEnterStateEventList.Add(new EventState(action, parameters));
                    break;
                case ActionType.Update:
                    OnUpdateStateEventList.Add(new EventState(action, parameters));
                    break;
                case ActionType.Exit:
                    OnExitStateEventList.Add(new EventState(action, parameters));
                    break;
                default:
                    Debug.LogWarning("请填写正确的事件类型");
                    return;
            }
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="actionType">事件类型</param>
        /// <param name="action">事件</param>
        public void RemoveAction(ActionType actionType, Action<object[]> action)
        {
            if (isRun)
            {
                Debug.LogWarning("状态正在运行中，不允许移除事件");
                return;
            }
            switch (actionType)
            {
                case ActionType.Enter:
                    for (int i = 0; i < OnEnterStateEventList.Count; i++)
                    {
                        if (OnEnterStateEventList[i].action == action)
                        {
                            OnEnterStateEventList.Remove(OnEnterStateEventList[i]);
                            break;
                        }
                    }
                    Debug.LogWarning("不存在此事件，无法删除");
                    break;
                case ActionType.Update:
                    for (int i = 0; i < OnUpdateStateEventList.Count; i++)
                    {
                        if (OnUpdateStateEventList[i].action == action)
                        {
                            //移除
                            OnUpdateStateEventList.Remove(OnUpdateStateEventList[i]);
                            break;
                        }
                    }
                    Debug.LogWarning("不存在此事件，无法删除");
                    break;
                case ActionType.Exit:
                    for (int i = 0; i < OnExitStateEventList.Count; i++)
                    {
                        if (OnExitStateEventList[i].action == action)
                        {
                            OnExitStateEventList.Remove(OnExitStateEventList[i]);
                            break;
                        }
                    }
                    Debug.LogWarning("不存在此事件，无法删除");
                    break;
                default:
                    Debug.LogWarning("请填写正确的事件类型");
                    return;
            }
        }
        
        /// <summary>
        /// 进入状态
        /// </summary>
        public virtual void Run()
        {
            //执行进入事件
            if (OnEnterStateEventList.Count > 0)
            {
                foreach (var item in OnEnterStateEventList)
                {
                    item.action(item.parameters);
                }
            }
            
            //将更新事件添加到MonoHelper
            foreach (var item in OnUpdateStateEventList)
            {
                MonoHelper.Instance.AddUpdateEvent(item);
            }
        }

        /// <summary>
        /// 离开状态
        /// </summary>
        public virtual void Stop()
        {
            //将更新事件从MonoHelper移除
            foreach (var item in OnUpdateStateEventList)
            {
                MonoHelper.Instance.RemoveUpdateEvent(item);
            }
            
            //执行退出事件
            if (OnExitStateEventList.Count > 0)
            {
                foreach (var item in OnExitStateEventList)
                {
                    item.action(item.parameters);
                }
            }
        }
    }
}
