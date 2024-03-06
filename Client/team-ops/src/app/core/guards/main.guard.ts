import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { UserStore } from '../../store/user.store';

export const canActivateMain: CanActivateFn = (route, state) => {
  const userStore = inject(UserStore)
  const router = inject(Router);

  if (userStore.getState().isAuthenticated) return true;
  else {
    router.navigate(['/login']);
    return false
  }
};
