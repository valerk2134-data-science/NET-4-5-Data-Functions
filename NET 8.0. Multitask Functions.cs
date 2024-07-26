﻿using System;
using System.Threading;
using System.Threading.Tasks;




namespace MultitaskFunctionsNamespace
{

    // Note made in Warsaw. Workplace. 2024.07.08 09:21. 
    // while(ture) {} loads cpu at 25% that is full core from 4 cores. CPU was with 4 cores.
    // using Sleep(10) reduced load of CPU to 0 (load was not enough to show 1%).


    public class MultitaskFunctions
    {
        public static Task CreateTask(Action method_in)
        {
            Task task_out = new Task(method_in);
            return task_out;
        }
        public static Thread CreateThread(Action method_in)
        {
            Thread thread_out = new Thread(new ThreadStart(method_in));
            return thread_out;
        }
        public static bool AllThreadCompleted(Thread[] thread_arr_in)
        {
            bool all_tasks_end = false;
            while (all_tasks_end == false)
            {
                all_tasks_end = true;
                foreach (Thread thread in thread_arr_in)
                {
                    if (thread.IsAlive == true)
                    {
                        all_tasks_end = false;
                        break;
                    }                    
                }
            }
            return true;
        }
        public static bool AllTasksCompleted(Task[] task_arr_in)
        {
            bool all_tasks_end = false;
            while (all_tasks_end == false)
            {
                all_tasks_end = true;
                foreach (Task task in task_arr_in)
                {
                    if (task.IsCompleted == false)
                    {
                        all_tasks_end = false;
                        break;
                    }
                }
            }
            return true;
        }
        public static void AllThreadsStart(Thread[] thread_arr_in)
        {
            foreach (Thread thread in thread_arr_in)
            {
                thread.Start();
            }
        }
        public static void AllTasksStart(Task[] task_arr_in)
        {
            foreach (Task task in task_arr_in)
            {
                task.Start();
            }
        }
    }
    public class TaskArray_Type
    {
        public TaskArray_Type() { }
        Task[] task_array = new Task[0];
        public Task[] TaskArray
        {
            get
            {
                return task_array;
            }
        }
        public void TaskAdd(Task task_in)
        {
            Task[] task_arr_out = new Task[task_array.Length + 1];
            for (Int32 i = 0; i < task_array.Length; i++)
            {
                task_arr_out[i] = task_array[i];
            }
            task_arr_out[task_arr_out.Length - 1] = task_in;
            task_array = task_arr_out;
        }
    }
    public class ThreadArray_Type
    {
        public ThreadArray_Type() { }
        Thread[] thread_array = new Thread[0];
        public Thread[] ThreadArray
        {
            get
            {
                return thread_array;
            }
        }
        public void ThreadAdd(Thread thread_in)
        {
            Thread[] task_arr_out = new Thread[thread_array.Length + 1];
            for (Int32 i = 0; i < thread_array.Length; i++)
            {
                task_arr_out[i] = thread_array[i];
            }
            task_arr_out[task_arr_out.Length - 1] = thread_in;
            thread_array = task_arr_out;
            Random dffd = new Random();
        }
    }
}