import { Component } from '@angular/core';
import { UserStore } from '../../../store/user.store';
import { Router } from '@angular/router';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss',
})
export class MainComponent {
  constructor(private userStore: UserStore,private router:Router) {
    console.log(this.userStore.getState());
  }
  logout(){
    this.userStore.logout()
    this.router.navigateByUrl('')
  }
}
