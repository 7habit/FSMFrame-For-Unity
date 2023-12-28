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