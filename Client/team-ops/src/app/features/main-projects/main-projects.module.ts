import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainProjectsRoutingModule } from './main-projects-routing.module';
import { GetFirstLetter } from '../../core/pipes/first-letter.pipe';
import { WikiComponent } from './pages/overview/wiki/wiki.component';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIcon } from '@angular/material/icon';
import { MatTreeModule } from '@angular/material/tree';
import { CreateUpdateWikiComponent } from './pages/overview/wiki/create-update-wiki/create-update-wiki.component';
import { ReactiveFormsModule } from '@angular/forms';
import { TruncatePipe } from '../../core/pipes/truncate.pipe';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { WorkItemsComponent } from './pages/boards/work-items/work-items.component';
import { CreateWorkItemComponent } from './pages/boards/dialogs/create-work-item/create-work-item.component';
import { AppModule } from '../../app.module';
import { WorkItemCardComponent } from './pages/boards/sprints/components/work-item-card/work-item-card.component';
import { SprintsComponent } from './pages/boards/sprints/sprints.component';
import { WorkItemDetailsDialogComponent } from './pages/boards/sprints/components/work-item-details-dialog/work-item-details-dialog.component';
import { InitialsPipe } from '../../core/pipes/initials.pipe';

@NgModule({
  declarations: [
    WikiComponent,
    CreateUpdateWikiComponent,
    WorkItemsComponent,
    CreateWorkItemComponent,
    WorkItemCardComponent,
    SprintsComponent,
    WorkItemDetailsDialogComponent
  ],
  imports: [
    CommonModule,
    MainProjectsRoutingModule,
    GetFirstLetter,
    CKEditorModule,
    MatSidenavModule,
    MatMenuModule,
    MatIcon,
    MatTreeModule,
    ReactiveFormsModule,
    MatTooltipModule,
    MatButtonModule,
    MatTableModule,
    ReactiveFormsModule,
    TruncatePipe,
    InitialsPipe
  ],
})
export class MainProjectsModule {}
