# FSMFrame-For-Unity
专为Unity项目的使用的FSM有限状态机框架

## 文件结构
EventState.cs
  事件和参数封装
  事件类型枚举
State.cs
  状态构造
  状态绑定/移除事件
  状态运行/停止
StateMachine.cs
  状态机构造
  状态机绑定默认子状态
  状态机绑定过渡状态及条件
  状态机的运行/停止
MonoHelper.cs
  unity更新事件执行协程
## 使用
1. unity场景挂载cs文件
需挂载MonoHelper.cs，可调整更新事件的更新时间间隔，默认1帧更新一次
2. 业务类中使用
   - using FSMFrame
   - 创建总状态机
       - 状态机对象(StateMachine)可以理解为可包含一个默认状态的盒子，自己也是一种状态，它继承于状态对象(State)
           - 因此状态机也可以添加自己的进入、更新、退出事件
       - 状态机运行时，会首先运行自己的进入事件，同时运行默认状态的进入事件
       - 状态机运行时会轮询自己绑定的过渡条件
           - 如果当前运行的子状态满足了过渡子状态对应的过渡条件，会将子状态停止，并运行对应的过渡状态
   - 创建子状态/子状态机
       - 为了在状态机运行时，同时运行另一种状态，可以添加自己的子状态
           - 同理，子状态也可以是状态机
       - 为了在子状态运行时，可以根据一定条件切换为其他子状态，可以在状态机中为两个子状态建立过渡关系
   - 给总状态机/子状态绑定进入/更新/退出事件
       - 状态机/状态都可以有自己的进入/更新/退出事件
       - 进入/更新/退出事件可以绑定一个/多个方法
   - 给状态机绑定默认状态
       - 每一个状态机，如果要同时执行子状态就必须绑定默认状态为对应的子状态
   - 给状态机绑定过渡关系
       - 每一个状态机，如果其子状态有多种可能的状态，可以通过绑定过渡关系来实现过渡
   - 运行总状态机
## 示例
FsmTest2.cs
```
using UnityEngine;
using FSMFrame;

public class FsmTest2 : MonoBehaviour
{
    //创建总状态机
    private StateMachine _leader;

    //子状态
    private StateMachine _walk;
    private State _idle;
    private State _sneak;
    private State _run;

    //条件
    [Header("速度条件")] [Range(0, 10)] public float speed;

    private void Start()
    {
        //初始化
        _leader = new StateMachine();
        _walk = new StateMachine();
        _idle = new State();
        _sneak = new State();
        _run = new State();

        //初始化事件
        _leader.AddAction(ActionType.Enter, PrintLog, new []{"_leader", "Enter"});
        _leader.AddAction(ActionType.Update, PrintLog, new []{"_leader", "Update"});
        _leader.AddAction(ActionType.Exit, PrintLog, new []{"_leader", "Exit"});
        _walk.AddAction(ActionType.Enter, PrintLog, new []{"_walk", "Enter"});
        _walk.AddAction(ActionType.Update, PrintLog, new []{"_walk", "Update"});
        _walk.AddAction(ActionType.Exit, PrintLog, new []{"_walk", "Exit"});
        _idle.AddAction(ActionType.Enter, PrintLog, new []{"_idle", "Enter"});
        _idle.AddAction(ActionType.Update, PrintLog, new []{"_idle", "Update"});
        _idle.AddAction(ActionType.Exit, PrintLog, new []{"_idle", "Exit"});
        _sneak.AddAction(ActionType.Enter, PrintLog, new []{"_sneak", "Enter"});
        _sneak.AddAction(ActionType.Update, PrintLog, new []{"_sneak", "Update"});
        _sneak.AddAction(ActionType.Exit, PrintLog, new []{"_sneak", "Exit"});
        _run.AddAction(ActionType.Enter, PrintLog, new []{"_run", "Enter"});
        _run.AddAction(ActionType.Update, PrintLog, new []{"_run", "Update"});
        _run.AddAction(ActionType.Exit, PrintLog, new []{"_run", "Exit"});
        
        //绑定默认状态
        _leader.SetDefaultSonState(_idle);
        _walk.SetDefaultSonState(_sneak);

        //创建过渡关系
        _leader.AddTransitionManage(_idle, _walk, () => speed > 1);
        _leader.AddTransitionManage(_walk, _idle, () => speed <= 1);
        _walk.AddTransitionManage(_sneak, _run, () => speed > 5);
        _walk.AddTransitionManage(_run, _sneak, () => speed <= 5);

        //开始总状态机
        _leader.Run();
    }

    private void PrintLog(object[] objects)
    {
        string content = "";
        if (objects != null)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                content += "," + objects[i];
            }
            
        }

        Debug.Log($"开始打印：{content}");
    }
}
```

![image](https://github.com/7habit/FSMFrame-For-Unity/assets/16428251/663d9ecf-fc5b-4ead-9e57-61fe406a21d3)


