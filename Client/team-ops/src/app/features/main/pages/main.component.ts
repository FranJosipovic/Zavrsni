import { Component } from '@angular/core';
import { UserStore } from '../../../store/user.store';
import { ActivatedRoute, ChildActivationEnd, NavigationEnd, Router } from '@angular/router';
import { User } from '../../../core/user/interfaces';
import { filter, tap } from 'rxjs';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss',
})
export class MainComponent {
  public userState: User;
  breadcrumbs: string[] = [];
  constructor(
    private userStore: UserStore,
    private router: Router,
    private activatedRoute:ActivatedRoute
  ) {
    this.userState = this.userStore.getState().user!;

    this.router.events
      .pipe(
        filter(e => e instanceof NavigationEnd)
      )
      .subscribe((data:any) => {
        console.log('route',activatedRoute.children);
      })
  }

  signout() {
    this.userStore.signout();
    this.router.navigateByUrl('');
  }
}
