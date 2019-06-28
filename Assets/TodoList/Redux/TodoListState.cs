using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Unity.UIWidgets.foundation;
using UnityEngine;

public enum ShowState
{
    All,
    Finished,
    UnFinished
}

public class TodoListState
{
    public TodoListState()
    {
        var saveData = PlayerPrefs.GetString(Constant.LIST_DATA_KEY,"");
        List<TodoModel> result = new List<TodoModel>();
        if (saveData.isNotEmpty())
        {
            var itemStrArrs = saveData.Split(new string[1] {"||||"}, StringSplitOptions.None);
            result = itemStrArrs.Select(
                (itemStr =>
                {
                    var itemStrArr = itemStr.Split(new string[1] {"@@@@"}, StringSplitOptions.None);
                    TodoModel item = new TodoModel(itemStrArr[0],
                        long.Parse(itemStrArr[1]),
                        Boolean.Parse(itemStrArr[2])
                    );
                    return item;
                })
            ).ToList();
        }
        mState = ShowState.All;
        mTodoList = result;
    }
    
    public TodoListState(List<TodoModel> mTodoList,ShowState state)
    {
        this.mTodoList = mTodoList;
        this.mState = state;
    }

    private List<TodoModel> mTodoList;
    private ShowState mState;
    
    public List<TodoModel> todoList
    {
        get => mTodoList;
    }
    
    public ShowState showState
    {
        get => mState;
    }
}