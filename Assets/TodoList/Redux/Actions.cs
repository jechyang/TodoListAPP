using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions
{
   public class AddAction
   {
      public AddAction(TodoModel item)
      {
         this.item = item;
      }

      public TodoModel item { get; set; }
      
   }

   public class UpdateFinishAction
   {
      public UpdateFinishAction(TodoModel item)
      {
         this.item = item;
      }

      public TodoModel item { get; set; }
   }

   public class DeleteAction
   {
      public DeleteAction(TodoModel item)
      {
         this.item = item;
      }

      public TodoModel item { get; set; }
   }
}
