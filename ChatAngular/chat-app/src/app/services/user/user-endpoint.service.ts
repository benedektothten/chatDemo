import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface User {
    id: number;
    name: string;
  }
  
  export interface CreateUser{
    name: string;
    password: string;
  }

@Injectable({
    providedIn: 'root'
})
export class UserEndpointService {

    private apiUrl = `http://localhost:5128/api/Users`;

    constructor(private http: HttpClient) { }


    createNewUser(request: CreateUser): Observable<User>{
        return this.http.post<User>(`${this.apiUrl}/create`, request);
      }
    
      loginUser(request: CreateUser): Observable<User>{
        return this.http.post<User>(`${this.apiUrl}/authenticate`, request);
      }
}