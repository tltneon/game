import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { TopBarComponent } from './top-bar/top-bar.component';
import { ProductListComponent } from './product-list/product-list.component';
import { BattleComponent } from './battle/battle.component';
import { BaseComponent } from './base/base.component';
import { StatsComponent } from './stats/stats.component';
import { SettingsComponent } from './settings/settings.component';
import { AdminComponent } from './admin/admin.component';
import { LoginComponent } from './login/login.component';

@NgModule({
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: ProductListComponent },
      { path: 'login', component: ProductListComponent },
      { path: 'base', component: BaseComponent },
      { path: 'battles', component: BattleComponent },
      { path: 'stats', component: StatsComponent },
      { path: 'settings', component: SettingsComponent },
      { path: 'admin', component: AdminComponent },
    ])
  ],
  declarations: [
    AppComponent,
    TopBarComponent,
    ProductListComponent,
    BattleComponent,
    BaseComponent,
    StatsComponent,
    SettingsComponent,
    AdminComponent,
    LoginComponent
  ],
  bootstrap: [ AppComponent ]
})
export class AppModule { }


/*
Copyright Google LLC. All Rights Reserved.
Use of this source code is governed by an MIT-style license that
can be found in the LICENSE file at http://angular.io/license
*/