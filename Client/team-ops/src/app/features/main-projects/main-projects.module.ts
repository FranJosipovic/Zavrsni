import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainProjectsRoutingModule } from './main-projects-routing.module';
import { GetFirstLetter } from '../../core/pipes/first-letter.pipe';
import { WikiComponent } from './pages/overview/wiki/wiki.component';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIcon } from '@angular/material/icon';
import {MatTreeModule} from '@angular/material/tree';
import { CreateUpdateWikiComponent } from './pages/overview/wiki/create-update-wiki/create-update-wiki.component';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    WikiComponent, CreateUpdateWikiComponent
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
    ReactiveFormsModule
  ]
})
export class MainProjectsModule { }
