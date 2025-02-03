import { Injectable } from '@angular/core';
import { User } from './user-endpoint.service';

@Injectable({
    providedIn: 'root'
})
export class UserManagerService {
  private currentUser: User | null = null;
  
    setCurrentUser(username: User): void {
        this.currentUser = username;
    
        localStorage.setItem('currentUser', JSON.stringify(username));
    
        const token = this.generateAuthToken(username);
        this.setToken(token);
      }
    
      getCurrentUser(): User | null {
        if(!this.isAuthenticated()){
          return null;
        }
    
        if (this.currentUser === null) {
          const storedUser = localStorage.getItem('currentUser');
          if (storedUser) {
            this.currentUser = JSON.parse(storedUser);
          }
        }
    
        return this.currentUser;
      }
    
      private setToken(token: string): void {
        const data = {
          value: token,
          timestamp: new Date().getTime()
        };
        localStorage.setItem('authToken', JSON.stringify(data));
      }
    
      private getToken(): string | null {
        return localStorage.getItem('authToken');
      }
    
      private generateAuthToken(user: User): string {
        // This is a simple example. In a real application, you would use a more secure method to generate the token.
        return btoa(`${user.id}:${user.name}`);
      }
    
      private clearToken(): void {
        localStorage.removeItem('authToken');
        localStorage.removeItem('currentUser');
      }
    
      private isAuthenticated(): boolean {
        const tokenData = this.getToken();
        if (!tokenData) {
          return false;
        }
    
        const data = JSON.parse(tokenData);
        const timestamp = data.timestamp;
        const age = new Date().getTime() - timestamp;
        if (age > 1000 * 60 * 60) {
          this.clearToken();
          return false;
        }
    
        return true;
      }
}