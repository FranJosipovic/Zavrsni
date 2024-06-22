import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './features/main/pages/main.component';
import { SigninComponent } from './features/auth/pages/signin/signin.component';
import { SignupComponent } from './features/auth/pages/signup/signup.component';
import { authGuard } from './core/guards/auth.guard';
import { OrganizationsComponent } from './features/main/pages/organizations/organizations.component';
import { OrganizationSettingsComponent } from './features/main/pages/organization-settings/organization-settings.component';
import { organizationIdResolver } from './core/resolver/resolvers/organization-id.resolver';

const routes: Routes = [
  {
    path: 'main',
    component: MainComponent,
    canActivate: [authGuard],
    children: [
      {
        path: '',
        component: OrganizationsComponent,
      },
      {
        path: ':organizationName/settings',
        component: OrganizationSettingsComponent,
        data: {
          breadcrumbs: {
            alias: 'Settings',
          },
        },
        resolve: {
          ids: organizationIdResolver,
        },
      },
      {
        path: ':organizationName/:projectName',
        loadChildren: () =>
          import('./features/main-projects/main-projects.module').then(
            (m) => m.MainProjectsModule 
          ),
      },
    ],
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
