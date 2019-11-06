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
    function qweq(qw){
      console.log("123", JSON.parse(qw));
      this.userData = JSON.parse(qw);
    }

    this.httpService.getRequest('api/Test').subscribe(responce => qweq(responce));
    console.log(this.password);
    this.httpService.getRequest('api/Test').subscribe((responce) => function(){
      console.log('test', responce);
      //this.userData = JSON.parse(responce);
    }, error => console.error(error));
  }

  setValue(password){
    this.password = password;
  }

  logout(){
    Cookie.delete('token');
    this.router.navigate(['/login']);
  }
}