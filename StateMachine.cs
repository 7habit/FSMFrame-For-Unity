using System.Collections.Generic;
using UnityEngine;
using System;

namespace FSMFrame
{
    public class StateMachine : State
    {
        /// <summary>
        /// 过渡管理类
        /// </summary>
        class TransitionManage
        {
            //原状态
            public State originState { get; }

            //目标状态
            public State targetState { get; }

            //过渡条件
            public Func<bool> condition { get; }

            public TransitionManage(State originState, State targetState, Func<bool> condition)
            {
                this.originState = originState;
                this.targetState = targetState;
                this.condition = condition;
            }
        }

        //被管理的状态
        // private readonly List<State> _managedStateList;
        //默认状态
        private State _defaultSonState;

        //当前状态
        private State _currentSonState;

        //过渡管理列表
        private List<TransitionManage> _transitionManageList;

        //构造
        public StateMachine() : base()
        {
            // _managedStateList = new System.Collections.Generic.List<State>();
            _transitionManageList = new List<TransitionManage>();
            //初始化函数
            InitMachineState();
        }

        //初始化函数
        private void InitMachineState()
        {
            //子状态过渡检测事件
            EventState checkTransitionEvent = new EventState(CheckStateTransition, null);
            //绑定子状态过渡检测到update
            OnUpdateStateEventList.Add(checkTransitionEvent);
        }

        /// <summary>
        /// 子状态过渡检测
        /// </summary>
        private void CheckStateTransition(object[] objects)
        {
            //遍历当前子状态过渡列表
            foreach (var item in _transitionManageList)
            {
                //找到满足条件的过渡状态
                if (item.originState == _currentSonState && item.condition())
                {
                    //过渡
                    TransitionState(item);
                }
            }
        }

        /// <summary>
        /// 执行过渡
        /// </summary>
        /// <param name="transitionManage">过渡管理</param>
        private void TransitionState(TransitionManage transitionManage)
        {
            //切换状态
            //让当前状态退出
            _currentSonState.Stop();
            //让切换的状态为当前状态
            _currentSonState = transitionManage.targetState;
            //让当前状态进入
            _currentSonState.Run();
        }


        /// <summary>
        /// 添加过渡
        /// </summary>
        /// <param name="originState">原状态</param>
        /// <param name="targetState">目标状态</param>
        /// <param name="condition">过渡条件</param>
        public void AddTransitionManage(State originState, State targetState, Func<bool> condition)
        {
            //已包含此过渡
            foreach (var item in _transitionManageList)
            {
                if (item.originState == originState && item.targetState == targetState)
                {
                    Debug.LogWarning("已存在此过渡条件，请不要重复添加");
                    return;
                }
            }

            //新建过度管理对象
            TransitionManage transitionManage = new TransitionManage(originState, targetState, condition);
            //添加过渡管理列表
            _transitionManageList.Add(transitionManage);
        }

        /// <summary>
        /// 移除过渡
        /// </summary>
        /// <param name="transitionName">过度名称</param>
        public void RemoveTransitionState(State originState, State targetState)
        {
            //已包含此过渡
            foreach (var item in _transitionManageList)
            {
                if (item.originState == originState && item.targetState == targetState)
                {
                    _transitionManageList.Remove(item);
                    return;
                }
            }

            Debug.LogWarning($"不包含此过渡条件，无法移除");
        }

        /// <summary>
        /// 设置默认子状态
        /// </summary>
        /// <param name="defaultSonState">默认子状态</param>
        public void SetDefaultSonState(State defaultSonState)
        {
            //正在运行
            if (isRun)
            {
                Debug.LogWarning("状态正在运行中，不允许添加事件");
                return;
            }

            //添加
            _defaultSonState = defaultSonState;
        }


        /// <summary>
        /// 重写进入状态
        /// </summary>
        public override void Run()
        {
            //状态机先进入
            base.Run();
            //子状态进入
            if (_defaultSonState != null)
            {
                //当前子状态为默认状态
                _currentSonState = _defaultSonState;
                //启动当前子状态
                _currentSonState.Run();
            }
        }

        /// <summary>
        /// 重写离开状态
        /// </summary>
        public override void Stop()
        {
            //子状态先离开
            if (_currentSonState != null)
            {
                //离开当前子状态
                _currentSonState.Stop();
            }

            //状态机离开
            base.Stop();
        }
    }
}