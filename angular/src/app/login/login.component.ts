import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { UserJSON } from '../models/user';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: [HttpService]
})
export class LoginComponent implements OnInit {
  userData: UserJSON;
  password: string = "api/message";
  login:string = "fpj34o0fi34jfi4fi";

  constructor(private httpService: HttpService) { }

  ngOnInit() {
    //, error => console.error(error));
  }

  makeGet(password, login){
    this.httpService.getData(login).subscribe((responce:string) => console.log(responce));
    console.log();
  }
  makePost(password, login){
    this.httpService.sendData(password, {"username": login}).subscribe((responce:string) => console.log(responce));
    console.log();
  }

}