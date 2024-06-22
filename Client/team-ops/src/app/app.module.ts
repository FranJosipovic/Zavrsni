import { NgModule } from '@angular/core';
import {
  BrowserModule,
  provideClientHydration,
} from '@angular/platform-browser';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainComponent } from './features/main/pages/main.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SignupComponent } from './features/auth/pages/signup/signup.component';
import { SigninComponent } from './features/auth/pages/signin/signin.component';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { GetFirstLetter } from './core/pipes/first-letter.pipe';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { OrganizationsComponent } from './features/main/pages/organizations/organizations.component';
import { OrganizationSettingsComponent } from './features/main/pages/organization-settings/organization-settings.component';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ProjectComponent } from './features/main-projects/pages/project/project.component';
import { SummaryComponent } from './features/main-projects/pages/overview/summary/summary.component';
import { CreateOrganizationDialogComponent } from './features/main/pages/organizations/dialogs/create-organization-dialog/create-organization-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { CreateProjectDialogComponent } from './features/main/pages/organizations/dialogs/create-project-dialog/create-project-dialog.component';
import { TruncatePipe } from './core/pipes/truncate.pipe';
import { InitialsPipe } from './core/pipes/initials.pipe';
@NgModule({
  declarations: [
    AppComponent,
    MainComponent,
    SigninComponent,
    SignupComponent,
    OrganizationsComponent,
    OrganizationSettingsComponent,
    ProjectComponent,
    SummaryComponent,
    CreateOrganizationDialogComponent,
    CreateProjectDialogComponent,
  ],
  imports: [
    CommonModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    MatToolbarModule,
    GetFirstLetter,
    MatSidenavModule,
    MatMenuModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatTabsModule,
    MatTableModule,
    MatTooltipModule,
    MatDialogModule,
    MatButtonModule,
    TruncatePipe,
    InitialsPipe
  ],
  providers: [provideClientHydration(), provideAnimationsAsync()],
  bootstrap: [AppComponent],
})
export class AppModule {}
