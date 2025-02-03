import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ChatSessionListComponent } from './chat-session-list/chat-session-list.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ChatSessionListComponent, FormsModule, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'chat-app';
}
