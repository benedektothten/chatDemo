import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../user/user-endpoint.service';

export interface ChatSession {
  id: number;
  name: string;
  owner: User;
}

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private apiUrl = 'http://localhost:5128/api/chatsessions'; // Replace with your API URL


  constructor(private http: HttpClient) { }


  getChatSessions(userId: number): Observable<ChatSession[]> {
    return this.http.get<ChatSession[]>(this.apiUrl);
  }

}