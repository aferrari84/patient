import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule }    from '@angular/common/http';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

import {MatButtonModule, MatCheckboxModule} from '@angular/material';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatSidenavModule} from '@angular/material/sidenav';

import { Helpers } from './helpers/helpers';
import { AuthGuard } from './helpers/canActivateAuthGuard';
import { AppConfig } from './config/config';

import { AppComponent } from './layout/app.component';
import { AppRoutingModule }     from './app-routing.module';

import { LoginComponent }      from './components/login/login.component';
import { LogoutComponent }      from './components/login/logout.component';
import { DashboardComponent }   from './components/dashboard/dashboard.component';
import { UsersComponent }      from './components/users/users.component';
import { HeadComponent }      from './layout/head.component';
import { LeftPanelComponent }      from './layout/left-panel.component';

import { BaseService }          from './services/base.service';
import { TokenService }          from './services/token.service';
import { UserService }          from './services/user.service';


@NgModule({
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MatButtonModule, 
    MatCheckboxModule,
    MatInputModule,
    MatFormFieldModule,
    MatSidenavModule,
    AppRoutingModule,
    HttpClientModule
  ],
  declarations: [
    AppComponent,
    HeadComponent,
    LeftPanelComponent,
    LoginComponent,
    LogoutComponent,
    DashboardComponent,
    UsersComponent
  ],
  providers: [
    AppConfig,
    Helpers,
    AuthGuard,
    BaseService,
    TokenService,
    UserService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
