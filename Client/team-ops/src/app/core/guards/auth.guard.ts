import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { UserStore } from '../../store/user.store';

export const authGuard: CanActivateFn = (route, state) => {
  const userStore = inject(UserStore)

  if (userStore.getState().isAuthenticated) return true;
  else {
    return false
  }
};
