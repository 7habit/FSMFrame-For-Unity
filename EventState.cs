using System;

namespace FSMFrame
{
    /// <summary>
    /// 事件类型枚举
    /// </summary>
    public enum ActionType{
        Enter, Update, Exit
    }
    
    /// <summary>
    /// 封装委托和参数
    /// </summary>
    public class EventState
    {
        //方法
        public Action<object[]> action { get; set; }
        //参数
        public object[] parameters { get; set; }

        public EventState(Action<object[]> action, object[] parameters)
        {
            this.action = action;
            this.parameters = parameters;
        }

    }
}