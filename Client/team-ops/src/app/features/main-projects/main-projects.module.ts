import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainProjectsRoutingModule } from './main-projects-routing.module';
import { GetFirstLetter } from '../../core/pipes/first-letter.pipe';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    MainProjectsRoutingModule,
    GetFirstLetter
  ]
})
export class MainProjectsModule { }
