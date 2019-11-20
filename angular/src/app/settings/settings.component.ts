import { Component, OnInit, Input } from '@angular/core';
import { HttpService } from '../http.service';
import { Cookie } from 'ng2-cookies/ng2-cookies';
import { Router } from '@angular/router';
import { GameVars } from '../gamevars';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css'],
  providers: [HttpService, GameVars]
})


export class SettingsComponent implements OnInit {
  username: string;
  password: string;
  role: boolean;

  constructor(private httpService: HttpService, private router: Router, private gameVars: GameVars) { }

  ngOnInit() {
    this.username = Cookie.get('username');
    this.role = Cookie.get('role') ? true : false;
  }

  setValue(password: string): void {
    this.httpService.postRequest('api/account/setaccountpassword', {"username": Cookie.get('username'), "password": password}).subscribe(
      (responce: string) => alert(this.gameVars.getText(responce)),
      error => this.gameVars.registerError(error.message));
  }

  logout(): void {
    Cookie.delete('token');
    this.router.navigate(['/login']);
  }
}