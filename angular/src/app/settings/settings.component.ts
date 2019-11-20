import { Component, OnInit, Input } from '@angular/core';
import { HttpService } from '../http.service';
import { Cookie } from 'ng2-cookies/ng2-cookies';
import { Router } from '@angular/router';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css'],
  providers: [HttpService]
})


export class SettingsComponent implements OnInit {
  username: string;
  password: string;

  constructor(private httpService: HttpService, private router: Router) { }

  ngOnInit() {
    this.username = Cookie.get('username');
  }

  setValue(password: string): void {
    this.httpService.postRequest('api/account/setaccountpassword', {"username": Cookie.get('username'), "password": password}).subscribe(
      (responce) => alert(responce),
      error => console.log(error));
  }

  logout(): void {
    Cookie.delete('token');
    this.router.navigate(['/login']);
  }
}