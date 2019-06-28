using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Reducers
{
   public static TodoListState ListReduce(TodoListState prevState, object action)
   {
      var currentList = prevState.todoList;
      var currentState = prevState.showState;
      switch (action)
      {
         case Actions.AddAction _:
            var addItem = (action as Actions.AddAction).item;
            
            currentList.Add(addItem);
            SaveData(currentList);
            return new TodoListState(currentList,currentState);
            break;
         case Actions.UpdateFinishAction _:
            var updateItem = (action as Actions.UpdateFinishAction).item;
            updateItem.isFinish = !(updateItem.isFinish);
            SaveData(currentList);
            return prevState;
         case Actions.DeleteAction _:
            var removeItem = (action as Actions.DeleteAction).item;
            currentList.Remove(removeItem);
            SaveData(currentList);
            return new TodoListState(currentList,currentState);
         default:
            return prevState;
      }
   }

   public static void SaveData(List<TodoModel> list)
   {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (var item in list)
      {
         string itemStr = item.content + "@@@@" + item.timestamp.ToString() + "@@@@" + item.isFinish.ToString();
         stringBuilder.Append(itemStr);
         stringBuilder.Append("||||");
      }
      stringBuilder.Remove(stringBuilder.Length - 4, 4);
      PlayerPrefs.SetString(Constant.LIST_DATA_KEY,stringBuilder.ToString());
   }
}
