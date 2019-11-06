import { Component, OnInit, Input } from '@angular/core';
import { HttpService } from '../http.service';
import { UserJSON } from '../models/user';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css'],
  providers: [HttpService]
})


export class SettingsComponent implements OnInit {
  userData: UserJSON;
  password: string = '2341232';

  constructor(private httpService: HttpService) { }

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
}