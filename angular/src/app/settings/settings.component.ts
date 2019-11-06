import { Component, OnInit, Input } from '@angular/core';
import { HttpService } from '../http.service';
import { UserJSON } from '../models/user';
import { Cookie } from 'ng2-cookies/ng2-cookies';
import { Router } from '@angular/router';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css'],
  providers: [HttpService]
})


export class SettingsComponent implements OnInit {
  userData: UserJSON;
  password: string = '#password';

  constructor(private httpService: HttpService, private router: Router) { }

  ngOnInit() {
    this.httpService.getRequest('api/auth/GetDummyUserData').subscribe((responce:UserJSON) => this.userData = responce);
  }

  setValue(password){
  }

  logout(){
    Cookie.delete('token');
    this.router.navigate(['/login']);
  }
}