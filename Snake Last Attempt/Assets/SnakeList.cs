using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SnakeList<T> {
    public SnakeNode head;
    public SnakeNode tail;
    public int count;

    public SnakeList() {
        head = null;
        tail = null;
        count = 0;
    }

    public class SnakeNode {
        public T data;
        public SnakeNode prev;
        public int index;

        public SnakeNode(T d, SnakeNode p, int i) {
            data = d;
            prev = p;
            index = i;
        }
    }

    public void Add(T data) {
        SnakeNode snakeNode = new SnakeNode(data, tail, count);

        if (count == 0) {
            head = snakeNode;
            tail = snakeNode;
        }

        else {
            tail = snakeNode;
        }

        count++;
    }

    public void Clear()
    {
        head = null;
        tail = null;
        count = 0;
    }
}


