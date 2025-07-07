import { Component, OnInit } from '@angular/core';
import { Task, TaskService } from '../../services/task.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css'],
})
export class TaskListComponent implements OnInit {
  tasks: Task[] = [];
  newTaskTitle = '';

  constructor(private taskService: TaskService) {}

  ngOnInit() {
    this.loadTasks();
  }

  loadTasks() {
    this.taskService.getTasks().subscribe((tasks) => (this.tasks = tasks));
  }

  addTask() {
    if (!this.newTaskTitle.trim()) return;
    this.taskService
      .addTask({ title: this.newTaskTitle, isCompleted: false })
      .subscribe(() => {
        this.newTaskTitle = '';
        this.loadTasks();
      });
  }

  updateTask(task: Task) {
    this.taskService.updateTask(task).subscribe();
  }

  deleteTask(id: number) {
    this.taskService.deleteTask(id).subscribe(() => this.loadTasks());
  }
}
