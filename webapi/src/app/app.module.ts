import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { TopBarComponent } from './top-bar/top-bar.component';
import { MainPageComponent } from './mainpage/mainpage.component';
import { BattleComponent } from './battle/battle.component';
import { BaseComponent } from './base/base.component';
import { StatsComponent } from './stats/stats.component';
import { SettingsComponent } from './settings/settings.component';
import { AdminComponent } from './admin/admin.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth.guard';
import { AdminUsersComponent } from './admin/admin-users/admin-users.component';
import { AdminBasesComponent } from './admin/admin-bases/admin-bases.component';
import { AdminSettingsComponent } from './admin/admin-settings/admin-settings.component';

@NgModule({
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule.forRoot([
        { path: '', component: MainPageComponent, pathMatch: 'full' },
        { path: 'login', component: LoginComponent },
        { path: 'base', component: BaseComponent },
        { path: 'battles', component: BattleComponent },
        { path: 'stats', component: StatsComponent },
        { path: 'settings', component: SettingsComponent },
        { path: 'admin', component: AdminComponent,
          children: [
            {
              path: '',
              children: [
                { path: '', component: AdminComponent },
                { path: 'users', component: AdminUsersComponent, canActivate: [AuthGuard],  },
                { path: 'bases', component: AdminBasesComponent, canActivate: [AuthGuard],  },
                { path: 'settings', component: AdminSettingsComponent, canActivate: [AuthGuard],  }
              ],
            }
          ]
        },
       // { path: '**', redirectTo: '/' }
    ])
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