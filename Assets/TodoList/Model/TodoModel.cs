using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TodoModel
{
    public TodoModel(string content, long timestamp, bool isFinish)
    {
        this.content = content;
        this.timestamp = timestamp;
        this.isFinish = isFinish;
    }
    
    public string content { get; set; }
    public long timestamp { get; set; }
    public bool isFinish { get; set; }
}
