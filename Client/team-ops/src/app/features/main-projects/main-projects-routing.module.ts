import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProjectComponent } from './pages/project/project.component';
import { organizationProjectIdResolver } from '../../core/resolver/resolvers/organization-project-id.resolver';

const routes: Routes = [
  {
    path: '',
    component: ProjectComponent,
    resolve:{
      ids: organizationProjectIdResolver
    }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MainProjectsRoutingModule {}
