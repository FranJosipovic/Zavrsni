import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './features/main/pages/main.component';
import { SigninComponent } from './features/auth/pages/signin/signin.component';
import { canActivateMain } from './core/guards/main.guard';
import { SignupComponent } from './features/auth/pages/signup/signup.component';

const routes: Routes = [
  {
    path: 'main',
    component: MainComponent,
    canActivate: [canActivateMain],
  },
  {
    path: 'signin',
    component: SigninComponent,
  },
  {
    path: 'signup',
    component: SignupComponent,
  },
  { path: '', redirectTo: 'signin', pathMatch: 'full' },
  { path: '**', redirectTo: 'signin', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
