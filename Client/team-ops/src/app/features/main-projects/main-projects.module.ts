import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainProjectsRoutingModule } from './main-projects-routing.module';
import { GetFirstLetter } from '../../core/pipes/first-letter.pipe';
import { WikiComponent } from './pages/overview/wiki/wiki.component';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';



@NgModule({
  declarations: [
    WikiComponent
  ],
  imports: [
    CommonModule,
    MainProjectsRoutingModule,
    GetFirstLetter,
    CKEditorModule
  ]
})
export class MainProjectsModule { }
