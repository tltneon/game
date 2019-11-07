import { Component, OnInit, Input } from '@angular/core';
import { HttpService } from '../http.service';
import { AccountJSON } from '../models/account';
import { Cookie } from 'ng2-cookies/ng2-cookies';
import { Router } from '@angular/router';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css'],
  providers: [HttpService]
})


export class SettingsComponent implements OnInit {
  userData: AccountJSON;
  password: string = '#password';

  constructor(private httpService: HttpService, private router: Router) { }

  ngOnInit() {
    this.httpService.getRequest('api/account/getDummyUserData').subscribe(
      (responce:AccountJSON) => this.userData = responce,
      error => console.log(error));
  }

  setValue(password){
  }

  logout(){
    Cookie.delete('token');
    this.router.navigate(['/login']);
  }
}