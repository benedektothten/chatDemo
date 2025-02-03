import { Component, OnInit } from '@angular/core';
import { ChatService, ChatSession } from '../services/chat/chat.service';
import {FormControl, FormsModule, ReactiveFormsModule} from '@angular/forms';
import { Observable } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { ListboxModule } from 'primeng/listbox';
import { MessageModule } from 'primeng/message';
import { PasswordModule } from 'primeng/password';
import { InputTextModule } from 'primeng/inputtext';
import { UserManagerService } from '../services/user/user.service';
import { UserEndpointService } from '../services/user/user-endpoint.service';

@Component({
  selector: 'app-chat-session-list',
  imports: [ReactiveFormsModule, FormsModule, AsyncPipe, ButtonModule, ListboxModule, MessageModule, PasswordModule, InputTextModule],
  templateUrl: './chat-session-list.component.html',
  styleUrls: ['./chat-session-list.component.css']
})
export class ChatSessionListComponent implements OnInit {
  username = new FormControl();
  password = new FormControl();
  chatSessions$!: Observable<ChatSession[]>;
  error: string | null = null;
  selectedSession: ChatSession | null = null;

  constructor(
    private chatService: ChatService,
    private userEndpointService: UserEndpointService,
    public userService: UserManagerService) { }

  ngOnInit(): void {
    const savedUser = this.userService.getCurrentUser();
    if (savedUser) {
      this.username.setValue(savedUser);
      //this.loadChatSessions();
    }
  }

  submitUsername(): void {
    if (!this.username.getRawValue().trim()) {
      this.error = 'Please enter a username';
      return;
    }

    if (!this.password.getRawValue().trim()) {
      this.error = 'Please enter a password';
      return;
    }
    
    this.userEndpointService.loginUser({name: this.username.getRawValue().trim(), password: this.password.getRawValue().trim()})
    .subscribe((user) => {
      this.userService.setCurrentUser(user);
      this.loadChatSessions(user.id);
    }, (error) => {
      this.error = 'Invalid username or password';
    });
  }

  createUser(): void{
    if (!this.username.getRawValue().trim()) {
      this.error = 'Please enter a username';
      return;
    }

    if (!this.password.getRawValue().trim()) {
      this.error = 'Please enter a password';
      return;
    }

    this.userEndpointService.createNewUser({name: this.username.getRawValue().trim(), password: this.password.getRawValue().trim()})
    .subscribe((user) => {
      this.userService.setCurrentUser(user);
      this.loadChatSessions(user.id);
    });
  }

  private loadChatSessions(userId: number): void {
    this.error = null;

    this.chatSessions$ = this.chatService.getChatSessions(userId);
  }
}