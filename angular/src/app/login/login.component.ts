import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { UserJSON } from '../models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: [HttpService]
})
export class LoginComponent implements OnInit {
  userData: UserJSON;
  password: string;
  login:string;
  error:string = "r u rly authed?";

  constructor(private httpService: HttpService, private router: Router) { }

  ngOnInit() {
  }

  auth(password, login){
    document.body.querySelector("#auth").innerHTML = "Processing...";
    document.body.querySelector("#processing").classList.toggle("active");
    this.httpService.postRequest("api/message", {"username": login, "password": password}).subscribe((responce) => responce == "authed" ? this.router.navigate(['base']) : function(){console.log(responce); this.error = "wrong answer";});
  }
}