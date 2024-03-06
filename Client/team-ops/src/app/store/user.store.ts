import { Inject, Injectable } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { User, UserAuthState } from '../core/user/interfaces';

@Injectable({
  providedIn: 'root',
})
export class UserStore {
  private ls: Storage;

  constructor(@Inject(DOCUMENT) private document: Document) {
    this.ls = document.defaultView?.localStorage!;
  }

  signIn(user: User, token: string) {
    this.ls?.setItem(
      'state',
      JSON.stringify({ token: token, user: user, isAuthenticated: true })
    );
  }

  getState(): UserAuthState {
    const storedState = this.ls?.getItem('state');
    let state = storedState
      ? JSON.parse(storedState)
      : {
          isAuthenticated: false,
          user: null,
          token: null,
        };
    return state;
  }

  getToken(): string | null {
    const storedState = this.ls?.getItem('state');
    if (storedState == null) return null;
    let state: UserAuthState = JSON.parse(storedState);
    return state.token;
  }

  logout(){
    this.ls?.removeItem('state')
  }
}
