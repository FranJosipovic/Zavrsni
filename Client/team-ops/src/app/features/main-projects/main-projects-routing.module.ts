import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProjectComponent } from './pages/project/project.component';
import { organizationProjectIdResolver } from '../../core/resolver/resolvers/organization-project-id.resolver';
import { SummaryComponent } from './pages/overview/summary/summary.component';
import { WikiComponent } from './pages/overview/wiki/wiki.component';
import { WorkItemsComponent } from './pages/boards/work-items/work-items.component';
import { SprintsComponent } from './pages/boards/sprints/sprints.component';

const routes: Routes = [
  {
    path: '',
    component: ProjectComponent,
    resolve: {
      ids: organizationProjectIdResolver,
    },
    children: [
      {
        path: 'overview/summary',
        component: SummaryComponent,
      },
      { path: 'overview/wiki', component: WikiComponent },
      {
        path: 'boards/work-items',
        component: WorkItemsComponent,
      },
      { path: 'boards/sprints', component: SprintsComponent }
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MainProjectsRoutingModule {}
