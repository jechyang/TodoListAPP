using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Unity.UIWidgets;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

public class TodoListApp : UIWidgetsPanel
{
    protected override Widget createWidget()
    {
        var store = new Store<TodoListState>(Reducers.ListReduce, new TodoListState());
        return new StoreProvider<TodoListState>(
            store: store,
            child: new MaterialApp(
                home: new Scaffold(
                    appBar: new AppBar(title: new Text("Todo List App")),
                    drawer: new Drawer(),
                    body: new TodoListPage()
                ),
                theme: new ThemeData(
                    primarySwatch: Colors.blue
                )
            )
        );
    }

    protected override void OnEnable()
    {
        FontManager.instance.addFont(Resources.Load<Font>(path: "MaterialIcons-Regular"), "Material Icons");
        base.OnEnable();
    }
}

public class TodoListPage : StatelessWidget
{
    private string mCurrentInptu = string.Empty;

    public override Widget build(BuildContext context)
    {
        var inputController = new TextEditingController();
        return new Column(
            children: new List<Widget>()
            {
                new Padding(
                    padding: EdgeInsets.only(top: 20)
                ),
                new StoreConnector<TodoListState, object>(
                    converter: (state => null),
                    builder: ((buildContext, _, dispatcher) =>
                    {
                        return new Flex(
                            direction: Axis.horizontal,
                            children: new List<Widget>()
                            {
                                new Expanded(
                                    flex: 6,
                                    child: new TextField(
                                        controller: inputController,
                                        decoration: new InputDecoration(hintText: "请输入待办事项"),
                                        onSubmitted: (value =>
                                        {
                                            TodoModel item = new TodoModel(value,
                                                long.Parse(TimeHelper.GetTimeStamp(DateTime.Now)),
                                                false);
                                            Actions.AddAction action = new Actions.AddAction(item);
                                            dispatcher.dispatch(action);
                                            inputController.clear();
                                        }),
                                        onChanged: (value => mCurrentInptu = value)
                                    )
                                ),
                                new Expanded(
                                    flex: 1,
                                    child: new IconButton(icon: new Icon(Icons.add),
                                        onPressed: (() =>
                                        {
                                            TodoModel item = new TodoModel(mCurrentInptu,
                                                long.Parse(TimeHelper.GetTimeStamp(DateTime.Now)),
                                                false);
                                            Actions.AddAction action = new Actions.AddAction(item);
                                            dispatcher.dispatch(action);
                                            mCurrentInptu = string.Empty;
                                            inputController.clear();
                                        })
                                    )
                                )
                            }
                        );
                    })
                ),
                new Expanded(
                    child: new StoreConnector<TodoListState, List<TodoModel>>(
                        converter: (state =>
                        {
                            var showState = state.showState;
                            var list = state.todoList;
                            switch (showState)
                            {
                                case ShowState.All:
                                    return list;
                                case ShowState.Finished:
                                    return list.Where((mo => mo.isFinish)).ToList();
                                case ShowState.UnFinished:
                                    return list.Where((mo => (!mo.isFinish))).ToList();
                                default:
                                    return list;
                            }
                        }),
                        builder: ((buildContext, model, dispatcher) =>
                        {
                            return ListView.seperated(
                                itemCount: model.Count,
                                itemBuilder: ((context1, index) =>
                                {
                                    var item = model[index];
                                    var iconColor = item.isFinish ? Colors.blue : Colors.grey;
                                    return new ListTile(title: new Text(item.content),
                                        subtitle: new Text(TimeHelper.ConvertStringToDateTime(item.timestamp.ToString())
                                            .ToString()),
                                        leading: new IconButton(icon: new Icon(Icons.check), iconSize: 20,
                                            color: iconColor,
                                            onPressed: (() =>
                                            {
                                                dispatcher.dispatch(new Actions.UpdateFinishAction(item));
                                            })),
                                        trailing: new IconButton(icon: new Icon(Icons.delete), iconSize: 20,
                                            onPressed: (() => { dispatcher.dispatch(new Actions.DeleteAction(item)); }))
                                    );
                                }),
                                separatorBuilder: ((context1, index) => { return new Divider(color: Colors.blue); })
                            );
                        })
                    )
                )
            },
            mainAxisSize: MainAxisSize.min
        );
    }
}