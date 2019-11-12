import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule, HttpClientJsonpModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { TopBarComponent } from './top-bar/top-bar.component';
import { MainPageComponent } from './mainpage/mainpage.component';
import { BattleComponent } from './battle/battle.component';
import { BaseComponent } from './base/base.component';
import { StatsComponent } from './stats/stats.component';
import { SettingsComponent } from './settings/settings.component';
import { AdminComponent } from './admin/admin.component';
import { LoginComponent } from './login/login.component';
import { AdminUsersComponent } from './admin/admin-users/admin-users.component';
import { AdminBasesComponent } from './admin/admin-bases/admin-bases.component';
import { AdminSettingsComponent } from './admin/admin-settings/admin-settings.component';

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    HttpClientJsonpModule
  ],
  declarations: [
    AppComponent,
    TopBarComponent,
    MainPageComponent,
    BattleComponent,
    BaseComponent,
    StatsComponent,
    SettingsComponent,
    AdminComponent,
    LoginComponent,
    AdminUsersComponent,
    AdminBasesComponent,
    AdminSettingsComponent
  ],
  bootstrap: [ AppComponent ]
})
export class AppModule { }